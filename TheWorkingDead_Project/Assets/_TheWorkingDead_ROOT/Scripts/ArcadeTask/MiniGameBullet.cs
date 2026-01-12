using UnityEngine;

public class MiniGameBullet : MonoBehaviour
{

    public float speed = 500f;
    private RectTransform rectTransform;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
            Debug.LogError(":3");
    }

 
    void Update()
    {
        if(rectTransform == null) return;

        rectTransform.anchoredPosition += new Vector2(0, speed * Time.unscaledDeltaTime);

        CheckCollisionWithEnemies();

        if (rectTransform.anchoredPosition.y > 1000f) Destroy(gameObject);
    }

    void CheckCollisionWithEnemies()
    {
        MiniGameEnemy[] enemies = FindObjectsOfType<MiniGameEnemy>();
        foreach (var enemy in enemies)
        {
            RectTransform enemyRect = enemy.GetComponent<RectTransform>();
            if (enemyRect == null) continue;

            Vector2 bulletScreenPos = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);

            if (RectTransformUtility.RectangleContainsScreenPoint(enemyRect, bulletScreenPos))
            {
                if (Arcade_Minigame.instance != null)
                    Arcade_Minigame.instance.EnemiesKilled();

                Destroy(enemy.gameObject);
                Destroy(gameObject);
                return;
            }
        }
        
    }
}


