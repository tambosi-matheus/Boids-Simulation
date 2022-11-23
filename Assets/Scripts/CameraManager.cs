using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InGameManager;

public class CameraManager : MonoBehaviour
{
    private static Vector2 screenMinMaxX, screenMinMaxY;
    private int zoom = 50;

    private void Awake()
    {
        screenMinMaxX = new Vector2(Screen.width * 0.05f, Screen.width * 0.95f);
        screenMinMaxY = new Vector2(Screen.height * 0.05f, Screen.height * 0.95f);
    }

    // Update is called once per frame
    void Update()
    {
        // Movimentation
        var mousePos = Input.mousePosition;
        var cameraPos = Camera.main.transform.position;

        var vel = Vector3.zero;
        if (mousePos.x < screenMinMaxX.x)
            vel.x = mousePos.x - screenMinMaxX.x;
        else if(mousePos.x > screenMinMaxX.y)
            vel.x = mousePos.x - screenMinMaxX.y;
        if (mousePos.y < screenMinMaxY.x)
            vel.y = mousePos.y - screenMinMaxY.x;
        else if (mousePos.y > screenMinMaxY.y)
            vel.y = mousePos.y - screenMinMaxY.y;

        vel.x /= screenMinMaxX.x;
        vel.y /= screenMinMaxY.x;
        
        cameraPos += vel * zoom / 200;
        


        // Zoom
        var deltaZoom = (int)Input.mouseScrollDelta.y * 10;
        if (deltaZoom != 0 &&
            (zoom - deltaZoom <= 200 && zoom - deltaZoom >= 1))
        {
            zoom -= deltaZoom;
            Camera.main.orthographicSize = zoom;
            if(deltaZoom > 0)
            {
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
                cameraPos = Vector3.Lerp(cameraPos, mouseWorldPos, 0.25f);
            }
        }

        cameraPos.x = Mathf.Clamp(cameraPos.x, Camera.main.aspect * zoom - gameSize.x, - Camera.main.aspect * zoom + gameSize.x);
        cameraPos.y = Mathf.Clamp(cameraPos.y, zoom - gameSize.y, - zoom + gameSize.y);

        Camera.main.transform.position = cameraPos;
    }    
}
