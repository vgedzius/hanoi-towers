using TMPro;
using UnityEngine;

namespace HanoiTowers
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI movesText;
        [SerializeField] Game game;

        void Update()
        {
            movesText.text = game.Moves.ToString();
        }
    }
}
