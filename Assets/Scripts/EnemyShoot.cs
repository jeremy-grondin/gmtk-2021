using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShoot : MonoBehaviour, ILife
{
    [SerializeField] UnityEvent startShoot = null;
    
    [SerializeField] UnityEvent onHit = null;
    [SerializeField] UnityEvent onDeath = null;
    [SerializeField] UnityEvent onNeedToDestroy = null;
    [SerializeField] UnityEvent onPlayerEnterRange = null;
    [SerializeField] UnityEvent onPlayerExitRange = null;

    Transform player = null;
    [SerializeField] GameObject bullet = null;
    [SerializeField] GameObject bulletSpawnPoint = null;

    [Header("Shooting")]

    [SerializeField] float rangeMax = 0;
    [SerializeField] float cooldownTimeBetweenWaves = 0;
    [SerializeField] float cooldownTimeBetweenBullets = 0;
    [SerializeField] int nbOfBulletsInWaves = 0;
    float currentCooldownTime = 0;
    int currentNbOfbulletShot = 0;
    bool isDead = false;

    [Header("Health")]
    [SerializeField] float maxLife = 0;
    float currentLife = 0;
    [SerializeField] RectTransform canvasToScaleWithLife = null;
    [SerializeField] float minimumScaleOfCanvas = 0;
    [SerializeField] GameObject deathParticles = null;

    private Animator animator;
    bool isPlayerInRange = false;

    private void Start()
    {
        isDead = false;
        currentLife = maxLife;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldownTime > 0)
            currentCooldownTime -= Time.deltaTime;

        if ((player.position - transform.position).magnitude <= rangeMax)
        {
            if (!isPlayerInRange)
            {
                onPlayerEnterRange.Invoke();
                isPlayerInRange = true;
            }

            
            transform.rotation = Quaternion.LookRotation(new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z), Vector3.up);

            if (currentCooldownTime <= 0)
            {
                if (startShoot != null)
                    startShoot.Invoke();

                //shoot bullet
                currentCooldownTime = cooldownTimeBetweenBullets;
                currentNbOfbulletShot++;
                GameObject clone = Instantiate(bullet, bulletSpawnPoint.transform.position, transform.rotation);                
                clone.gameObject.GetComponent<Bullet>().direction = player.position - transform.position;
                
                if(currentNbOfbulletShot == nbOfBulletsInWaves) // can be replaced by if(currentNbOfbulletShot % nbOfBulletsInWaves == 0)
                {
                    currentCooldownTime = cooldownTimeBetweenWaves;
                    currentNbOfbulletShot = 0;
                }
            }
        }
        else
        {
            if (isPlayerInRange)
            {
                onPlayerExitRange.Invoke();
                isPlayerInRange = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeMax);
    }

    public void TakeHit(float damage)
    {

        currentLife -= damage;
        float newScale = (currentLife / maxLife) * (1 - minimumScaleOfCanvas) + minimumScaleOfCanvas;
        canvasToScaleWithLife.localScale = new Vector3(newScale, newScale, newScale);
        if (onHit != null)
            onHit.Invoke();

        if (currentLife <= 0 && !isDead)
        {
            if (onDeath != null)
                onDeath.Invoke();

            isDead = true;
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
