using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    public GameObject Gun;
    public Transform WeaponParent;
    public bool isEquipped;

    // Start is called before the first frame update
    void Start()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.G))
        {
            Drop();
        }
    }

    void Drop()
    {
        WeaponParent.DetachChildren();
        Gun.transform.eulerAngles = new Vector3(Gun.transform.position.x, Gun.transform.position.z, Gun.transform.position.y);
        Gun.GetComponent<MeshCollider>().enabled = true;
        Gun.GetComponent<Rigidbody>().isKinematic = false;
        //Gun.GetComponent<BoxCollider>().isTrigger = true;
        isEquipped = false;
    }

    void Equip()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;
        Gun.transform.position = WeaponParent.transform.position;
        Gun.transform.rotation = WeaponParent.transform.rotation;
        //Gun.GetComponent<BoxCollider>().isTrigger = false;

        Gun.GetComponent<MeshCollider>().enabled = false;
        Gun.transform.SetParent(WeaponParent);
        isEquipped = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                Equip();
            }
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody weaponRigidbody = Gun.GetComponent<Rigidbody>();
        weaponRigidbody.position = collision.contacts[0].point;

        weaponRigidbody.velocity = Vector3.zero;
        weaponRigidbody.angularVelocity = Vector3.zero;

        weaponRigidbody.isKinematic = true;
    }

}
