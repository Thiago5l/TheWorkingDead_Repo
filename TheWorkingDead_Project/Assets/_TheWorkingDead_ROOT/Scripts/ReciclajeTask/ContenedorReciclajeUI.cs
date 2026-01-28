using UnityEngine;
using DG.Tweening;

public class ContenedorReciclajeUI : MonoBehaviour
{
    public TipoReciclaje tipoAceptado;

    private RectTransform rectTransform;
    private Vector3 scaleOriginal;
    private Vector2 posOriginal;

    private Tween tweenActivo;
    private bool bloqueado = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        scaleOriginal = rectTransform.localScale;
        posOriginal = rectTransform.anchoredPosition;
    }

    // 🔹 Ahora público para que DragUI lo pueda llamar
    public void KillTween()
    {
        if (tweenActivo != null && tweenActivo.IsActive())
        {
            tweenActivo.Kill();
            tweenActivo = null;
        }
    }

    public void Agrandar()
    {
        if (bloqueado) return;

        KillTween();
        tweenActivo = rectTransform.DOScale(scaleOriginal * 1.15f, 0.15f)
            .SetUpdate(true);
    }

    public void RestaurarTamano()
    {
        if (bloqueado) return;

        KillTween();
        tweenActivo = rectTransform.DOScale(scaleOriginal, 0.15f)
            .SetUpdate(true);
    }

    public void Felicidad()
    {
        bloqueado = true;
        KillTween();

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOScale(scaleOriginal * 0.85f, 0.18f));
        seq.Join(rectTransform.DOAnchorPosY(posOriginal.y + 40f, 0.18f));
        seq.Append(rectTransform.DOScale(scaleOriginal, 0.18f));
        seq.Join(rectTransform.DOAnchorPosY(posOriginal.y, 0.18f));

        tweenActivo = seq;
        seq.OnComplete(() =>
        {
            bloqueado = false;
            rectTransform.localScale = scaleOriginal;
            rectTransform.anchoredPosition = posOriginal;
        });
    }

    public void Tremble()
    {
        bloqueado = true;
        KillTween();

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOShakeAnchorPos(
            0.18f,
            new Vector2(25f, 0f),
            25,
            90f,
            false,
            true
        ));

        seq.OnComplete(() =>
        {
            rectTransform.anchoredPosition = posOriginal;
            rectTransform.localScale = scaleOriginal;
            bloqueado = false;
        });

        tweenActivo = seq;
    }

    private void OnDestroy()
    {
        KillTween();
    }
}
