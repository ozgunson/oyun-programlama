using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target; // Ana karakterin transform component'ý
    public float attackRange = 2f; // Saldýrý menzili
    public float attackRate = 2f; // Saldýrý hýzý
    private float nextAttackTime = 0f; // Bir sonraki saldýrý zamaný
    private Animator animator;
    private string[] attackAnimations = { "Attack", "Attack2", "Attack3", "Attack4" };

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ana karakter ile aramýzdaki mesafeyi hesapla
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        //ChaseAnimation chaseAnimation = FindObjectOfType<ChaseAnimation>();

        // Mesafe saldýrý menzili içinde ise ve saldýrý hýzý zamanýndan önce geçmiþse saldýrý animasyonunu baþlat
        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime && target.GetComponent<Movement>().IsDead() == false)
        {
            // Saldýrý animasyonunu baþlat
            int randomIndex = Random.Range(0, attackAnimations.Length);
            animator.SetTrigger(attackAnimations[randomIndex]);
            target.GetComponent<Movement>().TakeDamage();

            // Bir sonraki saldýrý zamanýný güncelle
            nextAttackTime = Time.time + 1f / attackRate;
            
        }
    }
}

