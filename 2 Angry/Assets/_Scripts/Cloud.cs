using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    [Header("Set in Inspector")]
    public GameObject cloudSphere;

    //Random Generator
    //#1 Number
    public int numSpheresMin = 6;
    public int numSpheresMax = 10;
    //#2 Distance between clouds
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);
    //#3 scales in each dimension
    public Vector2 spheresScaleRangeX = new Vector2(4, 8);
    public Vector2 spheresScaleRangeY = new Vector2(3, 4);
    public Vector2 spheresScaleRangeZ = new Vector2(2, 4);
    //#4 Thickness of Cloud
    public float scaleYMin = 2f;

    private List<GameObject> spheres;





    void Start()
    {
        spheres = new List<GameObject>();


        //Generate
        int num = Random.Range(numSpheresMin, numSpheresMax);
        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(this.transform);


            //Random Position
            Vector3 offset = Random.insideUnitSphere;// X^2 + Y^2  + Z^2 <= 1 bigger X is, smaller Y is.
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spTrans.localPosition = offset;

            //Random Scale
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(spheresScaleRangeX.x, spheresScaleRangeX.y);
            scale.y = Random.Range(spheresScaleRangeY.x, spheresScaleRangeY.y);
            scale.z = Random.Range(spheresScaleRangeZ.x, spheresScaleRangeZ.y);


            //Adjust Y Scale(thickness) by x distnace from core
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            spTrans.localScale = scale;
        }


    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
        */
    }


    //ReLocation of clouds
   void Restart()
    {
        foreach(GameObject sp in spheres)
        {
            Destroy(sp);
        }

        Start();
    }
  
}
