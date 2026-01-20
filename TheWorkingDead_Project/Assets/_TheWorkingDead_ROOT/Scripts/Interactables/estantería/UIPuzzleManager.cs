using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPuzzleManager : MonoBehaviour
{
    [Header("Canvas")]
    public RectTransform holder;
    public RectTransform piezaPrefab;
    public RectTransform panelDist;

    [Header("Fondo")]
    public RectTransform background;

    [Header("Slots manuales (empties)")]
    public List<RectTransform> manualSlots;

    [Header("Puzzle")]
    public List<Sprite> puzzleSprites;
    [Header("Tamaño de piezas")]
    [Range(0.5f, 2f)]
    [SerializeField] private float scalePieza = 1f;

    private List<RectTransform> piezas = new List<RectTransform>();

    private float pieceWidth;
    private float pieceHeight;

    public int piezasCorrectas;
    public int piezasFueraDeLugar = 3;

    [Header("Tiempo")]
    public Slider timeSlider;
    [SerializeField] private float maxTime = 60f;

    private float time;
    public bool tiempoStart;
    public bool win;
    public bool loose;

    void Start()
    {
        tiempoStart = false;
        time = maxTime;

        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;

        win = false;
        loose = false;

        if (puzzleSprites == null || puzzleSprites.Count != manualSlots.Count)
        {
            Debug.LogError("El numero de sprites debe coincidir con el numero de slots");
            return;
        }

        CrearPiezas();
        ColocarSoloAlgunas();
        //AjustarBackground();
    }

    void CrearPiezas()
    {
        piezas.Clear();

        //pieceWidth = manualSlots[0].rect.width;
        //pieceHeight = manualSlots[0].rect.height;
        pieceWidth = manualSlots[0].rect.width * scalePieza;
        pieceHeight = manualSlots[0].rect.height * scalePieza;

        for (int i = 0; i < puzzleSprites.Count; i++)
        {
            RectTransform pieza = Instantiate(piezaPrefab, holder);
            pieza.sizeDelta = new Vector2(pieceWidth, pieceHeight);

            Image img = pieza.GetComponent<Image>();
            img.sprite = puzzleSprites[i];

            PuzzlePieceUI pieceUI = pieza.GetComponent<PuzzlePieceUI>();
            pieceUI.manager = this;
            pieceUI.pieceIndex = i;
            pieceUI.isCorrect = false;

            piezas.Add(pieza);
        }
    }

    void ColocarSoloAlgunas()
    {
        piezasCorrectas = piezas.Count - piezasFueraDeLugar;

        List<RectTransform> disponibles = new List<RectTransform>(piezas);

        for (int i = 0; i < piezasFueraDeLugar; i++)
        {
            int index = Random.Range(0, disponibles.Count);
            RectTransform pieza = disponibles[index];
            disponibles.RemoveAt(index);

            pieza.anchoredPosition = new Vector2(
                Random.Range(-panelDist.rect.width / 2f, panelDist.rect.width / 2f),
                Random.Range(-panelDist.rect.height / 2f, panelDist.rect.height / 2f)
            );
        }

        for (int i = 0; i < disponibles.Count; i++)
        {
            RectTransform pieza = disponibles[i];
            RectTransform slot = manualSlots[pieza.GetComponent<PuzzlePieceUI>().pieceIndex];

            pieza.anchoredPosition = slot.anchoredPosition;
            pieza.GetComponent<PuzzlePieceUI>().isCorrect = true;
        }
    }

    public void TrySnap(PuzzlePieceUI piece)
    {
        RectTransform rt = piece.GetComponent<RectTransform>();
        RectTransform targetSlot = manualSlots[piece.pieceIndex];

        float snapDistance = pieceWidth * 0.4f;

        if (Vector2.Distance(rt.anchoredPosition, targetSlot.anchoredPosition) < snapDistance)
        {
            rt.anchoredPosition = targetSlot.anchoredPosition;
            piece.isCorrect = true;
            piezasCorrectas++;

            if (piezasCorrectas == manualSlots.Count)
            {
                win = true;
                Debug.Log("PUZZLE COMPLETADO");
            }
        }
    }

    //void AjustarBackground()
    //{
    //    background.SetSizeWithCurrentAnchors(
    //        RectTransform.Axis.Horizontal,
    //        holder.rect.width
    //    );

    //    background.SetSizeWithCurrentAnchors(
    //        RectTransform.Axis.Vertical,
    //        holder.rect.height
    //    );

    //    background.anchoredPosition = holder.anchoredPosition;
    //}

    void Update()
    {
        if (!tiempoStart || win || loose)
            return;

        time -= Time.deltaTime;
        timeSlider.value = time;

        if (time <= 0f)
        {
            time = 0f;
            loose = true;
            tiempoStart = false;
            Debug.Log("TIEMPO AGOTADO");
        }
    }
}
