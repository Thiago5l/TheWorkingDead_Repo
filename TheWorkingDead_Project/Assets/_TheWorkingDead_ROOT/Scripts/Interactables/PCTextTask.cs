using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PCTextTask : MonoBehaviour
{
    [Header("Tareas aleatorias")]
    [SerializeField] public GameObject objectTareas;
    private TareasAleatorias tareasScript;

    [Header("Texto")]
    [SerializeField] public GameObject prefabTaskUI;
    [SerializeField] public List<string> textsToSelect = new List<string>();
    [SerializeField] public List<string> textsSelected = new List<string>();
    [SerializeField] public int tamañoTextsSelected;
    [SerializeField] string newText = string.Empty;
    [SerializeField] string textToWrite = string.Empty;
    [SerializeField] public GameObject textUI;
    [SerializeField] public TMP_InputField insertedText;

    [Header("Tiempo")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] private float tiempoMax;
    [SerializeField] private float tiempoActual;
    [SerializeField] private float tiempoPorCaracter = 0.5f;

    [Header("Interacción y estado")]
    [SerializeField] Material Mat;
    [SerializeField] Material OutLine;
    [SerializeField] bool PlayerCerca;
    [SerializeField] bool tareaEnProceso;
    [SerializeField] bool tareaAcabada;

    [Header("Penalización")]
    [SerializeField] GameObject Player;
    [SerializeField] float penalizacion = 5f;

    void Start()
    {
        tareasScript = objectTareas.GetComponent<TareasAleatorias>();
        tareaEnProceso = false;
        tareaAcabada = false;
        PlayerCerca = false;
        prefabTaskUI.SetActive(false);
        GeneradorListaTextos();
    }

    public void GeneradorListaTextos()
    {
        if (textsToSelect.Count < tamañoTextsSelected)
        {
            Debug.LogError("No hay suficientes palabras en textsToSelect");
            return;
        }

        textsSelected.Clear();
        MezclarLista(textsToSelect);

        for (int i = 0; i < tamañoTextsSelected; i++)
            textsSelected.Add(textsToSelect[i]);

        PasarSiguientePalabra();
    }

    public void PasarSiguientePalabra()
    {
        insertedText.text = string.Empty;
        newText = string.Empty;

        if (textsSelected.Count > 0)
        {
            textToWrite = textsSelected[0];
            textUI.GetComponent<TextMeshProUGUI>().text = textToWrite;
            ConfigurarTiempo();
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !tareaAcabada)
        {
            PlayerCerca = true;
            GetComponent<MeshRenderer>().material = OutLine;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            PlayerCerca = false;
            GetComponent<MeshRenderer>().material = Mat;
        }
    }

    void Update()
    {
        if (tareaAcabada) return;

        if (tareaEnProceso)
        {
            tiempoActual -= Time.deltaTime;
            timeSlider.value = tiempoActual;
            if (tiempoActual <= 0f)
                FallarTarea();
        }
    }

    public void Interactuar()
    {
        if (PlayerCerca && !tareaAcabada && !tareaEnProceso)
        {
            tareaEnProceso = true;
            prefabTaskUI.SetActive(true);
            insertedText.text = string.Empty;
            insertedText.ActivateInputField();
            Player.GetComponent<PlayerController>().playerOcupado = true;
        }
    }

    void ConfigurarTiempo()
    {
        tiempoMax = textToWrite.Length * tiempoPorCaracter;
        tiempoActual = tiempoMax;
        timeSlider.maxValue = tiempoMax;
        timeSlider.value = tiempoMax;
    }

    void LateUpdate()
    {
        if (!tareaEnProceso || tareaAcabada) return;

        newText = insertedText.text;
        ActualizarTextoColoreado();

        if (string.IsNullOrEmpty(newText)) return;
        if (newText.Length > textToWrite.Length) { FallarTarea(); return; }

        for (int i = 0; i < newText.Length; i++)
            if (char.ToLower(newText[i]) != char.ToLower(textToWrite[i])) { FallarTarea(); return; }

        if (newText.Length == textToWrite.Length)
        {
            textsSelected.RemoveAt(0);
            if (textsSelected.Count > 0) PasarSiguientePalabra();
            else CompletarTarea();
        }
    }

    void FallarTarea()
    {
        tareaEnProceso = false;
        Player.GetComponent<PlayerController>().playerOcupado = false;
        timeSlider.value = 0;
        prefabTaskUI.SetActive(false);
        Player.GetComponent<OviedadZombie>().Zombiedad += (penalizacion / 100);
        GetComponent<MeshRenderer>().material = Mat;
    }

    void CompletarTarea()
    {
        tareaEnProceso = false;
        tareaAcabada = true;
        Player.GetComponent<PlayerController>().playerOcupado = false;
        prefabTaskUI.SetActive(false);
        GetComponent<MeshRenderer>().material = Mat;

        // Se comunica directamente con TareasAleatorias
        tareasScript.CompletarTarea(this.gameObject);
        this.enabled = false;
    }

    void ActualizarTextoColoreado()
    {
        string resultado = "";
        for (int i = 0; i < textToWrite.Length; i++)
        {
            if (i < newText.Length)
                resultado += (char.ToLower(newText[i]) == char.ToLower(textToWrite[i])) ? $"<color=green>{textToWrite[i]}</color>" : $"<color=red>{textToWrite[i]}</color>";
            else
                resultado += $"<color=white>{textToWrite[i]}</color>";
        }
        textUI.GetComponent<TextMeshProUGUI>().text = resultado;
    }
}


//textToWrite = textsSelected[0];
//textUI.GetComponent<TextMeshProUGUI>().text = textToWrite;

//ConfigurarTiempo();




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





//////////////////////////////////////////////////////////////////////////








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
