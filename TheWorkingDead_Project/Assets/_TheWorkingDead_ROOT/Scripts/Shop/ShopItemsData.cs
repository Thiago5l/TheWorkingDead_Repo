using UnityEngine;
[CreateAssetMenu(fileName ="New Item Data", menuName = "Item Data")]
public class ShopItemsData : ScriptableObject
{
    [SerializeField] private string itemname;
    [SerializeField] private string description;
    [SerializeField] private int cost;

    public string Itemname { get { return itemname; } }
    public string Description { get { return description; } }
    public int Cost { get { return cost; } }
}
