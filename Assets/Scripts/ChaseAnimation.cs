using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseAnimation : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    public Transform target;
    public float animationSmoothTime = .1f;
    public float maxSpeed = 6f;
    public float distanceToStartRunning = 10f;
    public float distanceToStopRunning = 5f;

    private float health = 100;
    private bool isDead;

    float currentSpeed;
    float currentAnimationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        bool isRunning = distanceToTarget < distanceToStartRunning && distanceToTarget > distanceToStopRunning;
        currentSpeed = Mathf.Lerp(currentSpeed, isRunning ? maxSpeed : 0f, Time.deltaTime * 10f);

        // Update the animator parameters
        currentAnimationSpeed = Mathf.Lerp(currentAnimationSpeed, isRunning ? 1f : 0f, Time.deltaTime / animationSmoothTime);
        animator.SetFloat("InputMagnitude", currentAnimationSpeed);

        // Set the NavMeshAgent speed
        agent.speed = currentSpeed;

        if (distanceToTarget < distanceToStopRunning)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.destination = target.position;
        }

        if(health <= 0 && !isDead)
        {
            isDead = true;
            animator.SetBool("IsDead", true);
        }
        if(isDead)
        {
            /*
            isRunning = false;
            agent.isStopped = true;
            */
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage()
    {
        health -= 20;
        animator.SetTrigger("IsAttacked");
    }
}

