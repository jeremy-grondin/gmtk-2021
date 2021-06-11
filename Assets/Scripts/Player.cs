using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speedMove = 0;

    [SerializeField]
    GameObject soulPrefab = null;

    GameObject soulReal = null;

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

        if (Input.GetMouseButtonDown(1) && soulReal == null)
        {
            soulReal = Instantiate(soulPrefab, transform.position, Quaternion.identity);
            soulReal.GetComponent<Rigidbody>().AddForce(new Vector3(10, 0, 0));

        }

        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Colision");
            if (collision.gameObject.CompareTag("soul"))
            {
                Destroy(collision.gameObject);
                soulReal = null;
                Debug.Log("Colision and tag");
            }
        }
    }
}
