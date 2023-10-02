using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> entityPool;

    // a pulse means when the timer finishes to spawn an enemy
    private float amountOfEnemiesPerPulse; // higher = more difficult
    private float spawnerDifficulty; // higher = more difficult
    private float timeBetweenPulses; // higher = easier

    // timer
    private float pulseTimer;

    // activation bool
    private bool activated;

    public float AmountOfEnemiesPerPulse
    {
        get { return amountOfEnemiesPerPulse; }
        set { amountOfEnemiesPerPulse = value; }
    }

    public float SpawnerDifficulty
    {
        get { return spawnerDifficulty; }
        set { spawnerDifficulty = value; }
    }

    public float TimeBetweenPulses
    {
        get { return timeBetweenPulses; }
        set { timeBetweenPulses = value; }
    }

    public bool Activated
    {
        get { return activated; }
        set { activated = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (spawnerDifficulty < 1)
        {
            spawnerDifficulty = 1;
        }

        if (spawnerDifficulty > 20)
        {
            spawnerDifficulty = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (activated)
        {
            pulseTimer += Time.deltaTime;
        }
        

        if (pulseTimer > timeBetweenPulses)
        {
            SpawnPulse();
            pulseTimer = 0;
        }
    }

    private void SpawnPulse()
    {
        // make some deviations in spawn position
        int xRand = Random.Range(-3, 4);
        int zRand = Random.Range(-3, 4);

        Vector3 newRandomPos;

        // check if we wanna add or subtrack the above ints on the vector
        int c = Random.Range(0, 1);

        if (c == 0)
        {
            newRandomPos = new Vector3(transform.position.x + xRand, transform.position.y, transform.position.z + zRand);
        }
        else
        {
            newRandomPos = new Vector3(transform.position.x - xRand, transform.position.y, transform.position.z - zRand);
        }
        // check if we wanna spawn
        int shouldSpawn = Random.Range(0, 20);
        if (shouldSpawn <= spawnerDifficulty)
        {
            for (int i = 0; i < amountOfEnemiesPerPulse; i++)
            {
                // get random entity from pool
                int randomEntity = Random.Range(0, entityPool.Count);


                GameObject temp = Instantiate(entityPool[randomEntity], newRandomPos, transform.rotation);
            }
        }
        else
        {
            return;
        }
    }
}
