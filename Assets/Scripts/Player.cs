using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speedMove = 0;

    [SerializeField]
    GameObject soul = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            transform.Translate(new Vector3(0, 0, speedMove * Time.deltaTime));
        if (Input.GetKey(KeyCode.S))
            transform.Translate(new Vector3(0, 0, -speedMove * Time.deltaTime));
        if (Input.GetKey(KeyCode.D))
            transform.Translate(new Vector3(speedMove * Time.deltaTime, 0, 0));
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(new Vector3(-speedMove * Time.deltaTime, 0, 0));

        if (Input.GetMouseButtonDown(1))
        {
            GameObject tempSoul = Instantiate(soul, transform.position, Quaternion.identity);
            tempSoul.GetComponent<Rigidbody>().AddForce(new Vector3(10, 0, 0));

        }
    }
}
