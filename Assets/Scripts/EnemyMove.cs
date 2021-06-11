using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] UnityEvent startAttack = null;
    [SerializeField] UnityEvent onHit = null;
    [SerializeField] UnityEvent onDeath = null;

    [SerializeField] private int damage = 0;
    [SerializeField] private float damageCooldown = 0;
    private float damageCurrentCooldown = 0;

    [SerializeField] private float speed = 0;
    [SerializeField] int maxLife = 0;
    int currentLife = 0;
    [SerializeField] private float stoppingDist = 0;

    public GameObject target = null;
    NavMeshAgent navMeshAgent = null;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if(damageCurrentCooldown > 0)
            damageCurrentCooldown -= Time.deltaTime;


        if ((transform.position - target.transform.position).magnitude > stoppingDist)
            navMeshAgent.SetDestination(target.transform.position);
        else
        {
            navMeshAgent.velocity = Vector3.zero;

            if(damageCurrentCooldown <= 0)
            {
                damageCurrentCooldown = damageCooldown;
                target.GetComponent<ILife>().TakeHit(damage);
                if(startAttack != null)
                    startAttack.Invoke();
            }

        }
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
            Destroy(gameObject);
        }
    }
}
