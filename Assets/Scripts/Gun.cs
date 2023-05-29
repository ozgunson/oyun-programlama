using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;

    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return; // Hedef obje oyuncu ise iþlemi atla
            }
            Debug.Log(hit.transform.name);
        }
    }
}
