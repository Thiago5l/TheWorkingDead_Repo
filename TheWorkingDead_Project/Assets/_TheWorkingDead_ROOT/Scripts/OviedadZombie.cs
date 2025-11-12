using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class OviedadZombie : MonoBehaviour
{

    #region General Variables
    public float Zombiedad = 0;
    [SerializeField] Image ZombiedadBar;
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform RespawnPoint;
    [SerializeField] public float sumValue;
    [SerializeField] float time;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ZombiedadBar == null)
        {
            // Busca el objeto "Fill" en la jerarquía
            ZombiedadBar = GameObject.Find("Fill").GetComponent<Image>();
        }
        float zValue = 1;
        StartCoroutine(WaitObviedad(zValue));
    }
   IEnumerator WaitObviedad(float duration)
    {
         while (Zombiedad< 1)
         {
            yield return new WaitForSeconds(time);
            Zombiedad = Zombiedad + sumValue;          
         }

    } 
             



// Update is called once per frame

private void Update()
{
        ZombiedadBar.GetComponent<Image>().fillAmount = Zombiedad;

        if (Zombiedad < 0)
        {
            Zombiedad = 0;
        }

        if (Zombiedad >= 1)
        {
            SceneManager.LoadScene("SCN_Game_Over");
        }
}

  }
