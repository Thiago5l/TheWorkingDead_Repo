using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class OviedadZombie : MonoBehaviour
{



    [Header("Values")]
    [SerializeField] public float Zombiedad;
    [SerializeField] public float maxZombiedad = 100;

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
        Zombiedad = maxZombiedad;

        zombiedadBar.maxValue = maxZombiedad;
        zombiedadBar.value = Zombiedad;
    }
    //-----//
    private void Update()
    {
        Zombiedad -= 0.01f;
        zombiedadBar.value = Zombiedad;
        if (Zombiedad == 0)
        {
            looseCanvas.SetActive(true);
        }
        //-----//
        if (Zombiedad >= 100f && Zombiedad <= 60.000f)
        {
            gregHead.sprite = gregSprites[0];
            Debug.Log("Good");
        }
        if (Zombiedad >= 60.000f && Zombiedad <= 25.000f)
        {
            gregHead.sprite = gregSprites[1];
            Debug.Log("medium");
        }
        if (Zombiedad >= 25.000f && Zombiedad <= 0.000f)
        {
            gregHead.sprite = gregSprites[2];
            Debug.Log("Bad");
        }
    }

    //    #region General Variables
    //    public float Zombiedad = 0;
    //    [SerializeField] Slider ZombiedadBar;
    //    [SerializeField] Transform PlayerTransform;
    //    [SerializeField] Transform RespawnPoint;
    //    [SerializeField] public float sumValue;
    //    [SerializeField] float time;
    //    [SerializeField] public GameObject LooseCanvas;
    //    #endregion

    //    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //    void Start()
    //    {
    //        if (ZombiedadBar == null)
    //        {
    //            // Busca el objeto "Fill" en la jerarquía
    //            ZombiedadBar = GameObject.Find("Fill").GetComponent<Slider>();
    //        }
    //        float zValue = 1;
    //        StartCoroutine(WaitObviedad(zValue));
    //    }
    //   IEnumerator WaitObviedad(float duration)
    //    {
    //         while (Zombiedad< 1)
    //         {
    //            yield return new WaitForSeconds(time);
    //            Zombiedad = Zombiedad + sumValue;          
    //         }

    //    } 




    //// Update is called once per frame

    //private void Update()
    //{
    //        ZombiedadBar.GetComponent<Image>().fillAmount = Zombiedad;

    //        if (Zombiedad < 0)
    //        {
    //            Zombiedad = 0;
    //        }

    //        if (Zombiedad >= 1)
    //        {
    //            LooseCanvas.SetActive(true);
    //        }
    //}
}
