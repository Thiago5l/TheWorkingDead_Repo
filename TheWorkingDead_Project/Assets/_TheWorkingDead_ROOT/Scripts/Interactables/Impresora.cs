using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;
public class Impresora : MonoBehaviour
{


    #region General Variables
    [SerializeField] float Value = 25;
    [SerializeField] Slider TaskBar;
    [SerializeField] GameObject Player;
    [SerializeField] float restValue;
    [SerializeField] float time;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(WaitObviedad(time));
    }
    IEnumerator WaitObviedad(float duration)
    {
        while (Value < 100)
        {
            yield return new WaitForSeconds(time);
            Value = Value - restValue;
        }

    }




    // Update is called once per frame

    private void FixedUpdate()
    {
        if (Input.GetKeyDown("space"))
        {
            Value += 1;
        }
        if (Value >= 100)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad -= 20;
            this.gameObject.SetActive(false);

        }
        if (Value <= 0)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad += 15;

        }
        TaskBar.value = Value;
    }
}
   

