using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assignment2
{
    public class GameManager : MonoBehaviour
    {
        private const int HomeSceneIndex = 0;

        //called by btn
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //called by btn
        public void GoToHome()
        {
            SceneManager.LoadScene(HomeSceneIndex);
        }
    }
}
