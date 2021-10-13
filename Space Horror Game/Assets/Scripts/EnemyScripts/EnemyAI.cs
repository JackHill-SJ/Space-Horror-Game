using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    [SerializeField]
    private LayerMask whatIsGround;

    bool PlayerInSight;

    //Patroling
    private Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [SerializeField]
    private float hearingRange, sightRange, attackRange;
    private bool playerInHearingRange, playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        LineOfSightRaycast();

        //Check for sight/distance from the player
        if (Vector3.Distance(transform.position,player.position) <= hearingRange)
        {
            playerInHearingRange = true;
        }
        else
        {
            playerInHearingRange = false;
        }

        if (PlayerInSight)
        {
            playerInSightRange = true;
        }

        //Check if distance to the player is close enough to attack
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            playerInAttackRange = true;
        }
        else
        {
            playerInAttackRange = false;
        }

        if (!playerInHearingRange && !playerInAttackRange) Patroling();
        if (playerInHearingRange || playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInHearingRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }
    private void ChasePlayer()
    {
        if (PlayerInSight)
        {
            agent.SetDestination(player.position);
        }
        else if(!PlayerInSight && playerInHearingRange)
        {
            Vector3 lastPlayerLocation = player.position;
            transform.LookAt(player);
            agent.SetDestination(lastPlayerLocation);
        }
    }
    private void AttackPlayer()
    {
        //Stops enemy from moving
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        //Enemy does animation to attack. game ends.
    }

    private void LineOfSightRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * sightRange, Color.red);
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, sightRange))
        {
            if (raycastHit.collider.name == "Player")
            {
                PlayerInSight = true;
            }
            else
            {
                PlayerInSight = false;
            }
        }
    }
}
