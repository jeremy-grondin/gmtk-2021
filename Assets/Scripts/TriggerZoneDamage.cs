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
        {
            ILife temp = other.gameObject.GetComponent<ILife>();
            if (temp != null)
                temp.TakeHit(damage);
        }
            
    }

}
