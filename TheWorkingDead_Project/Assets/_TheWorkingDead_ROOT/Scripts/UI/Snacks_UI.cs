using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class Snacks_UI : MonoBehaviour
{
    [SerializeField] GameObject SnackPrefab;
    [SerializeField] Transform gridParent;

    public List<GameObject> icons = new List<GameObject>();

    private void Awake()
    {
        if (gridParent == null)
            gridParent = transform;
    }

    public void SetSnacks(int amount)
    {
        while (icons.Count < amount)
        {
            GameObject icon = Instantiate(SnackPrefab, gridParent);
            icon.transform.localScale = Vector3.zero;

            ResetVisual(icon);
            icons.Add(icon);

            icon.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        for (int i = 0; i < icons.Count; i++)
        {
            GameObject icon = icons[i];
            icon.transform.DOKill();

            if (i < amount)
            {
                icon.SetActive(true);
                ResetVisual(icon);

                icon.transform.localScale = Vector3.zero;
                icon.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            }
            else
            {
                if (icon.activeSelf)
                {
                    icon.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack)
                        .OnComplete(() => icon.SetActive(false));
                }
            }
        }
    }

    void ResetVisual(GameObject icon)
    {
        Image img = icon.GetComponent<Image>();
        if (img != null)
        {
            img.fillAmount = 1f;
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }
    }
}
