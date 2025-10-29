using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class Impresora : MonoBehaviour
{


    #region General Variables

    [SerializeField] float ValueBarStart = 25;
    [SerializeField] float SumValue;
    [SerializeField] Slider TaskBar;
    [SerializeField] GameObject Player;
    [SerializeField] float restValue;
    [SerializeField] float time;
    [SerializeField] Material Mat;
    [SerializeField] Material OutLine;
    [SerializeField] bool PlayerCerca;
    [SerializeField] bool TareaActiva;
    [SerializeField] bool TareaAcabada;
    private float save;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TareaActiva = false;
        PlayerCerca = false;
        save = ValueBarStart;
        TareaAcabada = false;
    }
   

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && TareaAcabada == false)
        {
            PlayerCerca = true;
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = OutLine;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && TareaAcabada == false)
        {
            PlayerCerca = false;
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;

        }

    }

    
    private void FixedUpdate()
    {
        if (PlayerCerca == true && Input.GetKeyDown(KeyCode.E))
        {

            TareaActiva = true;
            TaskBar.gameObject.SetActive(true);
            StartCoroutine(WaitTaskBar(time));
            
        }

        if(TareaActiva == true)
        {
            TaskCode();
        }
     

        TaskBar.value = ValueBarStart;

    }


    void TaskCode()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ValueBarStart = ValueBarStart + SumValue;
        }
        if (ValueBarStart >= 100)
        {

            Player.GetComponent<OviedadZombie>().Zombiedad -= 20;
            ValueBarStart = save;
            TaskBar.gameObject.SetActive(false);
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;
            TareaAcabada = true;
            StopAllCoroutines();    
            TareaActiva = false;
            this.gameObject.GetComponent<Impresora>().enabled = false;


        }
        if (ValueBarStart <= 0)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad += 5;
            ValueBarStart = save;
            TareaActiva = false;
            StopAllCoroutines();

        }
    }


    IEnumerator WaitTaskBar(float duration)
    {
        while (ValueBarStart < 100 && ValueBarStart>0)
        {
            yield return new WaitForSeconds(duration);
            ValueBarStart = ValueBarStart - restValue;
        }

    }
       
}
   

