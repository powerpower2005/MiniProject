using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S; // Singleton
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    public WeaponDefinition[] weaponDefinitions;

    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster,WeaponType.spread,WeaponType.shield
    };



    private BoundsCheck bndCheck;

    public void ShipDestroyed(Enemy e)
    {
        //Potentially generate a PowerUp
        if(Random.value <= e.powerUpDropChance)
        {
            //choose which PowerUp to pick

            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            //spawn powerup
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();

            //set it to the proper WeaponType
            pu.SetType(puType);

            //set it to the position of the destroyed enemy
            pu.transform.position = e.transform.position;
        }
    }

    private void Awake()
    {
        S = this;

        bndCheck = GetComponent<BoundsCheck>();

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        // A generic Dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

    }

    public void SpawnEnemy()
    {
        //enemy instance
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //enemy random position
        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //enemy default position
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        //execute again
        Invoke("SpawnEnemy", 1 / enemySpawnPerSecond);

    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Static function that gets a WeaponDefinition from the WEAP_DICT static protected field of the Main class
    /// </summary>
    /// <returns>
    /// The WeaponDifinitions or, if there is no WeaponDefinition with the WeaponType passed in,
    /// returns a new WeaponDefinition with a WeaponType of none.
    /// </returns>
    /// <param name="wt">
    /// The WeaponType of the desired WeaponDefinitions
    /// </param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //check to make sure that the key exists in the Dictionary
        // Attempting to retrieve a key that didn't exist, would throu an error.
        // so the following if statement is important
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        //This returns a new WeaponDefinition with a type of WeaponType.none
        //which means it has failed to find the right WeaponDefinition
        return (new WeaponDefinition());

    }

}
