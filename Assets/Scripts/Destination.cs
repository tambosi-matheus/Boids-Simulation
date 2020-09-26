using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{

    public float speed;
    public Camera cam;
    Vector2 mousePos;

    Boolean followMouse;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            followMouse = !followMouse;
        }    

        if(followMouse)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }

        else
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
        }
        
    }
}
