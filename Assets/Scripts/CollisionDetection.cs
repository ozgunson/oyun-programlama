using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public PlayerAttackTwo pat;
    //public GameObject hitParticle;
    public float health = 100;
    public bool isDead;
    public float destroyDelay = 5f;
    public float timer = 0f;
    public GameObject obj;

    void Start()
    {
        obj = GameObject.FindGameObjectWithTag("Enemy");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && pat.isAttacking && !isDead)
        {
            Debug.Log(other.name);
            other.GetComponent<Animator>().SetTrigger("IsAttacked");
            TakeDamage();

            if (health <= 0 && !isDead)
            {
                isDead = true;
                other.GetComponent<Animator>().SetTrigger("IsDead");
            }

            if (isDead)
            {
                timer += Time.deltaTime;
                if (timer >= destroyDelay)
                {
                    Destroy(obj);
                }
            }
        }
    }

    public void TakeDamage()
    {
        health -= 50;
    }
}
