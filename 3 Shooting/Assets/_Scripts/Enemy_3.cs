using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector")]
    public float lifeTime = 5;

    [Header("Set Dynamically")]
    public Vector3[] points;
    public float birthTime;

    void Start()
    {
        //initialize
        points = new Vector3[3];

        //start position by Main.SpawnEnemy()
        points[0] = pos;

        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v;
        // random position v
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);

        points[1] = v;

        // other random position
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);

        points[2] = v;

        //set bithTime

        birthTime = Time.time;


    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //Interpolate 3 points curve
        Vector3 p01, p02;
        p01 = (1 - u) * points[0] + u * points[1];
        p02 = (1 - u) * points[1] + u * points[2];

        pos = (1-u) * p01 + u * p02;


    }
}
