using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Vector3 destination = Vector3.zero;
    [SerializeField] float speed = 0;
    [SerializeField] int damage = 0;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = destination - transform.position;
        transform.position += dir.normalized * (speed * Time.deltaTime);

        if (dir.magnitude < 0.1)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<Player>().TakeHit(damage);

        if (!other.gameObject.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
