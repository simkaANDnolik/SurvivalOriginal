using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWalls : MonoBehaviour
{
    public ParticleSystem wallDestroyEf;
    public GameObject wallGr1;
    public GameObject wallGr2;
    public GameObject wallGr3;
    public static bool invincible = false;

  
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("WallDetecter"))
        {
            

            //Debug.Log(EnemyController2w.isAttack);
            if (EnemyController2w.isAttack == true)
            {
                invincible = true;
                if (wallDestroyEf != null)
                {
                    wallDestroyEf.Play();
                }
                Destroy(wallDestroyEf, 0.5f);
                
                wallGr1.SetActive(false);
                wallGr2.SetActive(false);       
                wallGr3.SetActive(false);

                StartCoroutine(DisableInvincibleAfterDelay(2f));

                Destroy(gameObject, 5f);
                
            }
        }

    }
   
    IEnumerator DisableInvincibleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Проверяем, существует ли еще объект
        if (this != null && gameObject != null)
        {
            invincible = false;
            Debug.Log("Неуязвимость отключена");
        }
    }
}

    //private void Update()
    //{
    //    Debug.Log(EnemyController2w.isAttack);
    //}
