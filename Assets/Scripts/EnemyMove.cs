using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyMove : MonoBehaviour, ILife
{
    [SerializeField] UnityEvent startFollowing = null;
    [SerializeField] UnityEvent stopFollowing = null;
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

    [SerializeField] GameObject player = null;
    NavMeshAgent navMeshAgent = null;
    [SerializeField] float radiusDetection = 0;
    bool isFollowing = false; //only used for unityEvent startFollowing and stopFollowing

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

        //don't follow Player
        if ((transform.position - player.transform.position).magnitude > radiusDetection)
        {
            if (isFollowing && stopFollowing != null)
                stopFollowing.Invoke();
            isFollowing = false;
            navMeshAgent.velocity = Vector3.zero;
        }
        //follow Player
        else
        {
            if (!isFollowing && startFollowing != null)
                startFollowing.Invoke();
            isFollowing = true;

            //Continue to follow
            if ((transform.position - player.transform.position).magnitude > stoppingDist)
                navMeshAgent.SetDestination(player.transform.position);
            //Close enough
            else
            {
                navMeshAgent.velocity = Vector3.zero;

                if (damageCurrentCooldown <= 0)
                {
                    damageCurrentCooldown = damageCooldown;
                    player.GetComponent<ILife>().TakeHit(damage);
                    if (startAttack != null)
                        startAttack.Invoke();
                }

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
