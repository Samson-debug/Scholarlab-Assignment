using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimatorController : MonoBehaviour
{
    private static readonly int ExcitedHash   = Animator.StringToHash("Excited");
    private static readonly int IrritatedHash = Animator.StringToHash("Irritated");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        InteractionManager.OnStepAdvanced += HandleStepAdvanced;
    }

    private void OnDisable()
    {
        InteractionManager.OnStepAdvanced -= HandleStepAdvanced;
    }

    private void HandleStepAdvanced(InteractionManager.ExperimentStep step)
    {
        if (!animator) return;

        switch (step)
        {
            //after flask A shakes and excited animation is played
            case InteractionManager.ExperimentStep.PourB:
                animator.SetTrigger(ExcitedHash);
                break;
            
            //after flask B shakes and irritating animation is played
            case InteractionManager.ExperimentStep.Complete:
                animator.SetTrigger(IrritatedHash);
                break;
        }
    }
}
