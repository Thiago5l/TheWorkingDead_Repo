using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class ZombiedadScript : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] public int Zombiedad = 100;
    [SerializeField] public int sumValue;
    [SerializeField] int time;

    [Header("Game Objects")]
    [SerializeField] Slider ZombiedadBar;
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform RespawnPoint;
    [SerializeField] public GameObject LooseCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ZombiedadBar == null)
        {
            ZombiedadBar.value -= sumValue;
        }
        int zValue = 1;
        StartCoroutine(WaitObviedad(zValue));
    }
    IEnumerator WaitObviedad(int duration)
    {
        while (Zombiedad < 1)
        {
            yield return new WaitForSeconds(time);
            Zombiedad = Zombiedad - sumValue;        
        }

    }
    //-----//
    private void Update()
    {

         if (ZombiedadBar.value < 1)
         {
            Zombiedad = 1;
         }

         if (ZombiedadBar.value >= 0)
         {
            LooseCanvas.SetActive(true);
         }
    }

 }
