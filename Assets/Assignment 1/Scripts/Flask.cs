using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public enum FlaskState { Empty, HasLiquid, Shaken }

public class Flask : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    public Renderer liquidRenderer;
    public Transform pourPoint;
    [SerializeField] private TestTube testTube;

    [Header("Identity")]
    [Tooltip("0 = Flask A, 1 = Flask B")]
    public int flaskId;

    [Header("Settings")]
    [SerializeField] private float shakeDuration = 2.0f;

    [Header("Feedback")]
    public ParticleSystem unpleasantSmellParticles;
    public AudioClip shakingClip;

    [Header("Liquid Color")]
    [SerializeField] private Color endColor;

    private const float ShakeFrequency = 10f;
    private const float ShakeAmplitude = 15f;
    private const float HoverScaleMultiplier = 1.05f;
    private const int Unassigned = -1;

    private static readonly int TintID     = Shader.PropertyToID("_Tint");
    private static readonly int TopColorID = Shader.PropertyToID("_TopColor");

    public FlaskState CurrentState { get; private set; } = FlaskState.Empty;

    private Vector3 defaultScale;
    private Material[] liquidMats;
    private Color startTint;
    private Color startTopColor;
    private bool isShaking;
    private int pourOrder = Unassigned;

    private void Start()
    {
        defaultScale = transform.localScale;

        if (!liquidRenderer)
        {
            Debug.LogError($"[Flask] liquidRenderer not assigned on {gameObject.name}");
            return;
        }

        liquidMats = liquidRenderer.materials;

        if (liquidMats.Length == 0)
        {
            Debug.LogError($"[Flask] No materials found on {gameObject.name}.");
            return;
        }

        startTint     = liquidMats[0].GetColor(TintID);
        startTopColor = liquidMats[0].GetColor(TopColorID);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!InteractionManager.Instance ||
            !InteractionManager.Instance.CanInteract(flaskId, CurrentState))
            return;

        if (CurrentState == FlaskState.Empty)
        {
            if (testTube && testTube.CanPour())
                testTube.PourInto(this);
        }
        else if (CurrentState == FlaskState.HasLiquid && !isShaking)
        {
            StartCoroutine(ShakeRoutine());
        }
    }

    public void OnReceiveLiquid()
    {
        CurrentState = FlaskState.HasLiquid;

        if (pourOrder == Unassigned && InteractionManager.Instance != null)
        {
            pourOrder = InteractionManager.Instance.GetAndIncrementPourCount();
            InteractionManager.Instance.NotifyPourComplete();
        }
    }

    private IEnumerator ShakeRoutine()
    {
        isShaking = true;
        float timeElapsed = 0f;

        if (shakingClip != null && AudioManager.Instance != null)
            AudioManager.Instance.PlayExclusiveSFX(shakingClip, true);

        Quaternion startRot = transform.rotation;

        while (timeElapsed < shakeDuration)
        {
            float angle = Mathf.Sin(Time.time * ShakeFrequency) * ShakeAmplitude;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.right) * startRot;

            //change color
            if (pourOrder == 0)
            {
                float t = timeElapsed / shakeDuration;
                SetColor(
                    Color.Lerp(startTint, endColor, t),
                    Color.Lerp(startTopColor, endColor, t)
                );
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = startRot;

        if (AudioManager.Instance != null)
            AudioManager.Instance.StopSFX();

        //final effect
        if (pourOrder == 0)
            SetColor(endColor, endColor);
        else if (pourOrder == 1 && unpleasantSmellParticles != null)
            unpleasantSmellParticles.Play();

        CurrentState = FlaskState.Shaken;
        isShaking = false;
        
        if (InteractionManager.Instance) InteractionManager.Instance.NotifyShakeComplete();
    }

    private void SetColor(Color tint, Color top)
    {
        if (liquidMats == null) return;

        foreach (var mat in liquidMats)
        {
            mat.SetColor(TintID, tint);
            mat.SetColor(TopColorID, top);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isShaking && CurrentState != FlaskState.Shaken)
            transform.localScale = defaultScale * HoverScaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = defaultScale;
    }

    private void OnDestroy()
    {
        if (liquidMats == null) return;
        foreach (var mat in liquidMats)
            Destroy(mat);
    }
}