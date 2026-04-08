using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject gameCompletePanel;

    [Header("Settings")]
    [SerializeField] private float gameOverDelay = 4.0f;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        if (gameCompletePanel) gameCompletePanel.SetActive(false);
        else Debug.LogWarning("[GameManager] gameCompletePanel not assigned");
    }

    private void OnEnable()
    {
        InteractionManager.OnExperimentComplete += HandleExperimentComplete;
    }

    private void OnDisable()
    {
        InteractionManager.OnExperimentComplete -= HandleExperimentComplete;
    }

    private void HandleExperimentComplete()
    {
        StartCoroutine(ShowCompletionPanel());
    }

    private IEnumerator ShowCompletionPanel()
    {
        yield return new WaitForSeconds(gameOverDelay);

        if (gameCompletePanel) gameCompletePanel.SetActive(true);
    }
    
    //called by btn
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
