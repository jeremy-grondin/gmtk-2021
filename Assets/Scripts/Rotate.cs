using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    float speedRotation = 0;

    [SerializeField]
    float speedMove = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,  speedRotation * Time.deltaTime, 0));

        if (Input.GetKey(KeyCode.Space))
            transform.Translate(new Vector3(0, speedMove * Time.deltaTime, 0));
        if (Input.GetKey(KeyCode.LeftControl))
            transform.Translate(new Vector3(0, -speedMove * Time.deltaTime, 0));
    }
}
