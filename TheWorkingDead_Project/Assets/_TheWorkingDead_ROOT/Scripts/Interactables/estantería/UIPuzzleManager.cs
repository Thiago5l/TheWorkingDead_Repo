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

    [Header("Siluetas del puzzle (ordenadas)")]
    public List<Sprite> silhouetteSprites;
    [Range(0.1f, 1f)]
    public float silhouetteScale = 0.25f;

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
    public float maxTime = 60f;

    public float time;
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
        if (puzzleSprites.Count != manualSlots.Count || silhouetteSprites.Count != manualSlots.Count)
        {
            Debug.LogError("Sprites, siluetas y slots deben coincidir en cantidad");
            return;
        }

        CrearPiezas();
        AsignarSiluetasASlots();
        ColocarSoloAlgunas();
        //AjustarBackground();
    }

    void CrearPiezas()
    {
        piezas.Clear();

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

        // Después de crear todas las piezas, colocarlas aleatoriamente
        ColocarPiezasAleatorias();
    }
    void ColocarPiezasAleatorias()
    {
        List<RectTransform> piezasDisponibles = new List<RectTransform>(piezas);

        foreach (RectTransform pieza in piezasDisponibles)
        {
            pieza.anchoredPosition = new Vector2(
                Random.Range(-panelDist.rect.width / 2f, panelDist.rect.width / 2f),
                Random.Range(-panelDist.rect.height / 2f, panelDist.rect.height / 2f)
            );
        }

        piezasCorrectas = 0; // Todavía ninguna pieza está en su lugar
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

            // Ocultar silueta (hijo)
            Transform sil = targetSlot.Find("Silhouette");
            if (sil != null)
                sil.gameObject.SetActive(false);

            if (piezasCorrectas == manualSlots.Count)
            {
                win = true;
                Debug.Log("PUZZLE COMPLETADO");
            }
        }
    }

    //public void TrySnap(PuzzlePieceUI piece)
    //{
    //    RectTransform rt = piece.GetComponent<RectTransform>();
    //    RectTransform targetSlot = manualSlots[piece.pieceIndex];

    //    float snapDistance = pieceWidth * 0.4f;

    //    if (Vector2.Distance(rt.anchoredPosition, targetSlot.anchoredPosition) < snapDistance)
    //    {
    //        rt.anchoredPosition = targetSlot.anchoredPosition;
    //        piece.isCorrect = true;
    //        piezasCorrectas++;

    //        if (piezasCorrectas == manualSlots.Count)
    //        {
    //            win = true;
    //            Debug.Log("PUZZLE COMPLETADO");
    //        }
    //    }
    //}

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
    void AsignarSiluetasASlots()
    {
        for (int i = 0; i < manualSlots.Count; i++)
        {
            RectTransform slot = manualSlots[i];

            GameObject silhouetteGO = new GameObject("Silhouette");
            silhouetteGO.transform.SetParent(slot);
            silhouetteGO.transform.localScale = Vector3.one;

            RectTransform silRT = silhouetteGO.AddComponent<RectTransform>();
            silRT.anchorMin = silRT.anchorMax = silRT.pivot = new Vector2(0.5f, 0.5f);
            silRT.anchoredPosition = Vector2.zero;

            Image silImage = silhouetteGO.AddComponent<Image>();
            silImage.sprite = silhouetteSprites[i];
            silImage.raycastTarget = false;
            silImage.preserveAspect = true;

            // Tamaño nativo del sprite
            silImage.SetNativeSize();
            Vector3 tamañoSil = Vector3.one;
            silRT.localScale = new Vector3(tamañoSil.x * silhouetteScale, tamañoSil.y * silhouetteScale/*(silhouetteScale * 1.5f)*/, tamañoSil.z);
            // Ajuste visual de la silueta
            silImage.color = new Color(0f, 0f, 0f, 180f);
        }
    }




    //void AsignarSiluetasASlots()
    //{
    //    for (int i = 0; i < manualSlots.Count; i++)
    //    {
    //        Image slotImage = manualSlots[i].GetComponent<Image>();

    //        if (slotImage == null)
    //        {
    //            Debug.LogError("El slot necesita un componente Image");
    //            continue;
    //        }

    //        slotImage.sprite = silhouetteSprites[i];
    //        slotImage.color = new Color(1f, 1f, 1f, 0.4f); // semi-transparente
    //    }
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
