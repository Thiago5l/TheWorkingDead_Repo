using UnityEngine;

public class coin : MonoBehaviour
{
    [SerializeField] private CoinManager CoinManager;
    [SerializeField] private PlayerController playerController;
    [Header("Spawn Points")]
    public Transform[] spawnPoints;
    [Header("flags")]
    [SerializeField] private bool playercerca = false;
    public void givecoin()
    {
        if (!playercerca) return;

        playerController.coins++;
        CoinManager.Updatecoins();
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playercerca = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playercerca = false;
        }
    }
}
