using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;

    [SerializeField]
    private TextMeshProUGUI m_CoinsText;

    private void OnEnable()
    {
        m_Player.OnCoinCollected += OnCoinCollected;
    }

    private void OnDisable()
    {
        m_Player.OnCoinCollected -= OnCoinCollected;
    }

    private void OnCoinCollected(int coins)
    {
        m_CoinsText.text = coins.ToString();
    }
}
