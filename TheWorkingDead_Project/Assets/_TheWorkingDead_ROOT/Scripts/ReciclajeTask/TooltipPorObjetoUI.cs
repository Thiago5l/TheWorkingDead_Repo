using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class TooltipPorObjetoUI : MonoBehaviour
{
    [Header("Nombre del objeto")]
    public string nombreObjeto;

    [Header("Fuente y tamaño")]
    public TMP_FontAsset fuentePersonalizada;
    public int fontSizeFijo = 24;
    public Vector2 padding = new Vector2(10f, 5f);

    [Header("Color del texto")]
    public Color colorTexto = Color.white;

    [Header("Borde del texto")]
    public Color colorBorde = Color.black;
    [Range(0f, 1f)]
    public float grosorBorde = 0.2f;

    [Header("Imagen de fondo (opcional)")]
    public Sprite fondoImagen;

    private GameObject tooltipGO;
    private TextMeshProUGUI tooltipTMP;
    private RectTransform tooltipRT;
    private CanvasGroup canvasGroup;
    private Tween fadeTween;

    void Awake()
    {
        CrearTooltip();
    }

    void CrearTooltip()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) canvas = FindObjectOfType<Canvas>();

        tooltipGO = new GameObject("Tooltip_" + nombreObjeto);
        tooltipGO.transform.SetParent(canvas.transform, false);

        canvasGroup = tooltipGO.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        // Fondo
        Image bg = tooltipGO.AddComponent<Image>();
        if (fondoImagen != null)
        {
            bg.sprite = fondoImagen;
            bg.type = Image.Type.Sliced;
            bg.pixelsPerUnitMultiplier = 1f;
        }
        else
        {
            bg.color = new Color(0, 0, 0, 0.6f);
        }
        bg.raycastTarget = false;

        tooltipRT = tooltipGO.GetComponent<RectTransform>();
        tooltipRT.pivot = new Vector2(0, 1);
        tooltipRT.sizeDelta = new Vector2(200, 50);

        // Texto
        GameObject textGO = new GameObject("TextoTooltip");
        textGO.transform.SetParent(tooltipGO.transform, false);

        tooltipTMP = textGO.AddComponent<TextMeshProUGUI>();
        tooltipTMP.font = fuentePersonalizada;
        tooltipTMP.fontSize = fontSizeFijo;
        tooltipTMP.color = colorTexto;
        tooltipTMP.alignment = TextAlignmentOptions.Center;
        tooltipTMP.enableAutoSizing = false;
        tooltipTMP.raycastTarget = false;

        tooltipTMP.fontSharedMaterial.EnableKeyword("OUTLINE_ON");
        tooltipTMP.fontSharedMaterial.SetColor("_OutlineColor", colorBorde);
        tooltipTMP.fontSharedMaterial.SetFloat("_OutlineWidth", grosorBorde);

        RectTransform textRT = tooltipTMP.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;
    }

    void Update()
    {
        if (tooltipGO == null) return;

        Canvas canvas = GetComponentInParent<Canvas>();
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;

        Vector2 localMousePos;
        bool dentro = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            Input.mousePosition,
            cam,
            out localMousePos
        ) && GetComponent<RectTransform>().rect.Contains(localMousePos);

        if (dentro)
        {
            tooltipTMP.text = nombreObjeto;

            Vector2 textSize = tooltipTMP.GetPreferredValues(nombreObjeto);
            tooltipRT.sizeDelta = new Vector2(textSize.x + padding.x * 2f, textSize.y + padding.y * 2f);

            tooltipGO.transform.SetAsLastSibling();

            if (fadeTween != null) fadeTween.Kill();
            fadeTween = canvasGroup.DOFade(1f, 0.2f);
        }
        else
        {
            if (fadeTween != null) fadeTween.Kill();
            fadeTween = canvasGroup.DOFade(0f, 0.2f);
        }

        if (canvasGroup.alpha > 0f)
        {
            Vector2 pos = Input.mousePosition;

            float width = tooltipRT.sizeDelta.x;
            float height = tooltipRT.sizeDelta.y;
            float screenW = Screen.width;
            float screenH = Screen.height;

            if (pos.x + width > screenW) pos.x = screenW - width;
            if (pos.y - height < 0) pos.y = height;

            tooltipGO.transform.position = pos;
        }
    }

    private void OnDestroy()
    {
        if (tooltipGO != null) Destroy(tooltipGO);
    }
}
