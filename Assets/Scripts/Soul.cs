﻿using System.Collections;
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

    [SerializeField]
    GameObject auraToActivate = null;


    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
                EndPath();
        }
    }



    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.CompareTag("Player"))
        {
            if (canBePickupByPlayer)
            {
                other.gameObject.GetComponent<Player>().soulReal = null;
                Destroy(gameObject);
            }
        }
        else
            EndPath();
    }

    private void EndPath()
    {
        timer = 0;
        canBePickupByPlayer = true;
        GetComponent<MeshRenderer>().material = colorWhenPickable;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        auraToActivate.SetActive(true);
    }
}
