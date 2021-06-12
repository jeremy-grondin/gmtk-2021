using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ILife
{

    bool isDashing = false;
    [SerializeField] UnityEvent onThrow = null;
    [SerializeField] public UnityEvent onPickUp = null;
    [SerializeField] UnityEvent onDash = null;
    [SerializeField] UnityEvent onHit = null;
    [SerializeField] UnityEvent onDeath = null;


    [SerializeField] float speedMove = 0;
    [SerializeField] int maxLife = 0;
    public int currentLife = 0;

    public LineRenderer chainLineRenderer = null;
    [SerializeField] GameObject soulGameObject = null;
    Soul soulScript = null;
    [SerializeField] Camera cam = null;

    [SerializeField] Transform SoulStartPoint = null;
    [SerializeField] public GameObject rangeFeedBack = null;
    [SerializeField] RectTransform targetPos = null;
    [SerializeField] float maxRangeTargetPos = 0;

    [SerializeField] float dashSpeed = 0f;
    [SerializeField] float dashCooldown = 0f;
    [SerializeField] float dashDuration = 0f;
    float dashTime = 0f;
    float dashCooldownTimer = 0f;
    bool canDash = true;

    [SerializeField] ParticleSystem particule = null;

    Rigidbody rb;
    Vector3 dashDirection;

    void Start()
    {
        currentLife = maxLife;
        dashTime = dashDuration;
        dashCooldownTimer = dashCooldown;
        rb = GetComponent<Rigidbody>();
        soulScript = soulGameObject.GetComponent<Soul>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGamePause)
            return;

        Vector3 finalTranslation = Vector3.zero;

        if (Input.GetKey(KeyCode.Z) && !isDashing)
            finalTranslation.z += 1; 
        if (Input.GetKey(KeyCode.S) && !isDashing)
            finalTranslation.z -= 1;
        if (Input.GetKey(KeyCode.D) && !isDashing)
            finalTranslation.x += 1;
        if (Input.GetKey(KeyCode.Q) && !isDashing)
            finalTranslation.x -= 1;

        transform.Translate(finalTranslation.normalized * (speedMove * Time.deltaTime), Space.World);

        if (Input.GetMouseButtonDown(0) && canDash)
        {
            onDash.Invoke();
            dashDirection = transform.forward;
            isDashing = true;
            canDash = false;
            ParticleSystem go =  Instantiate(particule, transform.position, transform.rotation * particule.transform.rotation);
            go.Play();
            Destroy(go.gameObject, go.main.duration);
        }

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                canDash = true;
                dashCooldownTimer = dashCooldown;
            }
        }

        //Clamp pos if the soul is out
        if (soulGameObject.activeSelf)
        {
            float radius = soulScript.radius / 2;
            Vector3 selfToSoul = (soulGameObject.transform.position - transform.position);

            if (selfToSoul.magnitude > radius)            
                transform.position += selfToSoul.normalized * (selfToSoul.magnitude - radius);

            chainLineRenderer.SetPosition(0, transform.position);
            chainLineRenderer.SetPosition(1, soulGameObject.transform.position);

        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            Vector3 dir = (new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z));
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            targetPos.anchoredPosition3D = new Vector3(0, Mathf.Min(dir.magnitude, maxRangeTargetPos), 0);

            if (Input.GetMouseButtonDown(1) && !soulGameObject.activeSelf)
            {
                if (onThrow != null)
                    onThrow.Invoke();

                rangeFeedBack.SetActive(false);
                chainLineRenderer.gameObject.SetActive(true);
                soulGameObject.transform.position = SoulStartPoint.position;
                soulScript.initialPos = SoulStartPoint.position;
                soulGameObject.SetActive(true);
                soulScript.targetPos = transform.position + transform.forward * Mathf.Min(dir.magnitude, maxRangeTargetPos);
                if (soulScript.onStartFlying != null)
                    soulScript.onStartFlying.Invoke();
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
        dashTime -= Time.deltaTime;
        rb.velocity = dashDirection * dashSpeed;

        if (dashTime < 0)
        {
            dashTime = dashDuration;
            rb.velocity = Vector3.zero;
            isDashing = false;
        }

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
