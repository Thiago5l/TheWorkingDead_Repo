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

    [Header("Máximo ancho del tooltip")]
    public float anchoMaximo = 300f;

    [Header("Color del texto")]
    public Color colorTexto = Color.white;

    [Header("Borde del texto")]
    public Color colorBorde = Color.black;
    [Range(0f, 1f)]
    public float grosorBorde = 0.2f;

    [Header("Imagen de fondo opcional")]
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

        Image bg = tooltipGO.AddComponent<Image>();
        if (fondoImagen != null)
        {
            bg.sprite = fondoImagen;
            bg.type = Image.Type.Sliced;
        }
        else
        {
            bg.color = new Color(0, 0, 0, 0.6f);
        }
        bg.raycastTarget = false;

        tooltipRT = tooltipGO.GetComponent<RectTransform>();
        tooltipRT.pivot = new Vector2(0.5f, 1f);

        GameObject textGO = new GameObject("TextoTooltip");
        textGO.transform.SetParent(tooltipGO.transform, false);

        tooltipTMP = textGO.AddComponent<TextMeshProUGUI>();
        tooltipTMP.font = fuentePersonalizada;
        tooltipTMP.fontSize = fontSizeFijo;
        tooltipTMP.color = colorTexto;
        tooltipTMP.alignment = TextAlignmentOptions.Center;
        tooltipTMP.enableWordWrapping = true;
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
        if (!gameObject.activeInHierarchy) return;

        Canvas canvas = GetComponentInParent<Canvas>();
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;

        RectTransform targetRT = GetComponent<RectTransform>();
        Vector2 localMousePos;
        bool dentro = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetRT,
            Input.mousePosition,
            cam,
            out localMousePos
        ) && targetRT.rect.Contains(localMousePos);

        if (dentro)
        {
            tooltipTMP.text = nombreObjeto;
            tooltipTMP.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, anchoMaximo - padding.x * 2f);
            Vector2 textSize = tooltipTMP.GetPreferredValues(nombreObjeto, anchoMaximo, 1000f);
            tooltipRT.sizeDelta = new Vector2(Mathf.Min(textSize.x + padding.x * 2f, anchoMaximo), textSize.y + padding.y * 2f);

            tooltipGO.transform.SetAsLastSibling();

            if (fadeTween != null) fadeTween.Kill();
            fadeTween = canvasGroup.DOFade(1f, 0.2f).SetUpdate(true);
        }
        else
        {
            if (fadeTween != null) fadeTween.Kill();
            fadeTween = canvasGroup.DOFade(0f, 0.2f).SetUpdate(true);
        }

        if (canvasGroup.alpha > 0f)
        {
            Vector2 pos = Input.mousePosition;
            float width = tooltipRT.sizeDelta.x;
            float height = tooltipRT.sizeDelta.y;

            tooltipRT.pivot = new Vector2(0.5f, 1f);

            pos.x = Mathf.Clamp(pos.x, width / 2f, Screen.width - width / 2f);
            pos.y = Mathf.Clamp(pos.y, height, Screen.height);

            tooltipGO.transform.position = pos;
        }
    }

    private void OnDestroy()
    {
        if (fadeTween != null) fadeTween.Kill();
        if (tooltipGO != null) Destroy(tooltipGO);
    }
}
