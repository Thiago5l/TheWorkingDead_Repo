using System;
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
    [SerializeField] public Color buttonColorSaved;

    [SerializeField] float showTime = 0.5f;
    [SerializeField] float pauseTime = 0.3f;

    List<int> sequence = new List<int>();
    int playerIndex;
    bool playerTurn;

    [SerializeField] int rondasCompletadas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiTarea.SetActive(false);
        playerCerca = false;
        tareaAcabada = false;
        rondasCompletadas = 0;

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
            int index = i;
            buttonsList[i].onClick.AddListener(() => ButtonClicked(index));
            StartGame();
        }

    }

    void StartGame()
    {
        sequence.Clear();
        AddToSequence();
    }
    void AddToSequence()
    {
        playerTurn = false;
        playerIndex = 0;

        //int randomButton = Random.Range(0, buttonsList.Count);
        //sequence.Add(randomButton);

        //StartCoroutine(ShowSequence());
    }

    void ButtonClicked(int index)
    {
        if (!playerTurn) return;

        if (index == sequence[playerIndex])
        {
            playerIndex++;

            if (playerIndex >= sequence.Count)
            {
                StartCoroutine(NextRound());
            }
        }
        else
        {
            GameOver();
        }

        
    }
    IEnumerator NextRound()
    {
        playerTurn = false;
        rondasCompletadas += 1 ;
        yield return new WaitForSeconds(1f);
        AddToSequence();
    }

    void GameOver()
    {
        tareaAcabada = true;
        playerTurn = false;
    }
    void HighlightButton(Button button)
    {
        buttonColorSaved = button.image.color;
        button.image.color = Color.white;
    }

    void ResetButton(Button button)
    {
        button.image.color = buttonColorSaved;
    }

}
