using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorController : MonoBehaviour
{
    public Animator playerAnimator;
    private int playerDamage = 40;
    public Slider Knight_BossHP;
    private float attackRange = 20f;
    
    public Transform Boss;

    // Update is called once per frame
    void Update()
    {
        float distanceToBoss = Vector3.Distance(transform.position, Boss.position);
        if ( (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S) ))
        {
            playerAnimator.SetBool("Walk",true);

        }else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimator.SetTrigger("Attack");
            if (distanceToBoss <= attackRange)
            {
                EnemyController2w.KnightBoss_HP -= playerDamage;
                Knight_BossHP.value = EnemyController2w.KnightBoss_HP;
                return;
            }

        }
        else
        {
            playerAnimator.SetBool("Walk", false);

        }

    }
}
