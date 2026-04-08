using UnityEngine;
using System.Collections;

public class TestTube : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer liquidRenderer;

    [Header("Audio")]
    [SerializeField] private AudioClip pourClip;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Tilt")]
    [SerializeField] private float fullTiltAngle = 110f;
    [SerializeField] private float tiltSpeed = 120f;

    [Header("Pour")]
    [SerializeField] private float pourDuration = 1.5f;

    private const float FirstPourFill = 0.55f;
    private const float SecondPourFill = 1.0f;
    private const float FirstPourTiltRatio = 0.5f;
    private const float FallbackPourHeight = 1.5f;
    private const int MaxPours = 2;

    private static readonly int FillAmountID = Shader.PropertyToID("_FillAmount");

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Material instancedMaterial;
    private int poursDone;

    public bool IsPouring { get; private set; }

    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        if (!liquidRenderer)
        {
            Debug.LogError($"[TestTube] liquidRenderer not assigned on {gameObject.name}");
            return;
        }

        instancedMaterial = liquidRenderer.material;
    }

    public bool CanPour()
    {
        return !IsPouring && poursDone < MaxPours;
    }

    public void PourInto(Flask targetFlask)
    {
        if (!targetFlask)
        {
            Debug.LogError("[TestTube] PourInto called with null flask");
            return;
        }

        if (CanPour())
            StartCoroutine(PourRoutine(targetFlask));
    }

    private IEnumerator PourRoutine(Flask targetFlask)
    {
        IsPouring = true;

        Transform pourPoint = targetFlask.pourPoint;
        Vector3 pourPos = pourPoint != null
            ? pourPoint.position
            : targetFlask.transform.position + Vector3.up * FallbackPourHeight;

        //move to pour pos
        Vector3 startPos = transform.position;
        float t = 0f;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(startPos, pourPos, t);
            t += Time.deltaTime * moveSpeed;
            yield return null;
        }
        transform.position = pourPos;

        //tilt + pour simultaneously
        Quaternion startRot = transform.rotation;
        float currentTilt = (poursDone == 0) ? fullTiltAngle * FirstPourTiltRatio : fullTiltAngle;
        Quaternion targetRot = Quaternion.AngleAxis(currentTilt, Vector3.right) * startRot;
        float tiltDuration = Mathf.Abs(currentTilt / tiltSpeed);

        poursDone++;
        float targetFill = (poursDone == 1) ? FirstPourFill : SecondPourFill;
        float currentFill = (instancedMaterial && instancedMaterial.HasProperty(FillAmountID))
            ? instancedMaterial.GetFloat(FillAmountID) : 0f;

        float totalDuration = Mathf.Max(tiltDuration, pourDuration);

        if (pourClip && AudioManager.Instance) AudioManager.Instance.PlaySFX(pourClip);

        t = 0f;
        while (t < totalDuration)
        {
            float tiltT = Mathf.Clamp01(t / tiltDuration);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, tiltT);

            if (instancedMaterial && instancedMaterial.HasProperty(FillAmountID))
            {
                float fillT = Mathf.Clamp01(t / pourDuration);
                instancedMaterial.SetFloat(FillAmountID, Mathf.Lerp(currentFill, targetFill, fillT));
            }

            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRot;
        if (instancedMaterial && instancedMaterial.HasProperty(FillAmountID))
            instancedMaterial.SetFloat(FillAmountID, targetFill);

        //return to starting position
        Vector3 endPos = transform.position;
        Quaternion endRot = transform.rotation;
        t = 0f;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(endPos, startingPosition, t);
            transform.rotation = Quaternion.Lerp(endRot, startingRotation, t);
            t += Time.deltaTime * moveSpeed;
            yield return null;
        }
        transform.position = startingPosition;
        transform.rotation = startingRotation;

        IsPouring = false;
        targetFlask.OnReceiveLiquid();
    }

    private void OnDestroy()
    {
        if (instancedMaterial) Destroy(instancedMaterial);
    }
}
