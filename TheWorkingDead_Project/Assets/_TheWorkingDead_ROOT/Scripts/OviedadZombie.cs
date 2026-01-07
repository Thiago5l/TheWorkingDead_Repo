using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class OviedadZombie : MonoBehaviour
{



    [Header("Values")]
    [SerializeField] public float Zombiedad;
    [SerializeField] public float maxZombiedad = 100;
    [SerializeField] public float ZombiedadSpeed = 100;

    [Header("Game Objects")]
    [SerializeField] Slider zombiedadBar;
    [SerializeField] GameObject looseCanvas;

    [Header("Sprites")]
    [SerializeField] Transform gregHeadTransform;
    [SerializeField] Image gregHeadImage;
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
        if (Zombiedad > 100f)
        {
            Zombiedad = 100f;
        }

        if (Zombiedad < 0f)
        {
            Zombiedad = 0f;
        }

        Zombiedad -= ZombiedadSpeed * Time.deltaTime; 
        zombiedadBar.value = Zombiedad;

        if (Zombiedad <= 0f)
        {
            looseCanvas.SetActive(true);
            this.gameObject.GetComponent<PlayerController>().playerOcupado = true;
        }
        //-----//
        if (Zombiedad <= 100.000f && Zombiedad >= 70.000f)
        {
            gregHeadImage.sprite = gregSprites[0];
            Debug.Log("Good");

            gregHeadTransform.DOMoveX(3f, 2f).SetLoops(5, LoopType.Yoyo);
        }
        if (Zombiedad <= 70.000f && Zombiedad >= 30.000f)
        {
            gregHeadImage.sprite = gregSprites[1];
            Debug.Log("medium");
        }
        if (Zombiedad <= 30f && Zombiedad >= 0f)
        {
            gregHeadImage.sprite = gregSprites[2];
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
