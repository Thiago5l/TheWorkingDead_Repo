using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CursorSprite : MonoBehaviour
{
    public Texture2D cursorT;
    private Texture2D cursorTSave;
    public Texture2D cursorClick;
    private Texture2D cursorClickSave;
    
    public Texture2D cursorRotu;

    private Vector2 cursorHotSpot;

    void Start()
    {
        //no tocar esto que hace que el cursor si funcione
        Cursor.visible = true;
        //

        cursorHotSpot = new Vector2(cursorT.width / 2, cursorT.height / 2);
        Cursor.SetCursor(cursorT, cursorHotSpot, CursorMode.Auto);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorClick, cursorHotSpot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorT, cursorHotSpot, CursorMode.Auto);
        }
    }
}
