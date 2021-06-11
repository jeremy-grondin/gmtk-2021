using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speedMove = 0;

    [SerializeField]
    GameObject soulPrefab = null;

    public GameObject soulReal = null;

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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Vector3 dir = (new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
                soulReal.GetComponent<Rigidbody>().AddForce(dir * 100);
            }

        }
    }

}
