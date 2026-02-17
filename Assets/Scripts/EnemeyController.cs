using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemeyController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    private NavMeshAgent agent;
    private Vector3 randomDirection;
    private float changeDirectionTimer;
    private float minChange = 3f;
    private float maxChange = 8f;
    public Animator enemyAnimator;
    private bool enemyWalk = true;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= 40f) 
        {
            if (enemyWalk == true) {
                agent.SetDestination(player.position);
                enemyAnimator.SetBool("Walk", true);

            }
            
        }
        else
        {
            changeDirectionTimer -= Time.deltaTime;
            if (changeDirectionTimer <= 0f)
            {
                ChangeDirection();
            }
            if (enemyWalk == true)
            {
                agent.SetDestination(transform.position + randomDirection);
                enemyAnimator.SetBool("Walk", true);
            }
                
        }

    }
    void ChangeDirection()
    {
        randomDirection = Random.insideUnitSphere * 10f;
        changeDirectionTimer = Random.Range(minChange, maxChange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyAnimator.SetTrigger("Attack");
            enemyWalk = false;
            
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            enemyAnimator.SetBool("Walk", true);
            enemyWalk = true;

        }
    }
}
