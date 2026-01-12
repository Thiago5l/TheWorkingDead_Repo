using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening; // <--- Import DOTween

public class EnergeticasUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject energeticaPrefab;
    [SerializeField] Transform gridParent;
    [SerializeField] public PlayerController playerController;

    private List<GameObject> icons = new List<GameObject>();

    private void Awake()
    {
        if(gridParent == null) gridParent = transform;
    }

    public void SetEnergeticas(int amount)
    {
        // Crear iconos si faltan
        while (icons.Count < amount)
        {
            GameObject icon = Instantiate(energeticaPrefab, gridParent);
            icon.transform.localScale = Vector3.zero; // empezar invisible
            icons.Add(icon);

            // Animación de aparecer
            icon.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        // Activar / desactivar según cantidad
        for (int i = 0; i < icons.Count; i++)
        {
            if (i < amount)
            {
                if (!icons[i].activeSelf)
                {
                    icons[i].SetActive(true);
                    // Animación de aparecer
                    icons[i].transform.localScale = Vector3.zero;
                    icons[i].transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                }
            }
            else
            {
                if (icons[i].activeSelf)
                {
                    // Animación de gastar: escala a 0 y desactiva al terminar
                    icons[i].transform.DOScale(0f, 0.2f).SetEase(Ease.InBack)
                        .OnComplete(() => icons[i].SetActive(false));
                }
            }
        }
    }
}
