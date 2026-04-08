using UnityEngine;
using System;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public enum ExperimentStep { PourA, ShakeA, PourB, ShakeB, Complete }
    
    public static event Action<ExperimentStep> OnStepAdvanced;
    public static event Action OnExperimentComplete;

    public ExperimentStep CurrentStep { get; private set; } = ExperimentStep.PourA;

    private int pouredFlasksCount;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanInteract(int flaskId, FlaskState flaskState)
    {
        switch (CurrentStep)
        {
            case ExperimentStep.PourA:
                return flaskId == 0 && flaskState == FlaskState.Empty;
            case ExperimentStep.ShakeA:
                return flaskId == 0 && flaskState == FlaskState.HasLiquid;
            case ExperimentStep.PourB:
                return flaskId == 1 && flaskState == FlaskState.Empty;
            case ExperimentStep.ShakeB:
                return flaskId == 1 && flaskState == FlaskState.HasLiquid;
            default:
                return false;
        }
    }

    /// <summary>returns 0 for first poured flask, 1 for second.</summary>
    public int GetAndIncrementPourCount()
    {
        return pouredFlasksCount++;
    }

    public void NotifyPourComplete()
    {
        AdvanceStep();
    }

    public void NotifyShakeComplete()
    {
        AdvanceStep();
    }

    private void AdvanceStep()
    {
        if (CurrentStep >= ExperimentStep.Complete) return;

        CurrentStep++;

        OnStepAdvanced?.Invoke(CurrentStep);

        if (CurrentStep == ExperimentStep.Complete)
            OnExperimentComplete?.Invoke();
    }

    private void OnDestroy()
    {
        OnStepAdvanced = null;
        OnExperimentComplete = null;
    }
}
