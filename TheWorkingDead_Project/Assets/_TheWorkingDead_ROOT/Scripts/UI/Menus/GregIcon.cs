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

    private void Update()
    {
        if (zombieValue.Zombiedad == 0f && zombieValue.Zombiedad <= 0.200f)
        {
            actualGregSprite.sprite = gregSprites[0];
            Debug.Log("Good");
        }
        if (zombieValue.Zombiedad == 0.200f && zombieValue.Zombiedad <= 0.800f)
        {
            actualGregSprite.sprite = gregSprites[1];
            Debug.Log("medium");
        }
        if (zombieValue.Zombiedad == 0.800f && zombieValue.Zombiedad <= 100f)
        {
            actualGregSprite.sprite = gregSprites[2];
            Debug.Log("Bad"); 
        }
    }
}
