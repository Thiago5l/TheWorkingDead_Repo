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

    void KillTween()
    {
        if (tweenActivo != null && tweenActivo.IsActive())
            tweenActivo.Kill();
    }

    public void Agrandar()
    {
        if (bloqueado) return;
        KillTween();
        tweenActivo = rectTransform.DOScale(scaleOriginal * 1.1f, 0.12f).SetUpdate(true);
    }

    public void RestaurarTamano()
    {
        if (bloqueado) return;
        KillTween();
        tweenActivo = rectTransform.DOScale(scaleOriginal, 0.12f).SetUpdate(true);
    }

    public void Felicidad()
    {
        bloqueado = true;
        KillTween();

        Sequence seq = DOTween.Sequence();

        seq.Append(rectTransform.DOScale(scaleOriginal * 0.9f, 0.1f).SetEase(Ease.OutQuad));
        seq.Join(rectTransform.DOAnchorPosY(posOriginal.y + 15f, 0.1f).SetEase(Ease.OutQuad));
        seq.Append(rectTransform.DOScale(scaleOriginal, 0.12f).SetEase(Ease.OutBack));
        seq.Join(rectTransform.DOAnchorPosY(posOriginal.y, 0.12f).SetEase(Ease.OutBack));

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
        seq.Append(rectTransform.DOShakeAnchorPos(0.25f, new Vector2(35f, 10f), 35, 90f, false, true));
        seq.Join(rectTransform.DOScale(scaleOriginal * 1.05f, 0.12f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Flash));
        seq.OnComplete(() =>
        {
            rectTransform.anchoredPosition = posOriginal;
            rectTransform.localScale = scaleOriginal;
            bloqueado = false;
        });

        tweenActivo = seq;
    }
}
