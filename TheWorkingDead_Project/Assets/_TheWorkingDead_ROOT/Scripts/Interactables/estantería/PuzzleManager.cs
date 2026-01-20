using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("Game Elements")]
    [Range(1, 6)]
    [SerializeField] private int dificultad;
    [SerializeField] private Transform holder;
    [SerializeField] private Transform piezasPF;

    [Header("Puzzle UI")]
    [SerializeField] private List<Texture2D> imageTextures;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private Image levelSelectPF;

    [SerializeField] private List<Transform> piezas;
    [SerializeField] private Vector2Int dimensiones;
    [SerializeField] private float height;
    [SerializeField] private float width;
    [SerializeField] private Transform draggingPiece = null;
    [SerializeField] private Vector3 offset;

    [SerializeField] private int piezasCorrectas;

    [SerializeField] private int piezasFueraDeLugar = 3;

    [Header("general variables")]
    public bool win;
    public bool loose;
    public Slider timeSlider;
    [SerializeField] float maxTime;
    private float time;
    public bool tiempoStart;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiempoStart = false;
        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;
        time = maxTime;
        win = false;
        loose = false;
        //if (imageTextures.Count == 1)
        //{
        //    levelSelectPanel.gameObject.SetActive(false);
        //    StartPuzzle(imageTextures[0]);
        //    return;
        //}

        foreach (Texture2D texture in imageTextures) 
        {
            Image image = Instantiate(levelSelectPF, levelSelectPanel);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.GetComponent<Button>().onClick.AddListener(delegate {StartPuzzle(texture);});
        }

    }

    void StartPuzzle(Texture2D texture)
    {
        levelSelectPanel.gameObject.SetActive(false);
        piezas = new List<Transform>();
        dimensiones = GetDimensions(texture, dificultad);
        CrearPiezasDePuzzle(texture);
        ColocarSoloAlgunasPiezas();
        Scatter();
    }
    Vector2Int GetDimensions(Texture2D texture, int dificultad)
    {
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
    //Vector2Int GetDimensions(Texture2D texture, int dificultad)
    //{
    //    Vector2Int dimensions = Vector2Int.zero;
    //    if(texture.width < texture.height)
    //    {
    //        dimensiones.x = dificultad;
    //        dimensions.y = (dificultad * texture.height)/texture.width;

    //    }
    //    else
    //    {
    //        dimensions.x = (dificultad * texture.width)/texture.height;
    //        dimensions.y = texture.height;
    //    }

    //    return dimensions;
    //}

    private void CrearPiezasDePuzzle(Texture2D texture)
    {
        height = 1/dimensiones.y;
        float aspect = (float)texture.width/texture.height;
        width = 1/dimensiones.x;

        for (int i = 0; i < dimensiones.y; i++)
        {
            for (int j = 0; j < dimensiones.x; j++)
            {
                Transform pieza = Instantiate(piezasPF, holder);
                pieza.localPosition = new Vector3(
                    (-width * dimensiones.x/2) + (width * j) +(width/2),
                    (-height * dimensiones.y/2) + (height * i) +(height/2),
                    -1);

                pieza.localScale = new Vector3(width, height, 1f);

                pieza.name = $"Pieza {i * dimensiones.x} + j";
                piezas.Add(pieza);


                float width1 = 1f/dimensiones.x;
                float height1 = 1f/dimensiones.y;

                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1 * j, height1 * i);
                uv[1] = new Vector2(width1 * (j+1), height1 * i);
                uv[2] = new Vector2(width1 * j, height1 * (i+1));
                uv[3] = new Vector2(width1 * (j+1), height1 * (i+1));

                Mesh mesh = pieza.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;

                pieza.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
            }
        }
    }

    void ColocarSoloAlgunasPiezas()
    {
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = orthoHeight * screenAspect;

        // Copia de la lista para no repetir piezas
        List<Transform> piezasDisponibles = new List<Transform>(piezas);

        // Ajustamos contador inicial
        piezasCorrectas = piezas.Count - piezasFueraDeLugar;

        for (int i = 0; i < piezasFueraDeLugar && piezasDisponibles.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, piezasDisponibles.Count);
            Transform pieza = piezasDisponibles[randomIndex];
            piezasDisponibles.RemoveAt(randomIndex);

            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            pieza.position = new Vector3(x, y, -1);

            pieza.GetComponent<BoxCollider2D>().enabled = true;
        }

        // Las demás quedan fijas y correctas
        foreach (Transform pieza in piezasDisponibles)
        {
            pieza.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void Scatter()
    {
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (screenAspect + orthoHeight);

        float piezaWidth = width * holder.localScale.x;
        float piezaHeight = height * holder.localScale.y;

        orthoHeight -= piezaHeight;
        piezaWidth -= piezaWidth;

        foreach (Transform pieza in piezas)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            pieza.position = new Vector3(x, y, -1);
        }
    }
    // Update is called once per frame
    void Update()
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

        // CLICK INICIAL
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero
            );

            if (hit && hit.transform.GetComponent<BoxCollider2D>().enabled)
            {
                draggingPiece = hit.transform;

                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = draggingPiece.position.z;

                offset = draggingPiece.position - mouseWorld;

                // Traer al frente
                draggingPiece.position += Vector3.back;
            }
        }

        // ARRASTRAR
        if (Input.GetMouseButton(0) && draggingPiece != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = draggingPiece.position.z;

            draggingPiece.position = mouseWorld + offset;
        }

        // SOLTAR
        if (Input.GetMouseButtonUp(0) && draggingPiece != null)
        {
            SnapAndDisableIfCorrect();
            draggingPiece = null;
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        //    if (hit)
        //    {
        //        draggingPiece = hit.transform;
        //        offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    }

        //    if (draggingPiece && Input.GetMouseButtonUp(0))
        //    {
        //        SnapAndDisableIfCorrect();
        //        draggingPiece = null;
        //        draggingPiece.position += Vector3.forward;
        //    }

        //    if (draggingPiece)
        //    {
        //        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        newPosition += offset;
        //        draggingPiece.position = newPosition;
        //    }

        //}
    }
    private void SnapAndDisableIfCorrect()
    {
        int piezaIndex = piezas.IndexOf(draggingPiece);
        int col = piezaIndex % dimensiones.x;
        int row = piezaIndex / dimensiones.x;
        Vector2 targetPosition = new((-width * dimensiones.x/2) + (width * col) + (width/2), (-height * dimensiones.y/2) + (height*row) + (height/2));
        if (Vector2.Distance(draggingPiece.localPosition, targetPosition)<(width/2))
        {
            draggingPiece.localPosition = targetPosition;
            draggingPiece.GetComponent<BoxCollider2D>().enabled = false;
            piezasCorrectas++;
            if(piezasCorrectas == piezas.Count)
            {
                win = true;
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
