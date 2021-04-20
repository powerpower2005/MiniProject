using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    static public Hero S; // Singleton

    [Header("Set in Inspector")]
    public float speed = 30;
    //move naturally
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;



    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;


    // this holds a reference to the last triggering GameObject
    private GameObject lastTriggerGo = null;

    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    // Create a WeaponFireDelegate field named fireDelegate
    public WeaponFireDelegate fireDelegate;


    //property
    public float shieldLevel
    {
        get
        {
            return _shieldLevel;
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            // if the shield is going to be set to less than zero

            if(value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }

    }

    void Start()
    {
        if(S == null)
        {
            S = this; // Set the singleton

        }
        else
        {
            //Singleton have to be one.
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
        }

        //Reset the weapons to start Hero with 1 blaster
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);


    }


    void Update()
    {
        //movement
        //Raw -> have only 3 values that -1,0,1
        //reason not to use Raw -> use it to rotate
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;


        //Rotation 
        //It gives player reality
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * pitchMult, 0);

        //fire projectile -> delegate
 //       if (Input.GetKeyDown(KeyCode.Space))
 //       {
 //           TempFire();
 //       }

        //Use the fireDelegate to fire Weapons
        if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        //print root parent not their children.
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Triggered :" + go.name);

        //Make sure it's not the same triggering go as last time.
        // avoid to repeat that
        if(go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;

        if(go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else if(go.tag == "PowerUp")
        {
            //If the shield was triggered by a PowerUp
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy :" + go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;

            default:
                //If it is the same type
                if (pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                else // If this is a different weapon type
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
         }
        pu.AbsorbedBy(this.gameObject);

    }

    Weapon GetEmptyWeaponSlot()
    {
        for(int i = 0; i<weapons.Length; i++)
        {
            if(weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }

    void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
}
