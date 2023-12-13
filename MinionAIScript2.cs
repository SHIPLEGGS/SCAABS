using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAIScript2 : MonoBehaviour
{
    public List<GameObject> targetList = new List<GameObject>();

    private Transform currentTarget;

    public MinionAIScript minionScript;
    public NavMeshAgent minionAgent;
    public bool isBlue;

    public string blueMinionTag = "BlueMinion";
    public string redMinionTag = "RedMinion";
    public string blueTurretTag = "BlueTurret";
    public string redTurretTag = "RedTurret";
    public float stopDistance = 2.0f;
    public float aggroRange = 5.0f;
    public float targetSwitchInterval = 2.0f;

    public float timeSinceLastTargetSwitch = 0.0f;



    public GameObject closestTarget;

    // Start is called before the first frame update
    void Start()
    {
        minionScript = this.GetComponentInParent<MinionAIScript>();
        minionAgent = this.GetComponentInParent<NavMeshAgent>();
        isBlue = minionScript.isBlue;
        FindAndSetTarget();

    }

    private void Update()
    {
        timeSinceLastTargetSwitch += Time.deltaTime;

        if (timeSinceLastTargetSwitch >= targetSwitchInterval)
        {
            CheckAndSwitchTargets();
            timeSinceLastTargetSwitch = 0.0f;
        }

        if (currentTarget != null)
        {
            Vector3 directionToTarget = currentTarget.position - transform.position;
            Vector3 stoppingPosition = currentTarget.position - directionToTarget.normalized * stopDistance;
            minionAgent.SetDestination(stoppingPosition);

        }
        if (targetList.Count > 0 && minionScript.hasTarget == false)
        {
            for (var i = targetList.Count - 1; i > -1; i--)
            {
                if (targetList[i] != null)
                {
                    float closestDistance = Mathf.Infinity;
                    float distance = Vector3.Distance(gameObject.transform.position, targetList[i].transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = targetList[i];
                    }
                }
                else
                {
                    targetList.RemoveAt(i);
                }


            }
            minionScript.target = closestTarget;
            minionScript.hasTarget = true;

        }


    }

    private void CheckAndSwitchTargets()
    {
        GameObject[] enemyRedMinions = GameObject.FindGameObjectsWithTag(redMinionTag);
        Transform closestEnemyRedMinion = GetClosestObjectInRadius(enemyRedMinions, aggroRange);

        if (closestEnemyRedMinion != null)
        {
            currentTarget = closestEnemyRedMinion;
        }
        else
        {
            GameObject[] enemyRedTurrets = GameObject.FindGameObjectsWithTag(redTurretTag);
            currentTarget = GetClosestObject(enemyRedTurrets);
        }


        GameObject[] enemyBlueMinions = GameObject.FindGameObjectsWithTag(blueMinionTag);
        Transform closestEnemyBlueMinion = GetClosestObjectInRadius(enemyBlueMinions, aggroRange);

        if (closestEnemyBlueMinion != null)
        {
            currentTarget = closestEnemyBlueMinion;
        }
        else
        {
            GameObject[] enemyBlueTurrets = GameObject.FindGameObjectsWithTag(blueTurretTag);
            currentTarget = GetClosestObject(enemyBlueTurrets);
        }
    }


    private Transform GetClosestObject(GameObject[] objects)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj.transform;
            }
        }

        return closestObject;

    }

    private Transform GetClosestObjectInRadius(GameObject[] objects, float radius)
    {

        float closestDistance = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);

            if (distance < closestDistance && distance <= radius)
            {
                closestDistance = distance;
                closestObject = obj.transform;
            }
        }

        return closestObject;
    }

    private void FindAndSetTarget()
    {

        GameObject[] enemyRedMinions = GameObject.FindGameObjectsWithTag(redMinionTag);
        Transform closestEnemyRedMinion = GetClosestObjectInRadius(enemyRedMinions, aggroRange);

        if (closestEnemyRedMinion != null)
        {
            currentTarget = closestEnemyRedMinion;
        }
        else
        {
            GameObject[] enemyRedTurrets = GameObject.FindGameObjectsWithTag(redTurretTag);
            currentTarget = GetClosestObject(enemyRedTurrets);

        }

        GameObject[] enemyBlueMinions = GameObject.FindGameObjectsWithTag(blueMinionTag);
        Transform closestEnemyBlueMinion = GetClosestObjectInRadius(enemyBlueMinions, aggroRange);

        if (closestEnemyBlueMinion != null)
        {
            currentTarget = closestEnemyBlueMinion;
        }
        else
        {
            GameObject[] enemyBlueTurrets = GameObject.FindGameObjectsWithTag(blueTurretTag);
            currentTarget = GetClosestObject(enemyBlueTurrets);

        }

    }

    public void OnTriggerEnter(Collider collider)
    {
        if (isBlue)
        {
            if (!targetList.Contains(collider.gameObject))
            {
                if (collider.gameObject.layer == 12 || collider.gameObject.layer == 10)
                {
                    targetList.Add(collider.gameObject);
                }

            }
        }
        else
        {
            if (!targetList.Contains(collider.gameObject))
            {
                if (collider.gameObject.layer == 11 || collider.gameObject.layer == 9)
                {
                    targetList.Add(collider.gameObject);
                }

            }
        }


    }

    public void OnTriggerExit(Collider collider)
    {
        if (targetList.Contains(collider.gameObject))
        {
            targetList.Remove(collider.gameObject);
        }
    }
}
