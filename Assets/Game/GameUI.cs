using TMPro;
using UnityEngine;

namespace HanoiTowers
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI movesText;
        [SerializeField] Game game;
        [SerializeField] GameObject victoryPanel;
        [SerializeField] TextMeshProUGUI vicotrySubtext;

        void Update()
        {
            movesText.text = game.Moves.ToString();
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
    }
}