using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//inherited by Enemy
public class Enemy_1 : Enemy
{
    [Header("Set in Inspector")]
    //# seconds for a full sine wave.
    public float waveFrequency = 2;

    //wave width
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0; // initial value on x Axis
    private float birthTime;


    private void Start()
    {
        //pos is inherited by Enemy class

        x0 = pos.x;

        birthTime = Time.time;
    }

    //want to move differently against EnemyClass
    public override void Move()
    {
        // Because pos is a property, can't directly set pos.x
        //

        Vector3 tempPos = pos;

        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);

        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        //Rotate
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        //base.move() -> still move down in y axis
        base.Move();

    }
}
