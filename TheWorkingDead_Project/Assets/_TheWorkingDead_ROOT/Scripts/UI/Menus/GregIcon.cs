using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GregIcon : MonoBehaviour
{
    [SerializeField] OviedadZombie zombieValue;

    [SerializeField] Image actualGregSprite;
    [SerializeField] Sprite[] gregSprites;
    [SerializeField] Transform gregImage;

    private void Update()
    {
        if (zombieValue.Zombiedad >= 0f && zombieValue.Zombiedad <= 0.400f)
        {
            actualGregSprite.sprite = gregSprites[0];
            Debug.Log("Good");
        }
        if (zombieValue.Zombiedad >= 0.400f && zombieValue.Zombiedad <= 0.850f)
        {
            actualGregSprite.sprite = gregSprites[1];
            StartCoroutine(SpriteShake());
            Debug.Log("medium");
        }
        if (zombieValue.Zombiedad >= 0.850f && zombieValue.Zombiedad <= 1.000f)
        {
            actualGregSprite.sprite = gregSprites[2];
            Debug.Log("Bad"); 
        }
    }

    IEnumerator SpriteShake()
    {
        gregImage.transform.localPosition = new Vector2(-59, 189);
        yield return new WaitForSecondsRealtime(0.2f);
        gregImage.transform.localPosition = new Vector2(-61, 193);
        yield return new WaitForSecondsRealtime(0.2f);
        gregImage.transform.localPosition = new Vector2(-60, 192);
    }
}
