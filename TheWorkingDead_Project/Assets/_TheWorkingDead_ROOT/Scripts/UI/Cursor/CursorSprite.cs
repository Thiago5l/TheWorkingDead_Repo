using UnityEngine;
using UnityEngine.Rendering;

public class CursorSprite : MonoBehaviour
{
    public Texture2D cursorT;

    private Vector2 cursorHotSpot;

    private void Start()
    {
        Cursor.visible = true;

        cursorHotSpot = new Vector2(cursorT.width / 2, cursorT.height / 2);
        Cursor.SetCursor(cursorT, cursorHotSpot, CursorMode.Auto);
    }
}
