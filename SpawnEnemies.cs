using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    // Game Objects/Prefabs
    public GameObject enemyPrefab;
    public GameObject EasyEnemy;
    public GameObject MediumEnemy;
    public GameObject HardEnemy;
    public GameObject BossEnemy;
    private Dictionary<EnemyLevels, GameObject> Enemies = new Dictionary<EnemyLevels, GameObject>(4);

    // Enemies, how many have been created and how many are to be created
    public int totalEnemy =10;
    private int numEnemy =0;
    private int spawnedEnemy = 0;

    // Enums:
        // Spawn Types
    public enum SpawnTypes
    {
        Normal, Once, Wave, TimedWave
    }

        // Enemy Levels
    public enum EnemyLevels
    {
        Easy, Medium, Hard, Boss
    }

    // Enemy level used
    public EnemyLevels enemyLevel = EnemyLevels.Easy;


    // The ID of the spawner
    private int SpawnID;

    //----------------------------------------------
    // Different Spawn states
    private bool waveSpawn = false;
    public bool Spawn = true;
    public SpawnTypes spawnType = SpawnTypes.Normal;
    // Timed wave controls
    public float waveTimer = 30.0f;
    private float timeTillWave = 0.0f;
    // Wave Controls
    public int totalWaves = 5;
    private int numWaves = 0;
    //----------------------------------------------


    void Start()
    {
        // Sets a random number for the id of the spawner
        SpawnID = Random.Range(1, 500);
        Enemies.Add(EnemyLevels.Easy, EasyEnemy);
        Enemies.Add(EnemyLevels.Medium, MediumEnemy);
        Enemies.Add(EnemyLevels.Hard, HardEnemy);
        Enemies.Add(EnemyLevels.Boss, BossEnemy);
        // SendMessage("StartSpawning");
    }

    public Color gizmoColor = Color.red;
    // Draws a Cube to show where the spawn point is
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }


    public void StartSpawning()
    {
        if(Spawn)
        {
            // Spawns enemies everytime one dies  *
            if(spawnType == SpawnTypes.Normal)
            {
                // Checks to see if the number of spawned enemies is less than the max num of enemies
                if(numEnemy < totalEnemy)
                {
                    // spawns an enemy
                    SpawnEnemy();
                }
            }
            // Spawns enemies only once   **
            else if(spawnType == SpawnTypes.Once)
            {
                // checks to see if the overall spawned num of enemies is more or equal to the total to be spawned
                if(spawnedEnemy >= totalEnemy)
                {
                    // sets the spawner to false
                    Spawn = false;
                }
                else
                {
                    // spawns an enemy
                    SpawnEnemy();
                }
            }
            // Spawns enemies in waves, so once all are dead, spawns more   ***
            else if (spawnType == SpawnTypes.Wave)
            {
                if(numWaves < totalWaves +1)
                {
                    if(waveSpawn)
                    {
                        // spawns an enemy
                        SpawnEnemy();
                    }
                    if(numEnemy == 0)
                    {
                        // enables the wave spawner
                        waveSpawn = true;
                        // increase the number of waves
                        numWaves++;
                    }
                }
            }
            // Spawns enemies in the waves but based on time
            else if(spawnType == SpawnTypes.TimedWave)
            {
                // cheks if the number of waves is bigger than the total waves
                if(numWaves <= totalWaves)
                {
                    // Increases the timer to allow the timed waves to work
                    timeTillWave += Time.deltaTime;
                    if(waveSpawn)
                    {
                        // spawns an enemy
                        SpawnEnemy();
                    }
                    // checks if the time is equal to the time required for a new wave
                    if(timeTillWave >= waveTimer)
                    {
                        // enables the wave spawner
                        waveSpawn = true;
                        // sets the time back to zero
                        timeTillWave = 0.0f;
                        // increases the number of waves
                        numWaves++;
                        // A hack to get it to spawn the same number of enemies regardless of how many have been killed
                        numEnemy = 0;
                    }
                    if(numEnemy >= totalEnemy)
                    {
                        // disables the wave spawner
                        waveSpawn = false;
                    }
                }
                else
                {
                    Spawn = false;
                }
            }
        }

        if(Input.GetKeyDown("space"))
        {
            SendMessage("removeMe");
        }

        if (Input.GetKeyDown("b"))
        {
            SendMessage("startSpawning");
        }
    }

    // spawns an enemy based on the enemy level that you selected
    private void SpawnEnemy()
    {
        Debug.Log("Spawn Enemy Was Called");
        GameObject Enemy = (GameObject)Instantiate(Enemies[enemyLevel], gameObject.transform.position, Quaternion.identity);
        Enemy.SendMessage("setName", SpawnID);
        // Increases the total number of enemies spawned and the number of spawned enemies
        numEnemy++;
        spawnedEnemy++;
        Debug.Log(spawnedEnemy);
    }

    // Call this function from the enemy when it "dies" to remove an enemy count
    public void killEenmy(int sID)
    {
        // if the enemy's spawnID is equal to this spawnersID then remove an enemy count
        if(SpawnID == sID)
        {
            numEnemy--;
        }
    }

    // Game started, start spawning
    public void StartGame()
    {
        Invoke("StartSpawning", 10);
    }

    // enable the spawner based on the spawnerID
    public void enableSpawner(int sID)
    {
        if(SpawnID == sID)
        {
            Spawn = true;
        }
    }

    // disable the spawner based on spawnerID
    public void disablesSpawner(int sID)
    {
        if(SpawnID == sID)
        {
            Spawn = false;
        }
    }

    // returns the time till the next wave, for an interface, ect.
    public float TimeTillWave
    {
        get
        {
            return timeTillWave;
        }
    }

    // Enable the spawner, useful for trigger events because you don't know the spawner's ID
    public void enabletrigger()
    {
        Spawn = true;
    }
}
