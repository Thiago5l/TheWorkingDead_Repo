using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;

public class OviedadZombie : MonoBehaviour
{

    #region General Variables
    public float Zombiedad = 0;
    [SerializeField] Slider ZombiedadBar;
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform RespawnPoint;
    [SerializeField] float sumValue;
    [SerializeField] float time;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float zValue = 100;
        StartCoroutine(WaitObviedad(zValue));
    }
   IEnumerator WaitObviedad(float duration)
    {
         while (Zombiedad< 100)
         {
            yield return new WaitForSeconds(time);
            Zombiedad = Zombiedad + sumValue;          
         }

    } 
             



// Update is called once per frame

private void FixedUpdate()
{
        ZombiedadBar.value = Zombiedad;

        if (Zombiedad >= 100)
        {
            PlayerTransform.position = RespawnPoint.position;
            Zombiedad = 0f;
        }
}

  }
