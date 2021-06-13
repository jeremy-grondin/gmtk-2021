using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Vector3 direction = Vector3.zero;
    [SerializeField] float speed = 0;
    [SerializeField] int damage = 0;

    // Update is called once per frame
    void Update()
    {
        transform.position += direction.normalized * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<Player>().TakeHit(damage);

        if (!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Soul"))
            Destroy(gameObject);
    }
}
