using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    public float energeticas;
    public int snacks;
    public float coins;

    public void CopyFromPlayer(PlayerController player)
    {
        energeticas = player.energeticas;
        snacks = player.snacks;
        coins = player.coins;
    }

    public void ApplyToPlayer(PlayerController player)
    {
        player.energeticas = energeticas;
        player.snacks = snacks;
        player.coins = coins;
    }
}
