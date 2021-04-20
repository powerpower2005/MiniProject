using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [Header("Set in Inspector")]
    //This is an unusual but handy use of Vector2s.x hold a min value
    // and y a max value for a Random.Range() that will be called later

    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f; // powerUp exist
    public float fadeTime = 4f; // powerup fade

    [Header("Set Dynamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    private void Awake()
    {
        // Find cube
        cube = transform.Find("Cube").gameObject;

        // Find TextmMesh
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        //Set a random velocity
        Vector3 vel = Random.onUnitSphere; // Get Random XYZ velocity
        //this gives you a vector point that exist on the surface of the sphere with a radius of 1m around the origin

        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        //set the rotation 
        transform.rotation = Quaternion.identity;
        // identity means no rotation

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }

    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //fade out the Powerup over time
        //Given the default values, a PowerUp will exist for 10 seconds and then fade out over 4 seconds
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //for lifetime seconds, u will be <= 0

        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //use u to determine the alpha value of the cube and letter
        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }

    }

    public void SetType(WeaponType wt)
    {
        // Grab the WeaponDefinition from Main
        WeaponDefinition def = Main.GetWeaponDefinition(wt);

        //Set the color of cube child
        cubeRend.material.color = def.color;

        letter.text = def.letter;
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        //This function is called by the Hero class when a PowerUp is collected
        // We could tween into the target and shrink in size

        Destroy(this.gameObject);
    }


}
