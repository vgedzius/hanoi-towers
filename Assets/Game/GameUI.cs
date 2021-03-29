using TMPro;
using UnityEngine;

namespace HanoiTowers
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI movesText;
        [SerializeField] Game game;
        [SerializeField] GameObject victoryPanel;
        [SerializeField] GameObject losePanel;

        void Update()
        {
            movesText.text = game.Moves.ToString();
        }

        public void ShowVictoryPanel()
        {
            victoryPanel.SetActive(true);
        }
        
        public void HideVictoryPanel()
        {
            victoryPanel.SetActive(false);
        }

        public void ShowLosePanel()
        {
            losePanel.SetActive(true);
        }
        
        public void HideLosePanel()
        {
            losePanel.SetActive(false);
        }
    }
}
