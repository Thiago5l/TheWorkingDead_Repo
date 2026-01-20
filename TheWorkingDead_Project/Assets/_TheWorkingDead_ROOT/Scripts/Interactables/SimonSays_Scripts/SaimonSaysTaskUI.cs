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


public class SaimonSaysTaskUI : TaskBase
{
    [SerializeField] public List<SimonButton> buttonsList = new List<SimonButton>();
    [SerializeField] public List<int> intOrdenBotonesPlayer = new List<int>();
    //[SerializeField] public Color buttonColorSaved;

    [SerializeField] float showTime = 0.5f;
    [SerializeField] float pauseTime = 0.3f;
    
    [SerializeField] List<int> sequence = new List<int>();
    [SerializeField] int tamañoSequence;
    int playerIndex;
    bool playerTurn;

    [SerializeField] int rondasCompletadas =0;
    [SerializeField] public int rondasACompletar;

    public GameObject playerBloqueado;


    protected override void IniciarTarea()
    { InicioDeJuego(); }
    protected override void CancelarTarea()
    {
        CancelarBase();

        sequence.Clear();
        intOrdenBotonesPlayer.Clear();
        playerTurn = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (rondasCompletadas == rondasACompletar)
        {
            StopAllCoroutines();
            uiTarea.SetActive(false);
            tareaAcabada = true;
            CancelarBase();

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
                Loose();

            }
            intOrdenBotonesPlayer.Clear();
        }
    }


    void InicioDeJuego()
    {
        sequence.Clear();

        for (int i = 0; i < tamañoSequence; i++)
        {
            int randomBtton = Random.Range(0, buttonsList.Count);
            sequence.Add(randomBtton);
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
        playerBloqueado.gameObject.SetActive(true);
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

        playerBloqueado.gameObject.SetActive(false);
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
            Win();
        }
    }


    void HighlightButton(int index)
    {
        buttonsList[index].button.image.sprite = buttonsList[index].highlightColor;
    }

    void ResetButton(int index)
    {
        buttonsList[index].button.image.sprite = buttonsList[index].normalColor;
    }

}
