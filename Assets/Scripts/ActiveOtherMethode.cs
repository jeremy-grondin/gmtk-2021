using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOtherMethode : MonoBehaviour
{

    [SerializeField] EnemyMove reference = null ;
    [SerializeField] EnemyShoot shooter = null ;


   public void DestroyRoutine()
   {
       reference.DestroyRoutine();
   }

    public void DestroyRoutineShooter()
    {
        shooter.DestroyRoutine();
    }

}
