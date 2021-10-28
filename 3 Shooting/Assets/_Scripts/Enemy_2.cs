using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{

    [Header("Set in Inspector")]
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;

    [Header("Set Dynamically")]
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

    void Start()
    {
        
        // point of left side of screen.
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        // point of right side of screen.
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //Possiblly swap sides
        if (Random.value > 0.5f)
        {
            // set the x of each point to its negative will move it to the other side of the screen.

            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;
    }

    public override void Move()
    {
        // u has a value between 0 & 1
        float u = (Time.time - birthTime) / lifeTime;

        // If u>l, then it has been longer than lifeTime since birtTime
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //Adjust u by adding a U curve based on a Sine wave
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

        //Interpolate the two linear points
        pos = (1 - u) * p0 + u * p1;
    }

}
