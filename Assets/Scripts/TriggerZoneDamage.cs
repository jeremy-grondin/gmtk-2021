using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneDamage : MonoBehaviour
{
    [SerializeField] string tagToDamage = null;
    [SerializeField] float damage = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToDamage))
            other.gameObject.GetComponent<ILife>().TakeHit(damage);
    }

}
