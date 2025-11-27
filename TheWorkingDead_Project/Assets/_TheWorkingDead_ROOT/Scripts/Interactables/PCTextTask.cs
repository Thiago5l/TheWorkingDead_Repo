using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit;
using System.Threading.Tasks;
//using Unity.VisualScripting;

public class PCTextTask : MonoBehaviour
{

//CÓDIGO COMPLETO PASADO POR IA


    #region Tareas aleatorias
    [SerializeField] public GameObject objectTareas;
    private TareasAleatorias tareasScript;
    #endregion

    #region Texto
    [SerializeField] public GameObject prefabTaskUI;
    [SerializeField] public List<string> textsToSelect = new List<string>();
    [SerializeField] public List<string> textsSelected = new List<string>();
    [SerializeField] public int tamañoTextsSelected;
    [SerializeField] string newText = string.Empty;
    [SerializeField] string textToWrite = string.Empty;
    [SerializeField] public GameObject textUI;
    [SerializeField] public TMP_InputField insertedText;
    #endregion

    #region TIEMPO
    [SerializeField] private Slider timeSlider;
    [SerializeField] private float tiempoMax;
    [SerializeField] private float tiempoActual;
    [SerializeField] private float tiempoPorCaracter = 0.5f;
    #endregion

    #region Interacción y estado
    [SerializeField] Material Mat;
    [SerializeField] Material OutLine;
    [SerializeField] bool PlayerCerca;
    [SerializeField] bool tareaActiva;
    [SerializeField] bool tareaAcabada;
    [SerializeField] bool tareaFallada;
    [SerializeField] bool tareaEnProceso;
    [SerializeField] private int palabarasBien;
    #endregion

    #region Penalización
    [SerializeField] GameObject Player;
    [SerializeField] float penalizacion = 5f; // Cantidad de penalización
    #endregion

    void Start()
    {
        tareasScript = objectTareas.GetComponent<TareasAleatorias>();

        palabarasBien = 0;

        textsSelected.Clear();

        tareaEnProceso = false;
        tareaFallada = false;
        tareaActiva = false;
        tareaAcabada = false;
        PlayerCerca = false;

        prefabTaskUI.SetActive(false);
        GeneradorListaTextos();
    }

    public void GeneradorListaTextos()
    {
        if (textsToSelect == null || textsToSelect.Count == 0)
        {
            Debug.LogError("textsToSelect está vacío!");
            return;
        }

        if (textsToSelect.Count < tamañoTextsSelected)
        {
            Debug.LogError("No hay suficientes palabras en textsToSelect");
            return;
        }

        textsSelected.Clear();
        MezclarLista(textsToSelect);

        for (int i = 0; i < tamañoTextsSelected; i++)
        {
            textsSelected.Add(textsToSelect[i]);
        }

        palabarasBien = 0;

        PasarSiguientePalabra();
        //textToWrite = textsSelected[0];
        //textUI.GetComponent<TextMeshProUGUI>().text = textToWrite;

        //ConfigurarTiempo();
    }



    //public void GeneradorListaTextos()
    //{
    //    if (textsToSelect == null || textsToSelect.Count == 0)
    //    {
    //        Debug.LogError("textsToSelect está vacío!");
    //        return;
    //    }

    //    MezclarLista(textsToSelect);
    //    textToWrite = textsSelected[0];
    //    textUI.GetComponent<TextMeshProUGUI>().text = textToWrite;
    //}

    public void PasarSiguientePalabra()
    {
        insertedText.text = string.Empty;
        newText = string.Empty;
        
        textToWrite = textsSelected[0];
        textUI.GetComponent<TextMeshProUGUI>().text = textToWrite;

        ConfigurarTiempo();
    }

    private void MezclarLista(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    // ===============================
    // DETECCIÓN DEL JUGADOR (MATERIALES)
    // ===============================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !tareaAcabada)
        {
            if (tareaActiva)
            {
                PlayerCerca = true;
                GetComponent<MeshRenderer>().material = OutLine;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !tareaAcabada)
        {
            PlayerCerca = false;
            GetComponent<MeshRenderer>().material = Mat;
        }
    }

    // ===============================
    // CONTROL DE ORDEN DE TAREAS
    // ===============================
    void Update()
    {
        if (tareasScript.OrdenTareas[0] == this.gameObject)
            tareaActiva = true;
        else
            tareaActiva = false;

        if (tareaAcabada)
        {
            for (int i = 0; i < tareasScript.OrdenTareas.Count; i++)
            {
                if (tareasScript.OrdenTareas[i] == this.gameObject)
                {
                    objectTareas.GetComponent<TareasAleatorias>().posTareaAcabada = i;
                    objectTareas.GetComponent<TareasAleatorias>().acabarTarea = true;
                    return;
                }
            }
        }

        if (tareaEnProceso)
        {
            
            tiempoActual -= Time.deltaTime;
            timeSlider.value = tiempoActual;

            if (tiempoActual <= 0f)
            {
                FallarTarea();
            }
        }
    }

    // ===============================
    // INTERACTUAR
    // ===============================
    public void Interactuar()
    {
        Player.GetComponent<PlayerController>().playerOcupado = true;
        if (PlayerCerca && tareaActiva && !tareaAcabada && !tareaEnProceso)
        {
            tareaEnProceso = true;
            prefabTaskUI.SetActive(true);
            insertedText.text = string.Empty;
            insertedText.ActivateInputField();
            tareaFallada = false;
        }
    }


    // ===============================
    // CONTROL DEL TIEMPO
    // ===============================

    void ConfigurarTiempo()
    {

        tiempoMax = textToWrite.Length * tiempoPorCaracter;
        tiempoActual = tiempoMax;

        timeSlider.maxValue = tiempoMax;
        timeSlider.value = tiempoMax;
    }

    // ===============================
    // VALIDACIÓN DEL TEXTO
    // ===============================
    void LateUpdate()//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        if (!tareaActiva || tareaAcabada) return;

        newText = insertedText.text;
        ActualizarTextoColoreado();

        if (string.IsNullOrEmpty(newText)) return;

        // Si se pasa de largo -> FALLO
        if (newText.Length > textToWrite.Length)
        {
            FallarTarea();
            return;
        }

        // Fallo por letra incorrecta
        for (int i = 0; i < newText.Length; i++)
        {
            if (char.ToLower(newText[i]) != char.ToLower(textToWrite[i]))
            {
                FallarTarea();
                return;
            }
        }

        // Palabra Completada correctamente
        if (newText.Length == textToWrite.Length)
        {
            palabarasBien++;
            textsSelected.RemoveAt(0);

            if (textsSelected.Count > 0)
            {
                PasarSiguientePalabra();
            }
        }

        if (palabarasBien == tamañoTextsSelected)
        {
            CompletarTarea();
        }
    }

    // ===============================
    // CUANDO SE FALLA
    // ===============================


    void FallarTarea()
    {
        if (!tareaEnProceso) return; // evita dobles llamadas

        tareaFallada = true;
        tareaActiva = false;
        tareaEnProceso = false;
        timeSlider.value = 0;

        Player.GetComponent<PlayerController>().playerOcupado = false;

        StartCoroutine(FalloConDelay());
    }

    IEnumerator FalloConDelay()
    {
        // Espera medio segundo
        yield return new WaitForSeconds(0.5f);

        prefabTaskUI.SetActive(false);
        insertedText.text = string.Empty;

        // Penalización
        if (Player != null)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad += (penalizacion / 100);
        }

        GetComponent<MeshRenderer>().material = Mat;

    }





    //void FallarTarea()
    //{
    //    tareaFallada = true;
    //    tareaActiva = false;
    //    tareaEnProceso = false;

    //    prefabTaskUI.SetActive(false);
    //    insertedText.text = string.Empty;

    //    // Penalización (igual que tu zombie)
    //    if (Player != null)
    //    {
    //        Player.GetComponent<OviedadZombie>().Zombiedad += (penalizacion / 100);
    //    }

    //    GetComponent<MeshRenderer>().material = Mat;

    //    Debug.Log("TAREA FALLADA - Penalización aplicada");
    //}

    // ===============================
    // CUANDO SE COMPLETA
    // ===============================
    void CompletarTarea()
    {
        tareaEnProceso = false;
        tareaFallada = false;
        tareaAcabada = true;
        tareaActiva = false;
        timeSlider.value = 0;

        Player.GetComponent<PlayerController>().playerOcupado = false;

        prefabTaskUI.SetActive(false);
        GetComponent<MeshRenderer>().material = Mat;

        objectTareas.GetComponent<TareasAleatorias>().ganaTarea = true;
        this.enabled = false;
    }

    // ===============================
    // TEXTO COLOREADO
    // ===============================
    void ActualizarTextoColoreado()
    {
        string resultado = string.Empty;

        for (int i = 0; i < textToWrite.Length; i++)
        {
            if (i < newText.Length)
            {
                if (char.ToLower(newText[i]) == char.ToLower(textToWrite[i]))
                    resultado += $"<color=green>{textToWrite[i]}</color>";
                else
                    resultado += $"<color=red>{textToWrite[i]}</color>";
            }
            else
            {
                resultado += $"<color=white>{textToWrite[i]}</color>";
            }
        }

        textUI.GetComponent<TextMeshProUGUI>().text = resultado;
    }







    //////////////////////////////////////////////////////////////////////////




}



//PRIMER CÓDIGO



    //[SerializeField] public GameObject prefabTaskUI;
    //[SerializeField] public List<string> textsToSelect = new List<string>();
    //[SerializeField] string newText = string.Empty;
    //[SerializeField] string textToWrite = string.Empty;
    //[SerializeField] string letra = string.Empty;
    //[SerializeField] public GameObject textUI;
    //[SerializeField] public GameObject insertedText;
    //[SerializeField] public bool tareaFallada;
    //[SerializeField] public bool tareaActiva;

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    GeneradorListaTextos();
    //    tareaFallada = false;
    //    tareaActiva = false;
    //}

    //public void GeneradorListaTextos()
    //{
    //    // Validar que tengamos suficientes tareas
    //    if (textsToSelect == null || textsToSelect.Count == 0)
    //    {
    //        Debug.LogError("textsToSelect está vacío!");
    //        return;
    //    }

    //    MezclarLista(textsToSelect);

    //    textToWrite = textsToSelect[1];


    //    textUI.GetComponent<TextMeshProUGUI>().text = textToWrite;


    //}



    //private void MezclarLista(List<string> list)
    //{
    //    for (int i = list.Count - 1; i > 0; i--)
    //    {
    //        int j = UnityEngine.Random.Range(0, i + 1);
    //        string temp = list[i];
    //        list[i] = list[j];
    //        list[j] = temp;
    //    }
    //}





    //void ActualizarTextoColoreado()
    //{
    //    string resultado = "";

    //    for (int i = 0; i < textToWrite.Length; i++)
    //    {
    //        if (i < newText.Length)
    //        {
    //            if (char.ToLower(newText[i]) == char.ToLower(textToWrite[i]))
    //                resultado += $"<color=green>{textToWrite[i]}</color>";
    //            else
    //                resultado += $"<color=red>{textToWrite[i]}</color>";
    //        }
    //        else
    //        {
    //            resultado += $"<color=white>{textToWrite[i]}</color>";
    //        }
    //    }

    //    textUI.GetComponent<TextMeshProUGUI>().text = resultado;
    //}










    //// Update is called once per frame
    //void Update()
    //{

    //    if (tareaActiva)
    //    {
    //        ActualizarTextoColoreado();

    //        TMP_InputField input = insertedText.GetComponent<TMP_InputField>();
    //        newText = input.text;

    //        // Si no ha escrito nada, no comprobamos
    //        if (string.IsNullOrEmpty(newText))
    //        return;

    //        // Si escribe más caracteres de los que tiene la palabra objetivo
    //        if (newText.Length > textToWrite.Length)
    //        {
    //            tareaFallada = true;
    //        }

    //        // Comprobamos carácter a carácter
    //        for (int i = 0; i < newText.Length; i++)
    //        {
    //            if (char.ToLower(newText[i]) != char.ToLower(textToWrite[i]))
    //            {
    //                tareaFallada = true;
    //            }
    //        }

    //        // Si llega aquí, TODO lo escrito hasta ahora es correcto
    //        Debug.Log("Va bien");

    //        // Si además ya ha terminado la palabra completa
    //        if (newText.Length == textToWrite.Length)
    //        {
    //            tareaFallada = false;
    //            tareaActiva = false;
    //            prefabTaskUI.SetActive(false);
    //        }



    //        //////////////////////////////////////////////////////////////////////////////////////////




    //        ////newText = insertedText.GetComponent<TMP_Text>().text.Trim();
    //        //TMP_InputField input = insertedText.GetComponent<TMP_InputField>();
    //        //newText = input.text/*.Trim()*/;


    //        //if (string.Equals(textToWrite, newText, System.StringComparison.OrdinalIgnoreCase))/*(textToWrite.Trim() == newText)*/
    //        //{
    //        //    Debug.Log("DEBORASTE");
    //        //}




    //    }

    //}

    //public void TaskCode()
    //{
    //    tareaActiva = true;
    //    prefabTaskUI.SetActive(true);

    //}
