using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //
    private Transform target;
    public float attackRange = 1f;
    public float attackRate = 1f;
    public float nextAttackTime = 0f;
    private Animator animator;
    private string[] attackAnimations = { "PunchRight", "PunchLeft", "Kick", "Kick2" };
    //

    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    void Update()
    {
        CheckAttack();
    }

    void CheckAttack()
    {
        if (Input.GetMouseButtonDown(0) && target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
            {
                int randomIndex = Random.Range(0, attackAnimations.Length);
                animator.SetTrigger(attackAnimations[randomIndex]);
                target.GetComponent<ChaseAnimation>().TakeDamage();
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }
    }
}
