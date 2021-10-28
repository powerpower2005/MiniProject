using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    [Header("Set in Inspector")]
    public float rotationPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    // This Non-public variable will not appear in the Insepctor
    Material mat;


    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //Read the current shield level from the Hero singleton
        int curLevel = Mathf.FloorToInt(Hero.S.shieldLevel);

        if(levelShown != curLevel)
        {
            levelShown = curLevel;
            //adjust shader offset
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
            
        }
        //Rotate shield on z Axis
        float rZ = -(rotationPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler( 0, 0, rZ);


    }
}
