﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShoot : MonoBehaviour, ILife
{
    [SerializeField] UnityEvent startShoot = null;
    
    [SerializeField] UnityEvent onHit = null;
    [SerializeField] UnityEvent onDeath = null;
    [SerializeField] UnityEvent onNeedToDestroy = null;

    [SerializeField] Transform player = null;
    [SerializeField] GameObject bullet = null;

    [Header("Shooting")]

    [SerializeField] float rangeMax = 0;
    [SerializeField] float cooldownTime = 0;
    float currentCooldownTime = 0;

    [Header("Health")]
    [SerializeField] int maxLife = 0;
    int currentLife = 0;
    [SerializeField] GameObject deathParticles = null;

    private Animator animator;

    private void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldownTime > 0)
            currentCooldownTime -= Time.deltaTime;

        if ((player.position - transform.position).magnitude <= rangeMax)
        {
            transform.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);

            if (currentCooldownTime <= 0)
            {
                if (startShoot != null)
                    startShoot.Invoke();

                currentCooldownTime = cooldownTime;
                GameObject clone = Instantiate(bullet, transform.position, transform.rotation);
                clone.gameObject.GetComponent<Bullet>().direction = player.position - transform.position;
            }
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeMax);
    }

    public void TakeHit(int damage)
    {
        currentLife -= damage;
        if (onHit != null)
            onHit.Invoke();

        if (currentLife <= 0)
        {
            if (onDeath != null)
                onDeath.Invoke();
            animator = GetComponentInChildren<Animator>();
            animator.SetBool("dead",true);
        }
    }
   public void DestroyRoutine()
    {
        if (onNeedToDestroy != null)
            onNeedToDestroy.Invoke();
        GameObject clone = Instantiate(deathParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
