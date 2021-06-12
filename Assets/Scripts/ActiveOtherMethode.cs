using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOtherMethode : MonoBehaviour
{

    [SerializeField] EnemyMove reference = null ;


   public void DestroyRoutine()
   {
       reference.DestroyRoutine();
   }
}
