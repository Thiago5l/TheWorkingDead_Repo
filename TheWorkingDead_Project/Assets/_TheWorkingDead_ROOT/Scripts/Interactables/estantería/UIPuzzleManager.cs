using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPuzzleManager : MonoBehaviour
{
    [Header("Zona objetivo")]
    public RectTransform slotPrefab;
    private List<RectTransform> slots = new();

    [Header("Canvas")]
    public Canvas canvas;
    public RectTransform canvasRect;
    public RectTransform holder;
    public RectTransform piezaPrefab;
    public RectTransform panelDist;

    [Header("Puzzle")]
    [Range(1, 6)] public int dificultad;
    public List<Texture2D> imageTextures;
    public Vector3[] corners;

    private List<RectTransform> piezas = new();
    private Vector2Int dimensiones;
    private float pieceWidth;
    private float pieceHeight;

    public int piezasCorrectas;
    public int piezasFueraDeLugar = 3;

    public bool win;
    public bool loose;
    public Slider timeSlider;
    [SerializeField] float maxTime;
    private float time;
    public bool tiempoStart;

    void Start()
    {
        tiempoStart = false;
        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;
        time = maxTime;
        win = false;
        loose = false;

        Debug.Log("UIPuzzleManager START");
        corners = new Vector3[4];
        if (imageTextures == null || imageTextures.Count == 0)
        {
            Debug.LogError("No hay texturas asignadas");
            return;
        }

        Debug.Log("Iniciando puzzle con textura: " + imageTextures[0].name);
        StartPuzzle(imageTextures[0]);
    }

    void StartPuzzle(Texture2D texture)
    {
        Debug.Log("StartPuzzle()");

        dimensiones = GetDimensions(texture, dificultad);
        Debug.Log($"Dimensiones puzzle: {dimensiones.x} x {dimensiones.y}");

        CrearPiezas(texture);
        CrearSlots();
        ColocarSoloAlgunas();
    }

    Vector2Int GetDimensions(Texture2D texture, int dificultad)
    {
        Debug.Log("Calculando dimensiones | dificultad: " + dificultad);

        if (texture.width < texture.height)
        {
            int y = Mathf.RoundToInt((float)dificultad * texture.height / texture.width);
            return new Vector2Int(dificultad, y);
        }
        else
        {
            int x = Mathf.RoundToInt((float)dificultad * texture.width / texture.height);
            return new Vector2Int(x, dificultad);
        }
    }

    void CrearPiezas(Texture2D texture)
    {
        Debug.Log("CrearPiezas()");

        pieceWidth = holder.rect.width / dimensiones.x;
        pieceHeight = holder.rect.height / dimensiones.y;

        Debug.Log($"Tamaño pieza: {pieceWidth} x {pieceHeight}");

        for (int y = 0; y < dimensiones.y; y++)
        {
            for (int x = 0; x < dimensiones.x; x++)
            {
                Debug.Log($"Creando pieza [{x},{y}]");

                RectTransform pieza = Instantiate(piezaPrefab, holder);
                pieza.sizeDelta = new Vector2(pieceWidth, pieceHeight);

                Vector2 pos = new(
                    x * pieceWidth + pieceWidth / 2,
                    -y * pieceHeight - pieceHeight / 2
                );
                pieza.anchoredPosition = pos;

                Image img = pieza.GetComponent<Image>();
                if (img == null)
                {
                    Debug.LogError("La pieza no tiene Image");
                    continue;
                }

                Rect rect = new Rect(
                    x * texture.width / dimensiones.x,
                    (dimensiones.y - y - 1) * texture.height / dimensiones.y,
                    texture.width / dimensiones.x,
                    texture.height / dimensiones.y
                );

                img.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                Debug.Log("Sprite creado: " + rect);

                PuzzlePieceUI pieceUI = pieza.GetComponent<PuzzlePieceUI>();
                if (pieceUI == null)
                {
                    Debug.LogError("Falta PuzzlePieceUI en el prefab");
                    continue;
                }

                pieceUI.manager = this;
                pieceUI.pieceIndex = y * dimensiones.x + x;

                piezas.Add(pieza);
            }
        }

        Debug.Log("Total piezas creadas: " + piezas.Count);
    }

    void ColocarSoloAlgunas()
    {
        Debug.Log("ColocarSoloAlgunas()");

        piezasCorrectas = piezas.Count - piezasFueraDeLugar;
        Debug.Log("Piezas correctas iniciales: " + piezasCorrectas);

        List<RectTransform> disponibles = new(piezas);

        for (int i = 0; i < piezasFueraDeLugar; i++)
        {
            int index = Random.Range(0, disponibles.Count);
            RectTransform pieza = disponibles[index];
            disponibles.RemoveAt(index);

            
            Vector2 randomPos = new Vector2(
                Random.Range(-panelDist.rect.width / 2, panelDist.rect.width / 2),
                Random.Range(-panelDist.rect.height / 2, panelDist.rect.height / 2)
            );

            pieza.anchoredPosition = randomPos;
            Debug.Log("Pieza movida fuera: " + pieza.name + " -> " + randomPos);
        }

        foreach (var p in disponibles)
        {
            p.GetComponent<PuzzlePieceUI>().isCorrect = true;
            Debug.Log("Pieza fija: " + p.name);
        }
    }

    public void TrySnap(PuzzlePieceUI piece)
    {
        Debug.Log("TrySnap pieza index: " + piece.pieceIndex);

        int index = piece.pieceIndex;
        int col = index % dimensiones.x;
        int row = index / dimensiones.x;

        Vector2 target = new(
            col * pieceWidth + pieceWidth / 2,
            -row * pieceHeight - pieceHeight / 2
        );

        RectTransform rt = piece.GetComponent<RectTransform>();
        float dist = Vector2.Distance(rt.anchoredPosition, target);

        Debug.Log("Distancia a target: " + dist);

        if (dist < pieceWidth * 0.4f)
        {
            rt.anchoredPosition = target;
            piece.isCorrect = true;
            piezasCorrectas++;

            Debug.Log("Pieza encajada | Total correctas: " + piezasCorrectas);

            if (piezasCorrectas == piezas.Count)
            {
                win = true;
                Debug.Log("PUZZLE COMPLETADO");
            }
        }
        else
        {
            Debug.Log("No encajó");
        }
    }
    void CrearSlots()
    {
        slots.Clear();

        for (int y = 0; y < dimensiones.y; y++)
        {
            for (int x = 0; x < dimensiones.x; x++)
            {
                RectTransform slot = Instantiate(slotPrefab, holder);
                slot.SetAsFirstSibling(); // Siempre detrás de las piezas

                slot.sizeDelta = new Vector2(pieceWidth, pieceHeight);

                Vector2 pos = new(
                    x * pieceWidth + pieceWidth / 2,
                    -y * pieceHeight - pieceHeight / 2
                );

                slot.anchoredPosition = pos;
                slots.Add(slot);
            }
        }

        Debug.Log("Zona objetivo creada: " + slots.Count + " slots");
    }

    private void Update()
    {
        if (tiempoStart)
        {
            time -= Time.deltaTime;
            timeSlider.value = time;

            if (time <= 0f)
            {
                loose = true;
                tiempoStart = false;
            }
        }
    }

    public void startTimer()
    {
        tiempoStart = true;
    }
    public void stopTimer()
    {
        tiempoStart = false;
    }
}
