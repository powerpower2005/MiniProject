using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;


    [Header("Set Dynamically")]
    public float camZ;// desired Z pos of the camera

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        //if there's only one line following an if, it doesn't need braces.
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;

            if(POI.tag == "Projectile")
            {
                // check if POI is sleeping.
                // sleeping means -> amount of movement is less than 0.02
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;

                    //go to next frame
                    return;

                }
            }

        }



        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
