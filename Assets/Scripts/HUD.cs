using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] GameObject life1 = null;
    [SerializeField] GameObject life2 = null;
    [SerializeField] GameObject life3 = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerHit()
    {
        int playerLife = Player.currentLife;

        switch(playerLife)
        {
            case 0:
                life1.SetActive(false);
                break;
            case 1:
                life2.SetActive(false);
                break;
            case 2:
                life3.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void PlayerHeal()
    {
        int playerLife = Player.currentLife;
        Debug.Log("HEAL HUD : " + Player.currentLife);
        switch (playerLife)
        {
            case 1:
                life1.SetActive(true);
                break;
            case 2:
                life2.SetActive(true);
                break;
            case 3:
                life3.SetActive(true);
                break;
            default:
                break;
        }
    }
}
