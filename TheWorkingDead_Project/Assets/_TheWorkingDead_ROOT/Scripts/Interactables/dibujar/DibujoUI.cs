using UnityEngine;
using UnityEngine.UI;

public class DibujoUI : MonoBehaviour
{
    public RawImage rawImage;
    public int texturaAncho = 512;
    public int texturaAlto = 512;
    public Color colorLinea = Color.black;
    [Range(0f, 100f)]
    public int grosor = 4;

    private Texture2D textura;
    private Vector2 ultimoPunto;

    void Start()
    {
        textura = new Texture2D(texturaAncho, texturaAlto, TextureFormat.RGBA32, false);
        textura.filterMode = FilterMode.Point;

        Color[] pixeles = new Color[texturaAncho * texturaAlto];
        for (int i = 0; i < pixeles.Length; i++)
            pixeles[i] = Color.clear;

        textura.SetPixels(pixeles);
        textura.Apply();

        rawImage.texture = textura;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                rawImage.rectTransform, Input.mousePosition)) return;

            ultimoPunto = ObtenerPosicion();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 puntoActual = ObtenerPosicion();
            DibujarLinea(ultimoPunto, puntoActual);
            ultimoPunto = puntoActual;
        }
    }

    Vector2 ObtenerPosicion()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rawImage.rectTransform,
            Input.mousePosition,
            null,
            out Vector2 localPos
        );

        Rect rect = rawImage.rectTransform.rect;

        float x = (localPos.x - rect.x) / rect.width * texturaAncho;
        float y = (localPos.y - rect.y) / rect.height * texturaAlto;

        return new Vector2(x, y);
    }

    void DibujarLinea(Vector2 a, Vector2 b)
    {
        int pasos = Mathf.CeilToInt(Vector2.Distance(a, b));
        for (int i = 0; i < pasos; i++)
        {
            Vector2 p = Vector2.Lerp(a, b, i / (float)pasos);
            DibujarPunto((int)p.x, (int)p.y);
        }
        textura.Apply();
    }

    void DibujarPunto(int x, int y)
    {
        for (int i = -grosor; i <= grosor; i++)
            for (int j = -grosor; j <= grosor; j++)
            {
                int px = x + i;
                int py = y + j;

                if (px >= 0 && px < texturaAncho && py >= 0 && py < texturaAlto)
                    textura.SetPixel(px, py, colorLinea);
            }
    }
}