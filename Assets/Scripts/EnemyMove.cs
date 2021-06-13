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

    [SerializeField] UnityEvent onNeedToDestroy = null;

    [SerializeField] private int damage = 0;
    [SerializeField] private float damageCooldown = 0;
    private float damageCurrentCooldown = 0;

    [SerializeField] private float speed = 0;
    [SerializeField] float maxLife = 0;
    float currentLife = 0;
    [SerializeField] RectTransform canvasToScaleWithLife = null;
    [SerializeField] float minimumScaleOfCanvas = 0;

    [SerializeField] private float stoppingDist = 0;
     GameObject player = null;
    NavMeshAgent navMeshAgent = null;
    [SerializeField] float radiusDetection = 0;
    bool isFollowing = false; //only used for unityEvent startFollowing and stopFollowing
    [SerializeField] GameObject deathParticles = null;
    private Animator animator;


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        currentLife = maxLife;
        player = GameObject.FindGameObjectWithTag("Player");
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusDetection);
    }

        public void TakeHit(int damage)
    {
        currentLife -= damage;
        float newScale = Mathf.Max(minimumScaleOfCanvas, currentLife / maxLife);
        canvasToScaleWithLife.localScale = new Vector3(newScale, newScale, newScale);

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
        Instantiate(deathParticles);
        Destroy(gameObject);
    }
}
