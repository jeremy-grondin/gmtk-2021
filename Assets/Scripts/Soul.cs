using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField]
    bool canBePickupByPlayer = false;

    [SerializeField]
    float timer = 2;

    [SerializeField]
    Material colorWhenPickable = null;


    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                canBePickupByPlayer = true;
                GetComponent<MeshRenderer>().material = colorWhenPickable;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canBePickupByPlayer)
        {
            other.gameObject.GetComponent<Player>().soulReal = null;
            Destroy(gameObject);
        }
    }
}
