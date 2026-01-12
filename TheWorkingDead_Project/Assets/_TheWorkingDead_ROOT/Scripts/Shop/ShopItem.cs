using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("itemdata")]
    [SerializeField] ShopItemsData ShopItemsData;
    [SerializeField] TMP_Text ItemName;
    [SerializeField] TMP_Text ItemDescription;
    [SerializeField] TMP_Text ItemPrice;
    [Header("references")]
    [SerializeField] PlayerController PlayerController;
    [SerializeField] EnergeticasUI EnergeticasUI;
    [SerializeField] Snacks_UI Snacks_UI;
    [SerializeField] CoinManager CoinManager;
    private void Awake()
    {
        updateui();
    }
    void updateui()
    {
        ItemName.text = ShopItemsData.Itemname;
        ItemDescription.text = ShopItemsData.Description;
        ItemPrice.text = ShopItemsData.Cost.ToString();
    }
    public void comprarenergetica()
    {
      if (PlayerController.coins>=ShopItemsData.Cost)
        { PlayerController.energeticas= PlayerController.energeticas+1;
            PlayerController.coins -= ShopItemsData.Cost;
            EnergeticasUI.SetEnergeticas((int)PlayerController.energeticas);
            CoinManager.Updatecoins();
        }
    }
    public void comprarsnack()
    {
        if (PlayerController.coins>=ShopItemsData.Cost)
        {
            PlayerController.snacks= PlayerController.snacks+1;
            PlayerController.coins -= ShopItemsData.Cost;
            Snacks_UI.SetSnacks((int)PlayerController.snacks);
            CoinManager.Updatecoins();
        }
    }
}
