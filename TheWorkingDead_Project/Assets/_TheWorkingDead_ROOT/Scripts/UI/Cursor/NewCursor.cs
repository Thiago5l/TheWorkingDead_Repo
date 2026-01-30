using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewCursor : MonoBehaviour
{
    public RectTransform cursorImage;
    public Sprite[] cursorSprites;
    public Sprite[] cursorSpritesSave;
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

    public void CambiarCursor(Sprite newCursor)
    {
        if (cursorSprites[0] != newCursor)
        {
            cursorSpritesSave = cursorSprites;


            for (int i = 0; i < cursorSprites.Length; i++)
            {
                cursorSprites[i] = newCursor;
            }
        }
    }

    public void CursorToNormalState()
    {
        cursorSprites = cursorSpritesSave;
    }
}
