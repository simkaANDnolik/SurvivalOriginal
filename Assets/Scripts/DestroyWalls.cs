using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWalls : MonoBehaviour
{
    public ParticleSystem wallDestroyEf;
    public GameObject wallGr1;
    public GameObject wallGr2;
    public GameObject wallGr3;


  
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("WallDetecter"))
        {

            Debug.Log("0");
            Debug.Log(EnemyController2w.isAttack);
            if (EnemyController2w.isAttack == true)
            {
                if (wallDestroyEf != null)
                {
                    wallDestroyEf.Play();
                }
                Destroy(wallDestroyEf, 0.5f);

                wallGr1.SetActive(false);
                wallGr2.SetActive(false);
                wallGr3.SetActive(false);
                Destroy(gameObject, 5f);

            }
        }

    }
}

    //private void Update()
    //{
    //    Debug.Log(EnemyController2w.isAttack);
    //}
