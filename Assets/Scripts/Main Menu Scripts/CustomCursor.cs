using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    
    public Texture2D CursorTexture;                     //Assign the cursor texture here
    public Vector2 hotSpot = Vector2.zero;              //Where on the texture the "click" happens
    public CursorMode cursorMode = CursorMode.Auto;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //make the click in the center of the texture
        hotSpot = new Vector2(CursorTexture.width/2, CursorTexture.height/2);
        
        //change the cursor when the scene starts
        Cursor.SetCursor(CursorTexture, hotSpot, cursorMode);

    }

}
