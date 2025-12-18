using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using DG.Tweening;

public class GregIcon : MonoBehaviour
{
    [SerializeField] OviedadZombie zombieValue;

    [Header("Images")]
    [SerializeField] RectTransform actualGregSprite;
    [SerializeField] Sprite[] gregSprites;
    //[SerializeField] Transform gregImage;

    private void Update()
    {
        if (zombieValue.Zombiedad >= 100f && zombieValue.Zombiedad <= 70f)
        {
            actualGregSprite.GetComponent<Image>().sprite = gregSprites[0];
            Debug.Log("Good");
        }
        if (zombieValue.Zombiedad >= 70f && zombieValue.Zombiedad <= 30f)
        {
            actualGregSprite.GetComponent<Image>().sprite = gregSprites[1];
            Debug.Log("medium");
        }
        if (zombieValue.Zombiedad >= 30f && zombieValue.Zombiedad <= 0f)
        {
            actualGregSprite.GetComponent<Image>().sprite = gregSprites[2];
            Debug.Log("Bad");
        }

    }
}
