using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ILife
{
    [SerializeField] float dashSpeed = 0f;

    bool isDashing = false;
    [SerializeField] UnityEvent onHit = null;
    [SerializeField] UnityEvent onDeath = null;


    [SerializeField] float speedMove = 0;
    [SerializeField] int maxLife = 0;
    int currentLife = 0;

    [SerializeField] GameObject soulPrefab = null;
    public GameObject soulReal = null;


    void Start()
    {
        currentLife = maxLife;
    }


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
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isDashing = true;

        if(soulReal != null)
        {
            float radius = soulReal.GetComponent<Soul>().radius / 2;
            Vector3 selfToSoul = (soulReal.transform.position - transform.position);

            if (selfToSoul.magnitude > radius)            
                transform.position += selfToSoul.normalized * (selfToSoul.magnitude - radius);
            

        }



        if (Input.GetMouseButtonDown(1) && soulReal == null)
        {
            soulReal = Instantiate(soulPrefab, transform.position, Quaternion.identity);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Vector3 dir = (new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
                soulReal.GetComponent<Rigidbody>().AddForce(dir * 400);
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
            Dash();
    }

    void Dash()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
        isDashing = false;
    }


    public void TakeHit(int damage)
    {
        currentLife -= damage;
        Debug.Log(currentLife.ToString());
        if (onHit != null)
            onHit.Invoke();

        if (currentLife <= 0)
        {
            Debug.Log("Player Dead");
            if (onDeath != null)
                onDeath.Invoke();
        }
    }

}
