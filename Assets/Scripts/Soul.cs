using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Soul : MonoBehaviour
{
    [SerializeField] public UnityEvent onStartFlying = null;
    [SerializeField] UnityEvent onStartExplosion = null;
    [SerializeField] UnityEvent onPickUp = null;
    [SerializeField] UnityEvent onEndExplode = null;


    [SerializeField] public float radius = 0;
    bool canBePickupByPlayer = false;
    bool reachPos = false;

    [SerializeField] public Vector3 targetPos = Vector3.zero;
    [HideInInspector] public Vector3 initialPos = Vector3.zero;
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
        if (reachPos)
            return;

        Vector3 dir = (new Vector3(targetPos.x, 0, targetPos.z) - new Vector3(transform.position.x, 0, transform.position.z));
        if((new Vector3(initialPos.x, 0, initialPos.z) - new Vector3(transform.position.x, 0, transform.position.z)).magnitude >= (new Vector3(initialPos.x, 0, initialPos.z) - new Vector3(targetPos.x, 0, targetPos.z)).magnitude)
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

                Player playerScript = other.gameObject.GetComponent<Player>();

                if (playerScript.onPickUp != null)
                    playerScript.onPickUp.Invoke();

                playerScript.rangeFeedBack.SetActive(true);
                playerScript.chainLineRenderer.gameObject.SetActive(false);
                canBePickupByPlayer = false;
                reachPos = false;
                auraToActivate.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        else if (!other.gameObject.CompareTag("Ground") && !other.gameObject.CompareTag("Enemy") && !reachPos)
            EndPath();
    }

    private void EndPath()
    {
        if (onStartExplosion != null)
            onStartExplosion.Invoke();
        reachPos = true;
    }

    public void EndExplosion()
    {   
        if (onEndExplode != null)
            onEndExplode.Invoke();
        canBePickupByPlayer = true;
        GetComponentInChildren<MeshRenderer>().material = colorWhenPickable;
        auraToActivate.SetActive(true);
        
    }
}
