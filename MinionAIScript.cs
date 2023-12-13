using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAIScript : MonoBehaviour
{
    public Vector3 destination;
    public Vector3 finalDestination;

    public Material blueMinionMat;
    public Material redMinionMat;
    public GameObject minimapIcon;
    
    public bool isBlue;

    public GameObject target;
    public bool hasTarget = false;
    public bool passedHalfway = false;
    public bool isBerserker = false;
    public bool isSniper = false;
    public bool isTank = false;

    public float health = 100;
    public float attackTimer = 2;
    public float damage = 20;
   

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        if (isBlue)
        {
            this.gameObject.GetComponent<Renderer>().material = blueMinionMat;
            this.gameObject.layer = 9;
            this.gameObject.tag = "BlueMinion";
            minimapIcon.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material = redMinionMat;
            this.gameObject.layer = 10;
            this.gameObject.tag = "RedMinion";
            minimapIcon.GetComponent<SpriteRenderer>().color = Color.red;
        }

        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget && target != null)
        {
            agent.SetDestination(target.transform.position);
            attackTimer = attackTimer - Time.deltaTime;
            if (attackTimer <= 0)
            {
                attackTimer = 2;
                if (target.TryGetComponent(out MinionAIScript minionTargetScript))
                {
                    minionTargetScript.health -= 20;
                    if (isBerserker)
                    {
                        if (health < 20)
                        {
                            damage = 40;
                        }
                    }
                    if (isSniper)
                    {
                        damage = 0;
                        if (minionTargetScript.isBerserker)
                        {
                            damage = 50;
                        }
                    }
                    if (isTank && minionTargetScript.isSniper)
                    {
                        damage = 0;
                    }
                }
                if (target.TryGetComponent(out PlayerScript1 playerTargetScript))
                {
                    playerTargetScript.health -= 20;
                    if (isBerserker)
                    {
                        if (health < 20)
                        {
                            damage = 40;
                        }
                    }
                }
                if (target.TryGetComponent(out TowerAIScript TowerScript))
                {
                    //TowerAIScript.health -= 20;
                    
                }

                //Debug.Log("Health" + health);
            }

        }


        if (target == null)
        {

            hasTarget = false;

            if (passedHalfway)
            {
                agent.SetDestination(finalDestination);
            }
            else
            {
                agent.SetDestination(destination);
            }

        }


    

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

    }

}
