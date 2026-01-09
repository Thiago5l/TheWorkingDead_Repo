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
    
    [SerializeField] Image gregHeadImage;
    [SerializeField] Sprite[] gregSprites;

    [Header("For Sprite Movement")]
    [SerializeField] Transform gregHeadTransform;
    [SerializeField] Vector2 endPosition = new Vector2(0, 0);
    [SerializeField] Vector2 startPosition;
    [SerializeField] float desiredDuration = 3f;
    [SerializeField] float elapsedTime;

    //[Header("Others")]
    //[SerializeField] Transform PlayerTransform;
    //[SerializeField] Transform RespawnPoint;


    void Start()
    {
        Zombiedad = maxZombiedad;

        zombiedadBar.maxValue = maxZombiedad;
        zombiedadBar.value = Zombiedad;

        //

        startPosition = gregHeadTransform.localPosition;
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
        //-----//
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;

        gregHeadTransform.localPosition = Vector2.Lerp(startPosition, endPosition, percentageComplete);
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
