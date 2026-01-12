using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]TMP_Text NumberofCoins;
    [SerializeField]PlayerController playerController;
    private void Awake()
    {
        Updatecoins();
    }
    public void Updatecoins()
    {
        NumberofCoins.text = playerController.coins.ToString();
    }
}
