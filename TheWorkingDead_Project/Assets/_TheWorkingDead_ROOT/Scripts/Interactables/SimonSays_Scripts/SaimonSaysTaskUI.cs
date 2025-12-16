using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaimonSaysTaskUI : MonoBehaviour
{
    
    
    [SerializeField] public GameObject uiTarea;
    [SerializeField] public GameObject player;
    [SerializeField] bool tareaAcabada;
    [SerializeField] bool playerCerca;


    [SerializeField] public List<Button> buttonsList = new List<Button>();
  
    [SerializeField] public float showTime;
    [SerializeField] public float pauseTime;
    [SerializeField] bool ifRobot;
    [SerializeField] bool ifPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiTarea.SetActive(false);
        playerCerca = false;
        tareaAcabada = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = false;
        }
    }



    public void Interact()
    {
        if (playerCerca && !tareaAcabada)
        {
            uiTarea.SetActive(true);
            StartCoroutine(WaitToStart());
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSecondsRealtime(1);

        for (int i = 0; i < buttonsList.Count; i++)
        {
            //buttonsList[i].OnPointerClick += ButtonClicked;
        }

    }


    void ButtonClicked()
    {

    }

}
