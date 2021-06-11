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
            Vector3 pos = transform.position;
            pos.z += 2.0f;
            GameObject tempSoul = Instantiate(soul, pos, Quaternion.identity);
            Vector3 dir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Plane plane = new Plane(Vector3.up, 0.0f);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                dir = new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z);
            }
            dir.Normalize();
            tempSoul.GetComponent<Rigidbody>().AddForce(dir * 100);
        }
    }
}
