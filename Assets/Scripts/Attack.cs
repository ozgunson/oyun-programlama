using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 10f;
    public float range = 2f;
    public float attackDelay = 1f;

    private bool canAttack = true;
    private Animator animator;
    private Transform target;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // Burada, "Player" etiketine sahip nesneyi hedef olarak se�iyoruz.
    }

    void Update()
    {
        if (canAttack && Vector3.Distance(transform.position, target.position) < range)
        {
            StartCoroutine(AttackDelay());
        }
    }

    IEnumerator AttackDelay()
    {
        canAttack = false;
        animator.SetTrigger("Attack"); // Sald�r� animasyonunu tetikliyoruz.
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    void DoDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range); // Sald�r� menzili i�erisindeki nesneleri al�yoruz.
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player") // Sadece "Player" etiketine sahip nesnelere hasar veriyoruz.
            {
                //hitCollider.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range); // Sald�r� menzilini g�steren bir �izgi �iziyoruz.
    }
}
