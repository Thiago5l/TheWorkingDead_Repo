using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]TMP_Text NumberofCoins;
    [SerializeField]PlayerController playerController;

    public RectTransform uiCoins; // Asigna tu objeto Canvas aquí
    public float scaleFactor = 1.5f; // Hasta qué tamaño quieres que crezca
    public float duration = 0.5f; // Duración de la animación
    private void Awake()
    {
        Updatecoins();
    }
    public void Updatecoins()
    {
        NumberofCoins.text = playerController.coins.ToString();

    }
    public void AnimateUI()
    {
        // Guardamos el tamaño original
        Vector3 originalScale = uiCoins.localScale;

        // Animamos a un tamaño más grande y volvemos al original
        uiCoins.DOScale(originalScale * scaleFactor, duration / 2)
                 .SetLoops(2, LoopType.Yoyo)
                 .SetEase(Ease.OutBack); // Puedes cambiar la easing
    }
}
