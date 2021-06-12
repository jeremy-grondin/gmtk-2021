using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Soul : MonoBehaviour
{
    [SerializeField] UnityEvent onPickUp = null;


    [SerializeField] public float radius = 0;
    bool canBePickupByPlayer = false;


    [SerializeField] public Vector3 targetPos = Vector3.zero;
    [SerializeField] float speedMove = 0;

    [SerializeField] Material colorWhenPickable = null;
    [SerializeField] GameObject auraToActivate = null;


    private void Start()
    {
        auraToActivate.transform.localScale = new Vector3(radius, radius, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (canBePickupByPlayer)
            return;

        Vector3 dir = (new Vector3(targetPos.x, 0, targetPos.z) - new Vector3(transform.position.x, 0, transform.position.z));
        if(dir.magnitude < 0.1)
            EndPath();
        else
            transform.position += dir.normalized * (speedMove * Time.deltaTime);

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canBePickupByPlayer)
            {
                if (onPickUp != null)
                    onPickUp.Invoke();
                other.gameObject.GetComponent<Player>().soulReal = null;
                other.gameObject.GetComponent<Player>().rangeFeedBack.SetActive(true);
                Destroy(gameObject);
            }
        }
        else
            EndPath();
    }

    private void EndPath()
    {
        canBePickupByPlayer = true;
        GetComponentInChildren<MeshRenderer>().material = colorWhenPickable;
        auraToActivate.SetActive(true);
    }
}
