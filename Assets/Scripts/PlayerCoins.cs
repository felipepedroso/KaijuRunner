using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCoins : MonoBehaviour {
    public int CurrentAmount;
    public Text CoinsValueText;

    void Start()
    {
        CurrentAmount = 0;
    }

    public void CoinCollected(int amountCollected)
    {
        CurrentAmount += amountCollected;
        CurrentAmount = Mathf.Clamp(CurrentAmount, 0, int.MaxValue);
    }

    void Update()
    {
        if (CoinsValueText != null)
        {
            CoinsValueText.text = string.Format("{0:00000000}", (int)(CurrentAmount));
        }
    }
}
