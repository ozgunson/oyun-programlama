using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTwo : MonoBehaviour
{
    public GameObject weapon;
    public bool canAttack = true;
    public float attackCooldown = 1f;
    private string[] attackAnimations = { "AttackHorizontal","AttackDownward" };
    public bool isAttacking = false;

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(canAttack)
            {
                HitAttack();
            }
        }
    }

    public void HitAttack()
    {
        isAttacking = true;
        canAttack = false;
        int randomIndex = Random.Range(0, attackAnimations.Length);
        animator.SetTrigger(attackAnimations[randomIndex]);
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
}
