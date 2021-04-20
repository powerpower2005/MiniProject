using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{

    //This scripts is reusable
    //this only works for and orthographic Main Camera at [0,0,0]
    //this can make a boundary based on camera size.
    //this can check whether gameobject is on screen or not.

    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true; // this allow object to go through boundary when it's false

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;



    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;



    

    void Awake()
    {
        // get a value by multiplying aspect
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }



    //This Update happens after all the physics are caculated.
    // it is called after update()
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offDown = offUp = false;

        if(pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            isOnScreen = false;
            offRight = true;
        }
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            isOnScreen = false;
            offLeft = true;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            isOnScreen = false;
            offUp = true;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            isOnScreen = false;
            offDown = true;
        }

        // the object have to be in the boundary
        isOnScreen = !(offRight || offLeft || offDown || offUp);
        if(keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offDown = offUp = false;
        }

    }


    //Draw the bounds in the Scene
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
