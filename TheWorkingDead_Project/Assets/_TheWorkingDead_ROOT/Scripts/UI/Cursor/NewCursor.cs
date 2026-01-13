using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewCursor : MonoBehaviour
{
    public RectTransform cursorImage;
    public Sprite[] cursorSprites;

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

        if (Input.GetMouseButtonDown(0))
        {
            cursorImage.GetComponent<Image>().sprite = cursorSprites[1];
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursorImage.GetComponent<Image>().sprite = cursorSprites[0];
        }
    }
}
