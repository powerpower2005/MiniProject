using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Part is another serializable data storage class just like WeaponDefinition
/// </summary>
/// 
[System.Serializable]
public class Part
{
    public string name; // name of this part
    public float health; // amount of health this part has
    public string[] protectedBy; // other parts that protect this

    // These two fields aer set automatically in Start()
    // Caching like this makes it faster and easier to find these later
    [HideInInspector]
    public GameObject go; // the gameobject of this part
    [HideInInspector]
    public Material mat; // material to show damage

}

public class Enemy_4 : Enemy
{
    public Part[] parts;


    private Vector3 p0, p1; // Two points to interpolate
    private float timeStart; // Birth time
    private float duration = 4; // duration of movement

    private void Start()
    {
        //There is already an initial position chosen by Main.SpawnEnemy()
        // so add it to points as the initial p0 and p1
        p0 = p1 = pos;

        InitMovement();

        //Cache GameObject and Material
        Transform t;
        foreach(Part prt in parts)
        {
            t = transform.Find(prt.name);
            if(t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }

    void InitMovement()
    {
        p0 = p1;

        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;

        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        //Reset time
        timeStart = Time.time;
    }

    public override void Move()
    {
        //This completely overrides Enemy.move() with a linear interpolation

        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2); // Apply ease out easing to u
        pos = (1 - u) * p0 + u * p1; // Linear interpolation
    }

    //These two functions find a Part in parts based on name or GameObject

    Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if(prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }

    Part FindPart(GameObject go)
    {
        foreach(Part prt in parts)
        {
            if(prt.go == go)
            {
                return (prt);
            }
        }
        return (null);
    }

    //These functions return true if the Part has been destroyed
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }

    bool Destroyed(Part prt)
    {
        if(prt == null)
        {
            return true;
        }

        return (prt.health <= 0);
    }

    //This changes the color of just one part to red instead of the while ship
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    //This will override the OnColliderEnter that is part of Enemy.cs
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }

                //Damaging this enemy
                GameObject goHit = collision.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if(prtHit == null)
                {
                    goHit = collision.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }

                //check whether this part is still protected
                if(prtHit.protectedBy != null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        //if one of the protecting parts hasn't been destroyed
                        if (!Destroyed(s))
                        {
                            //then don't damage this part yet
                            Destroy(other);
                            return;
                        }
                    }
                }

                //It's not protected, make a damage
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;

                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    //Instead of destroying this enemy, disable the damaged part
                    prtHit.go.SetActive(false);
                }

                //check to see if the whole ship is destroyed
                bool allDestroyed = true;
                foreach(Part prt in parts)
                {
                    if (!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (allDestroyed)
                {
                    Main.S.ShipDestroyed(this);

                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }

}
