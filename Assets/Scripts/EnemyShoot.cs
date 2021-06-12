using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShoot : MonoBehaviour, ILife
{
    [SerializeField] UnityEvent startShoot = null;
    [SerializeField] UnityEvent onHit = null;
    [SerializeField] UnityEvent onDeath = null;

    [SerializeField] Transform player = null;
    [SerializeField] GameObject bullet = null;


    [SerializeField] float rangeMax = 0;
    [SerializeField] float cooldownTime = 0;
    float currentCooldownTime = 0;

    [SerializeField] int maxLife = 0;
    int currentLife = 0;

    private void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldownTime > 0)
            currentCooldownTime -= Time.deltaTime;

        if (currentCooldownTime <= 0  && (player.position - transform.position).magnitude <= rangeMax)
        {
            currentCooldownTime = cooldownTime;
            GameObject clone = Instantiate(bullet, transform.position, transform.rotation);
            clone.gameObject.GetComponent<Bullet>().destination = player.position;
            if (startShoot != null)
                startShoot.Invoke();
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
