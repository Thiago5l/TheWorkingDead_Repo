using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class Impresora : MonoBehaviour
{


    #region General Variables
    [SerializeField] float ValueBarStart = 25;
    [SerializeField] Slider TaskBar;
    [SerializeField] GameObject Player;
    [SerializeField] float restValue;
    [SerializeField] float time;
    [SerializeField] Material Mat;
    private float save;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        save = ValueBarStart;
        StartCoroutine(WaitTaskBar(time));
    }
    IEnumerator WaitTaskBar(float duration)
    {
        while (ValueBarStart < 100)
        {
            yield return new WaitForSeconds(time);
            ValueBarStart = ValueBarStart - restValue;
        }

    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (Input.GetKeyDown("space"))
        {
            ValueBarStart += 3f;
        }
        if (ValueBarStart >= 100)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad -= 20;
            ValueBarStart = save;
            TaskBar.gameObject.SetActive(false);
            this.gameObject.GetComponent<Impresora>().enabled = false;
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;

        }
        if (ValueBarStart <= 0)
        {
            //Player.GetComponent<OviedadZombie>().Zombiedad += 15;

            float xval = Player.GetComponent<OviedadZombie>().sumValue;
            Player.GetComponent<OviedadZombie>().sumValue += 15;
            StartCoroutine(WaitCambio(xval));

        }
        TaskBar.value = ValueBarStart;

        IEnumerator WaitCambio (float i)
        {
            yield return new WaitForSeconds(1);
            Player.GetComponent<OviedadZombie>().sumValue = i;
            
        }

    }
}
   

