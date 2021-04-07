using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HanoiTowers
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI movesText;
        [SerializeField] Game game;
        [SerializeField] GameObject victoryPanel;
        [SerializeField] GameObject menu;
        [SerializeField] TextMeshProUGUI vicotrySubtext;

        void Update()
        {
            movesText.text = game.Moves.ToString();
            
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }

        

        public void ShowVictoryPanel(bool champion, int moves)
        {
            victoryPanel.SetActive(true);

            vicotrySubtext.text = champion
                ? "You are an absolute champion, now go for a walk"
                : $"It took you {moves.ToString()} moves, but can you do better?";
        }

        public void HideVictoryPanel()
        {
            victoryPanel.SetActive(false);
        }

        void ShowMenu()
        {
            menu.SetActive(true);
        }

        public void HideMenu()
        {
            menu.SetActive(false);
        }
        
        void ToggleMenu()
        {
            if (menu.activeSelf)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }

        public void ExitToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}