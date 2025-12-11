using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class ZombiedadScript : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float zombiedad;
    [SerializeField] float maxZombiedad = 100;

    [Header("Game Objects")]
    [SerializeField] Slider zombiedadBar;
    [SerializeField] GameObject looseCanvas;

    [Header("Sprites")]
    [SerializeField] Image gregHead;
    [SerializeField] Sprite[] gregSprites;

    //[Header("Others")]
    //[SerializeField] Transform PlayerTransform;
    //[SerializeField] Transform RespawnPoint;


    void Start()
    {
        zombiedad = maxZombiedad;

        zombiedadBar.maxValue = maxZombiedad;
        zombiedadBar.value = zombiedad;
    }
    //-----//
    private void Update()
    {
        zombiedad -= 0.01f;
        zombiedadBar.value = zombiedad;
        if (zombiedad == 0)
        {
            looseCanvas.SetActive(true);
        }
        //-----//
        if (zombiedad >= 100f && zombiedad <= 60f)
        {
            gregHead.sprite = gregSprites[0];
            Debug.Log("Good");
        }
        if (zombiedad >= 60f && zombiedad <= 25f)
        {
            gregHead.sprite = gregSprites[1];
            Debug.Log("medium");
        }
        if (zombiedad >= 25f && zombiedad <= 0f)
        {
            gregHead.sprite = gregSprites[2];
            Debug.Log("Bad");
        }
    }
}
