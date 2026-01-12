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

    protected bool playerCerca;
    protected bool tareaAcabada;
    protected bool interactuando;

    private void LateUpdate()
    {
        if (taskExclamation != null && !playerCerca && !tareaAcabada)
            taskExclamation.SetActive(EstaEnListaDeTareas());
    }

    private void Awake()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (uiTarea == null)
            uiTarea = GameObject.FindWithTag("UiNpcConversation");
        if (taskManager == null)
            taskManager = FindAnyObjectByType<TareasAleatorias>();
    }

    protected virtual void Start()
    {
        if (uiTarea != null) uiTarea.SetActive(false);
        if (particles != null) particles.SetActive(true);
        if (canvasInteractKey != null) canvasInteractKey.SetActive(false);
        if (taskExclamation != null)
            taskExclamation.SetActive(EstaEnListaDeTareas());
        if (objRenderer == null) objRenderer = GetComponent<Renderer>();
    }

    #region Trigger

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("TaskPlayer") || tareaAcabada) return;

        playerCerca = true;

        // Mostrar canvas solo si está en la lista
        if (EstaEnListaDeTareas())
            canvasInteractKey?.SetActive(true);
        else
            canvasInteractKey?.SetActive(false);

        // Ocultar exclamación siempre al acercarse
        if (taskExclamation != null)
            taskExclamation.SetActive(false);

        CambiarColorOutline(colorCerca);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("TaskPlayer")) return;

        playerCerca = false;

        // Ocultar canvas interactivo
        if (canvasInteractKey != null)
            canvasInteractKey.SetActive(false);

        // Mostrar exclamación solo si está en la lista y no completada
        if (EstaEnListaDeTareas() && !tareaAcabada)
            taskExclamation?.SetActive(true);

        CambiarColorOutline(colorLejos);

        if (interactuando && !tareaAcabada)
            CancelarTarea();
    }

    #endregion

    #region Public Interaction

    public virtual void Interactuar()
    {
        if (!playerCerca || tareaAcabada || interactuando) return;

        interactuando = true;

        if (player != null)
            player.GetComponent<PlayerController>().playerOcupado = true;

        if (canvasInteractKey != null)
            canvasInteractKey.SetActive(false);

        if (uiTarea != null)
            uiTarea.SetActive(true);

        IniciarTarea();
    }

    #endregion

    #region Base States

    protected void CompletarTarea()
    {
        tareaAcabada = true;
        interactuando = false;

        if (particles != null) particles.SetActive(false);
        if (uiTarea != null) uiTarea.SetActive(false);
        if (canvasInteractKey != null) canvasInteractKey.SetActive(false);
        if (taskExclamation != null) taskExclamation.SetActive(false);
        if (player != null) player.GetComponent<PlayerController>().playerOcupado = false;

        StopAllCoroutines();
        this.enabled = false;
        ActualizarExclamacion();
    }

    protected void CancelarBase()
    {
        interactuando = false;

        if (uiTarea != null) uiTarea.SetActive(false);
        if (canvasInteractKey != null) canvasInteractKey.SetActive(false);
        if (player != null) player.GetComponent<PlayerController>().playerOcupado = false;

        StopAllCoroutines();
        ActualizarExclamacion();
    }

    #endregion

    #region Visual

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

    protected void ActualizarExclamacion()
    {
        if (taskExclamation != null)
            taskExclamation.SetActive(EstaEnListaDeTareas() && !playerCerca && !tareaAcabada);
    }

    #endregion
    #region win/loose
    protected void Win()
    { 
        feedbackcanvas.PlayWin();
        player.gameObject.GetComponent<PlayerController>().playerOcupado = false;
        uiTarea.gameObject.SetActive(false);
        interactuando = false;
        StopAllCoroutines();
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
