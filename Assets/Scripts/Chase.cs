using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform target;
    public float chaseRange = 10f; // Düþmanýn menzili
    private float distanceToTarget = Mathf.Infinity; // Hedefe olan mesafe
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (distanceToTarget <= chaseRange)
        {
            agent.destination = target.position;
        }
    }
}
