using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MinionSpawner : MonoBehaviour
{
    public float minionMoveSpeed;

    public GameObject minionPrefab;
    public float spawnInterval = 20.0f;
    public int minionsPerWave = 6;
    public int wavesUntilSuperMinion = 3;
    private int waveCount = 0;
    public Vector3 blueSpawnLocation = new Vector3(0, 1, -300);
    public Vector3 redSpawnLocation = new Vector3(0, 1, 300);

    public bool spawn = true;
    public bool isBerserker = false;
    public bool isSniper = false;
    public bool isTank = false;
    public float delayBetweenMinions;

    // Start is called before the first frame update
    void Start()
    {
        if (!isBerserker && !isSniper && !isTank)
        {
            StartCoroutine(SpawnWaves());

        }
    }

    void Update()
    {
        if (isBerserker)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SpawnRegularMinion();
            }
          
        }
        else if (isSniper) 
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                SpawnRegularMinion();
            }
        }
        else if (isTank)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                SpawnRegularMinion();
            }

        }
    }

    IEnumerator SpawnWaves()
    {
        while (true) // Infinite loop for continuous waves
        {
            waveCount++;

            if (waveCount % wavesUntilSuperMinion == 0)
            {
                for (int i = 0; i < minionsPerWave - 1; i++)
                {
                    SpawnRegularMinion();
                    yield return new WaitForSeconds(delayBetweenMinions);
                }

                SpawnRegularMinion();
                yield return new WaitForSeconds(delayBetweenMinions);

            }
            else
            {
                for (int i = 0; i < minionsPerWave; i++)
                {
                    SpawnRegularMinion();
                    yield return new WaitForSeconds(delayBetweenMinions);
                }

                yield return new WaitForSeconds(spawnInterval - delayBetweenMinions * minionsPerWave);
            }
        }
    }
    private void SpawnRegularMinion()
    {
        GameObject minion;

        // Blue Minions Instantiate
        {
            minion = Instantiate(minionPrefab, blueSpawnLocation, Quaternion.identity);
            minion.GetComponent<MinionAIScript>().destination = redSpawnLocation;
            minion.GetComponent<MinionAIScript>().isBlue = true;
            UnityEngine.AI.NavMeshAgent blueMinionAgent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
            blueMinionAgent.speed = minionMoveSpeed;
        }

        // Red Minions Instantiate

        minion = Instantiate(minionPrefab, redSpawnLocation, Quaternion.identity);
        minion.GetComponent<MinionAIScript>().destination = blueSpawnLocation;
        minion.GetComponent<MinionAIScript>().isBlue = false;
        UnityEngine.AI.NavMeshAgent redMinionAgent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        redMinionAgent.speed = minionMoveSpeed;

        // Set spawn back to true if necessary
        spawn = true;
    }
}



