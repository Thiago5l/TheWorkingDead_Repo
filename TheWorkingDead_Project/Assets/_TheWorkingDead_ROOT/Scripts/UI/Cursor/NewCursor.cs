using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewCursor : MonoBehaviour
{
    public RectTransform cursorImage;

    void Start()
    {
        Cursor.visible = false;
        cursorImage.GetComponent<Image>().raycastTarget = false;
    }

    void Update()
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorImage.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out position);

        position.x += cursorImage.sizeDelta.x / 2f;
        position.y -= cursorImage.sizeDelta.y / 2f;

        cursorImage.position = Input.mousePosition;
    }
}
