using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target; // Ana karakterin transform component'�
    public float attackRange = 2f; // Sald�r� menzili
    public float attackRate = 2f; // Sald�r� h�z�
    private float nextAttackTime = 0f; // Bir sonraki sald�r� zaman�
    private Animator animator;
    private string[] attackAnimations = { "Attack", "Attack2", "Attack3", "Attack4" };

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ana karakter ile aram�zdaki mesafeyi hesapla
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        //ChaseAnimation chaseAnimation = FindObjectOfType<ChaseAnimation>();

        // Mesafe sald�r� menzili i�inde ise ve sald�r� h�z� zaman�ndan �nce ge�mi�se sald�r� animasyonunu ba�lat
        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime && target.GetComponent<Movement>().IsDead() == false)
        {
            // Sald�r� animasyonunu ba�lat
            int randomIndex = Random.Range(0, attackAnimations.Length);
            animator.SetTrigger(attackAnimations[randomIndex]);
            target.GetComponent<Movement>().TakeDamage();

            // Bir sonraki sald�r� zaman�n� g�ncelle
            nextAttackTime = Time.time + 1f / attackRate;
            
        }
    }
}

