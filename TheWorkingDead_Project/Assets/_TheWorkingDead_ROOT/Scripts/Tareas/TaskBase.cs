using UnityEngine;

public abstract class TaskBase : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] protected GameObject canvasInteractKey;
    [SerializeField] protected GameObject uiTarea;
    [SerializeField] protected GameObject particles;
    [SerializeField] protected GameObject player;
    [SerializeField] protected TareasAleatorias taskManager;
    [SerializeField] protected GameObject taskExclamation;
    [SerializeField] FadeCanvas feedbackcanvas;

    [Header("Outline")]
    [SerializeField] protected Renderer objRenderer;
    [SerializeField] protected Color colorCerca = Color.yellow;
    [SerializeField] protected Color colorLejos = Color.black;

    [Header("Flags")]
    public bool playerCerca;
    public bool tareaAcabada;
    public bool interactuando;

    private void LateUpdate()
    {
        ActualizarCanvasInteract();
        ActualizarExclamacion();
    }
    private void OnEnable()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (uiTarea == null)
            uiTarea = GameObject.FindWithTag("UiNpcConversation");
        if (uiTarea == null)
            uiTarea = GameObject.FindWithTag("BrazoCaidoFeedback");
        if (taskManager == null)
            taskManager = FindAnyObjectByType<TareasAleatorias>();
        if (feedbackcanvas == null)
            feedbackcanvas = FindAnyObjectByType<FadeCanvas>();
    }
    protected virtual void Start()
    {
        if (uiTarea != null) uiTarea.SetActive(false);
        if (particles != null) particles.SetActive(true);
        if (canvasInteractKey != null) canvasInteractKey.SetActive(false);
        if (objRenderer == null) objRenderer = GetComponent<Renderer>();
        tareaAcabada = false;
    }

    #region Trigger

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("TaskPlayer")&&!tareaAcabada) return;

        playerCerca = true;
        if (EstaEnListaDeTareas())
        CambiarColorOutline(colorCerca);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("TaskPlayer")) return;

        playerCerca = false;


        CambiarColorOutline(colorLejos);

        if (interactuando && !tareaAcabada)
            CancelarTarea();
    }

    #endregion

    #region Public Interaction

    public virtual void Interactuar()
    {
        if (!EstaEnListaDeTareas()) return;

        if (!feedbackcanvas.brazoYaCaido)
        {
            if (playerCerca && !tareaAcabada && !interactuando)
            {
                uiTarea.SetActive(true);

                interactuando = true;

                if (player != null)
                    player.GetComponent<PlayerController>().playerOcupado = true;

                canvasInteractKey?.SetActive(false);

                IniciarTarea();
            }

        }
    }

    #endregion

    #region Base States

    protected void CompletarTarea()
    {
        if (taskExclamation != null)
            taskExclamation.SetActive(false);
        taskManager.CompletarTarea(this.gameObject);
        tareaAcabada = true;
        interactuando = false;

        if (particles != null) particles.SetActive(false);
        if (uiTarea != null) uiTarea.SetActive(false);
        if (canvasInteractKey != null) canvasInteractKey.SetActive(false);
        if (player != null) player.GetComponent<PlayerController>().playerOcupado = false;

        StopAllCoroutines();
        this.enabled = false;
    }

    protected void CancelarBase()
    {
        interactuando = false;

        if (uiTarea != null) uiTarea.SetActive(false);
        if (canvasInteractKey != null) canvasInteractKey.SetActive(false);
        if (player != null) player.GetComponent<PlayerController>().playerOcupado = false;

        StopAllCoroutines();
    }

    #endregion

    #region Visual

    void ActualizarCanvasInteract()
    {
        if (canvasInteractKey == null) return;

        bool mostrar =
            playerCerca &&
            EstaEnListaDeTareas() &&
            !tareaAcabada &&
            !interactuando;

        canvasInteractKey.SetActive(mostrar);
    }

    protected void CambiarColorOutline(Color color)
    {
        if (objRenderer == null) return;

        Material[] mats = objRenderer.materials;
        if (mats.Length > 1)
            mats[1].SetColor("_ContourColor", color);
    }

    public bool EstaEnListaDeTareas()
    {
        return taskManager != null && taskManager.OrdenTareas.Contains(this.gameObject);
    }

    void ActualizarExclamacion()
    {
        if (taskExclamation == null) return;

        if (tareaAcabada)
        {
            taskExclamation.SetActive(false);
            return;
        }

        bool mostrar =
            EstaEnListaDeTareas() &&
            !playerCerca &&
            !interactuando;

        taskExclamation.SetActive(mostrar);
    }



    #endregion
    #region win/loose
    protected void Win()
    {
        tareaAcabada = true;
        interactuando = false;
        feedbackcanvas.PlayWin();
        player.gameObject.GetComponent<PlayerController>().playerOcupado = false;
        uiTarea.gameObject.SetActive(false);
        interactuando = false;
        StopAllCoroutines();
        CompletarTarea();
        if (taskExclamation != null)
            taskExclamation.SetActive(false);
    }
    protected void Loose()
    {
        feedbackcanvas.PlayLose();
        player.gameObject.GetComponent<PlayerController>().playerOcupado = false;
        uiTarea.gameObject.SetActive(false);
        interactuando = false;
        StopAllCoroutines();
    }
    #endregion
    #region Abstract

    protected abstract void IniciarTarea();
    protected abstract void CancelarTarea();

    #endregion
}
