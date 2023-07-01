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
    public float destroyDelay = 5f;
    public float timer = 0f;

    private float health = 100;
    private bool isDead;
    public GameObject obj;

    float currentSpeed;
    float currentAnimationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        obj = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        bool isRunning = distanceToTarget < distanceToStartRunning && distanceToTarget > distanceToStopRunning && isDead == false;
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
            animator.SetTrigger("IsDead");
            
        }
        if(isDead)
        {
            /*
            isRunning = false;
            agent.isStopped = true;
            */
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        timer += Time.deltaTime;
        if (timer >= destroyDelay)
        {
            Destroy(obj);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage()
    {
        health -= 50;
        animator.SetTrigger("IsAttacked");
    }
}

