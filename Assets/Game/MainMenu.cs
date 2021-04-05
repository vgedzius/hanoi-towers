using UnityEngine;
using UnityEngine.SceneManagement;

namespace HanoiTowers
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex + 1
            );
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}