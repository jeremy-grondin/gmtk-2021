using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ILife
{

    bool isDashing = false;
    [SerializeField] UnityEvent onHit = null;
    [SerializeField] UnityEvent onDeath = null;


    [SerializeField] float dashSpeed = 0f;
    [SerializeField] float speedMove = 0;
    [SerializeField] int maxLife = 0;
    int currentLife = 0;

    [SerializeField] GameObject soulPrefab = null;
    public GameObject soulReal = null;
    [SerializeField] Camera cam = null;

    [SerializeField] Transform SoulStartPoint = null;
    [SerializeField] public GameObject rangeFeedBack = null;
    [SerializeField] RectTransform targetPos = null;
    [SerializeField] float maxRangeTargetPos = 0;


    void Start()
    {
        currentLife = maxLife;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            transform.Translate(new Vector3(0, 0, speedMove * Time.deltaTime), Space.World);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(new Vector3(0, 0, -speedMove * Time.deltaTime), Space.World);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(new Vector3(speedMove * Time.deltaTime, 0, 0), Space.World);
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(new Vector3(-speedMove * Time.deltaTime, 0, 0), Space.World);
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isDashing = true;

        //Clamp pos if the soul is out
        if(soulReal != null)
        {
            float radius = soulReal.GetComponent<Soul>().radius / 2;
            Vector3 selfToSoul = (soulReal.transform.position - transform.position);

            if (selfToSoul.magnitude > radius)            
                transform.position += selfToSoul.normalized * (selfToSoul.magnitude - radius);
            

        }


        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            Vector3 dir = (new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z));
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            targetPos.anchoredPosition3D = new Vector3(0, Mathf.Min(dir.magnitude, maxRangeTargetPos), 0);
            

            if (Input.GetMouseButtonDown(1) && soulReal == null)
            {
                soulReal = Instantiate(soulPrefab, SoulStartPoint.position, Quaternion.identity);
                soulReal.GetComponent<Soul>().targetPos = transform.position + transform.forward * Mathf.Min(dir.magnitude, maxRangeTargetPos);
                rangeFeedBack.SetActive(false);
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
