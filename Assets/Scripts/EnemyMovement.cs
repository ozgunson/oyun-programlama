using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float timeBetweenAttacks = 1f;

    private Transform playerTransform;
    private Animator animator;
    private bool isAttacking;
    private float attackTimer;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Ch18_nonPBR").transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Düþmanýn oyuncuyu takip etmesi
        if (distanceToPlayer <= stoppingDistance)
        {
            // Düþman oyuncuya saldýrmak üzereyse, saldýrýya geç
            if (distanceToPlayer <= attackDistance)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    animator.SetBool("IsAttacking", true);
                }
                else
                {
                    attackTimer += Time.deltaTime;
                    if (attackTimer >= timeBetweenAttacks)
                    {
                        attackTimer = 0;
                        // Saldýrý tamamlandý
                        animator.SetBool("IsAttacking", false);
                        isAttacking = false;
                    }
                }
            }
            // Düþman oyuncuya yakýn ama henüz saldýrý menzilinde deðilse, oyuncuya doðru ilerle
            else
            {
                isAttacking = false;
                animator.SetBool("IsAttacking", false);

                Vector3 direction = (playerTransform.position - transform.position).normalized;
                direction.y = 0f;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
        // Düþman oyuncudan uzaksa, idle animasyonunu oynat
        else
        {
            isAttacking = false;
            animator.SetBool("IsAttacking", false);

            animator.SetFloat("InputMagnitude", 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }
    }
}
