using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPoint : MonoBehaviour
{
    private const int experimentLabSceneIndex = 1;
    private const int quizGameSceneIndex = 2;
   
    public void LoadExperimentLabAssignment()
    {
        SceneManager.LoadScene(experimentLabSceneIndex);
    }

    public void LoadQuizGameAssignment()
    {
        SceneManager.LoadScene(quizGameSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
