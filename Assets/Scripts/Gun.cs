using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 50f;
    public float range = 100f;

    public Camera cam;
    public GameObject obj;
    public GameObject obj2;

    void Start()
    {
        obj = GameObject.Find("WeaponParent");
        obj2 = GameObject.Find("Aim");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && obj.transform.childCount > 0)
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

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
