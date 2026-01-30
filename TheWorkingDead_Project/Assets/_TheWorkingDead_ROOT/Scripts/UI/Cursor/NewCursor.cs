using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewCursor : MonoBehaviour
{
    public RectTransform cursorImage;
    public Sprite[] cursorSprites;
    public Sprite[] cursorSpritesSave;
    public Image cursorDef;
    void Start()
    {
        
        Cursor.visible = false;
        cursorImage.GetComponent<Image>().raycastTarget = false;
        cursorSpritesSave = (Sprite[])cursorSprites.Clone();

        //cursorSpritesSave = new Sprite[cursorSprites.Length];
        //cursorSprites.CopyTo(cursorSpritesSave, 0);
        //for (int i = 0; i < cursorSprites.Length; i++)
        //{
        //    cursorSpritesSave[i] = cursorSprites[i];

        //}
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


            cursorDef.sprite = newCursor;
            for (int i = 0; i < cursorSprites.Length; i++)
            {
                cursorSprites[i] = newCursor;
            }
        }
    }

    public void CursorToNormalState()
    {
        cursorSpritesSave.CopyTo(cursorSprites, 0);
        //for(int i = 0; i < cursorSprites.Length; i++)
        //{
        //    cursorSprites[i] = cursorSpritesSave[i];
        //}
        cursorDef.sprite = cursorSprites[0];
    }
}
