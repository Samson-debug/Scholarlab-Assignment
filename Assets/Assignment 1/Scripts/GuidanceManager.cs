using UnityEngine;
using TMPro;

public class GuidanceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI guidanceText;

    [Header("Settings")]
    [SerializeField] private string jsonFileName = "guidance_steps";

    private string[] steps;
    private int currentStep;

    [System.Serializable]
    private class GuidanceData
    {
        public string[] steps;
    }

    private void OnEnable()
    {
        InteractionManager.OnStepAdvanced += HandleStepAdvanced;
    }

    private void OnDisable()
    {
        InteractionManager.OnStepAdvanced -= HandleStepAdvanced;
    }

    private void Start()
    {
        LoadSteps();
        ShowCurrentStep();
    }

    private void LoadSteps()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (!jsonFile)
        {
            Debug.LogError($"[GuidanceManager] JSON file '{jsonFileName}' not found in Resources");
            steps = new string[] { "No guidance data found" };
            return;
        }

        GuidanceData data = JsonUtility.FromJson<GuidanceData>(jsonFile.text);

        if (data == null || data.steps == null || data.steps.Length == 0)
        {
            Debug.LogError("[GuidanceManager] Failed to parse guidance JSON.");
            steps = new string[] { "No guidance data found." };
            return;
        }

        steps = data.steps;
    }

    private void HandleStepAdvanced(InteractionManager.ExperimentStep step)
    {
        currentStep++;

        if (currentStep >= steps.Length)
        {
            if (guidanceText != null)
                guidanceText.gameObject.SetActive(false);
            return;
        }

        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        if (!guidanceText)
        {
            Debug.LogWarning("[GuidanceManager] guidanceText not assigned.", this);
            return;
        }

        guidanceText.text = steps[currentStep];
    }
}
