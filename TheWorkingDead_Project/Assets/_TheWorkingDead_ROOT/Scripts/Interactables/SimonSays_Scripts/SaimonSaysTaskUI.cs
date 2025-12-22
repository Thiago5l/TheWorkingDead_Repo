using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SimonButton
{
    public Button button;
    public Sprite normalColor;
    public Sprite highlightColor;

}


public class SaimonSaysTaskUI : MonoBehaviour
{
    [SerializeField] public GameObject uiTarea;
    [SerializeField] public GameObject player;

    [SerializeField] bool tareaAcabada;
    [SerializeField] bool playerCerca;

    [SerializeField] public List<SimonButton> buttonsList = new List<SimonButton>();
    [SerializeField] public List<int> intOrdenBotonesPlayer = new List<int>();
    //[SerializeField] public Color buttonColorSaved;

    [SerializeField] float showTime = 0.5f;
    [SerializeField] float pauseTime = 0.3f;
    
    [SerializeField] List<int> sequence = new List<int>();
    [SerializeField] List<SimonButton> secuenciaDeBotones = new List<SimonButton>();
    [SerializeField] int tamañoSequence;
    int playerIndex;
    bool playerTurn;

    [SerializeField] int rondasCompletadas;
    [SerializeField] public int rondasACompletar;


    [SerializeField] private FadeCanvas taskFeedbackCanvas;

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
        if (other.CompareTag("TaskPlayer"))
        {
            playerCerca = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playerCerca = false;
        }
    }



    public void Interact()
    {
        
        if (playerCerca && !tareaAcabada)
        {
            player.GetComponent<PlayerController>().playerOcupado = true;
            uiTarea.SetActive(true);
            InicioDeJuego();
            //StartCoroutine(WaitToStart());
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (rondasCompletadas == rondasACompletar)
        {
            StopAllCoroutines();
            uiTarea.SetActive(false);
            tareaAcabada = true;

            Debug.Log("Tarea Completada!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!11");
        }



        if (intOrdenBotonesPlayer.Count == sequence.Count && playerTurn)
        {
            bool correcto = true;
            for (int i = 0; i < sequence.Count; i++)
            {
                if (intOrdenBotonesPlayer[i] != sequence[i])
                {
                    correcto = false;
                    break;
                }
            }
            if (correcto)
            {
                SiguienteRonda();
            }
            else
            {
                //Game Over
                uiTarea.SetActive(false);
                player.gameObject.GetComponent<PlayerController>().playerOcupado = false;
                tareaAcabada = false;
                taskFeedbackCanvas.PlayLose();
                Debug.Log("Game Over");

            }
            intOrdenBotonesPlayer.Clear();
        }
        

        //if(intOrdenBotonesPlayer .Count == sequence.Count && playerTurn)
        //{
        //    bool correcto = true;
        //    for(int i = 0; i < sequence.Count; i++)
        //    {
        //        if(intOrdenBotonesPlayer[i] != sequence[i])
        //        {
        //            correcto = false;
        //            break;
        //        }
        //    }
        //    if(correcto)
        //    {
        //        SiguienteRonda();
        //    }
        //    else
        //    {
        //        //Game Over
        //        tareaAcabada = true;
        //        uiTarea.SetActive(false);
        //    }
        //    intOrdenBotonesPlayer.Clear();
        //}

    }


    void InicioDeJuego()
    {
        sequence.Clear();
        secuenciaDeBotones.Clear();
        for (int i = 0; i <tamañoSequence; i++)
        {

            int randomBtton = Random.Range(0, buttonsList.Count);
            sequence.Add(randomBtton);
        }
        for (int i = 0; i < sequence.Count; i++)
        {
            secuenciaDeBotones.Add(buttonsList[sequence[i]]);
        }

        MostrarSecuencia();

    }

    public void MostrarSecuencia()
    {
        StartCoroutine(ShowSequence());
    }
    IEnumerator ShowSequence()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < sequence.Count; i++)
        {
            int index = sequence[i];
            Debug.Log("Indice de la secuencia: " + index);
            HighlightButton(index);
            yield return new WaitForSeconds(showTime);
            ResetButton(index);
            yield return new WaitForSeconds(pauseTime);
        }

        //foreach (int index in sequence)
        //{
        //    HighlightButton(/*buttonsList[index]*/index);
        //    yield return new WaitForSeconds(showTime);
        //    ResetButton(/*buttonsList[index]*/index);
        //    yield return new WaitForSeconds(pauseTime);
        //}

        playerTurn = true;
    }

    public void PresionarBotonPlayer(int index)
    {
        if (!playerTurn) return;
        else
        {
            intOrdenBotonesPlayer.Add(index);
        }
    }

    void SiguienteRonda()
    {
        if(rondasACompletar > rondasCompletadas)
        {
            rondasCompletadas += 1;
            tamañoSequence += 1;
            InicioDeJuego();
        }
        if (rondasCompletadas == rondasACompletar)
        {
            StopAllCoroutines();
            uiTarea.SetActive(false);
            tareaAcabada = true;
            taskFeedbackCanvas.PlayWin();
            Debug.Log("Tarea Completada!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!11");
        }
    }


    void HighlightButton(int index)
    {
        secuenciaDeBotones[index].button.image.sprite = secuenciaDeBotones[index].highlightColor;
    }

    void ResetButton(int index)
    {
        secuenciaDeBotones[index].button.image.sprite = secuenciaDeBotones[index].normalColor;
    }











    //void Start()
    //{

    //    uiTarea.SetActive(false);
    //    playerCerca = false;
    //    tareaAcabada = false;

    //    rondasCompletadas = 0;

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("TaskPlayer"))
    //    {
    //        playerCerca = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("TaskPlayer"))
    //    {
    //        playerCerca = false;
    //    }
    //}



    //public void Interact()
    //{

    //    if (playerCerca && !tareaAcabada)
    //    {
    //        uiTarea.SetActive(true);
    //        //StartCoroutine(WaitToStart());
    //    }

    //}


    //// Update is called once per frame
    //void Update()
    //{
    //    if (rondasCompletadas == rondasACompletar)
    //    {
    //        StopAllCoroutines();
    //        uiTarea.SetActive(false);
    //    }
    //}

    //IEnumerator WaitToStart()
    //{
    //    yield return new WaitForSecondsRealtime(1);


    //    StartGame();

    //    yield return new WaitForSecondsRealtime(1);
    //    for (int i = 0; i < buttonsList.Count; i++)
    //    {
    //        int index = i;
    //        buttonsList[i].button.onClick.AddListener(() => ButtonClicked(index));
    //        buttonsList[i].button.image.color = buttonsList[i].normalColor;
    //        StartGame();
    //    }

    //}

    //void StartGame()
    //{
    //    sequence.Clear();
    //    AddToSequence();
    //}
    //void AddToSequence()
    //{
    //    playerTurn = false;
    //    playerIndex = 0;

    //    int randomButton = Random.Range(0, (buttonsList.Count-1));
    //    sequence.Add(randomButton);

    //    StartCoroutine(ShowSequence());
    //}

    //IEnumerator ShowSequence()
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    for (int i = 0; i < sequence.Count; i++)
    //    {
    //        HighlightButton(/*buttonsList[index]*/sequence[i]);
    //        yield return new WaitForSeconds(showTime);
    //        ResetButton(sequence[i]);
    //        yield return new WaitForSeconds(pauseTime);
    //    }

    //    foreach (int index in sequence)
    //    {
    //        HighlightButton(/*buttonsList[index]*/index);
    //        yield return new WaitForSeconds(showTime);
    //        ResetButton(/*buttonsList[index]*/index);
    //        yield return new WaitForSeconds(pauseTime);
    //    }

    //    playerTurn = true;
    //}
    //public void ButtonClicked(int index)
    //{

    //    if (!playerTurn) Debug.Log("No es tu turno"); return;

    //    if (index == sequence[playerIndex])
    //    {
    //        Debug.Log("has llegado al 1º if buttoncliked");

    //        if (playerIndex >= sequence.Count)
    //        {
    //            Debug.Log("has llegado al 2ºº if buttoncliked");
    //            StartCoroutine(NextRound());
    //        }
    //    }
    //    else
    //    {
    //        GameOver();
    //    }


    //}
    //IEnumerator NextRound()
    //{
    //    playerTurn = false;
    //    rondasCompletadas += 1 ;
    //    yield return new WaitForSeconds(1f);
    //    AddToSequence();
    //}

    //void GameOver()
    //{
    //    tareaAcabada = true;
    //    playerTurn = false;
    //}
    //void HighlightButton(int index)
    //{
    //    buttonsList[index].button.image.color = buttonsList[index].highlightColor;
    //}

    //void ResetButton(int index)
    //{
    //    buttonsList[index].button.image.color = buttonsList[index].normalColor;
    //}

}
