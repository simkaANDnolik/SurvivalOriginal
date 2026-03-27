using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController2w: MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Vector3 randomDirection;
    private float changeDirectionTimer;
    private float minChange = 3f;
    private float maxChange = 8f;
    public Animator enemyAnimator;
    public int knightBossDamage = 40;
    public Slider PHP;
    public static int KnightBoss_HP = 500;
    public static bool isAttack = false;
    private float distanceToPlayer;

    // Новые переменные для атаки
    private bool canMove = true;
    private bool isAttacking = false;
    private float attackCooldown = 0f;
    private float attackAnimationTime = 1.5f; // Время анимации атаки

    // Параметры для настройки
    [SerializeField] private float detectionRange = 40f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldownTime = 3f; // Время между атаками
    [SerializeField] private float moveSpeedNormal = 3.5f;
    [SerializeField] private float moveSpeedAttack = 1.5f; // Скорость во время атаки

    // Таймер для перехода из idle обратно к действиям
    private float idleRecoveryTime = 0.5f;
    private float currentIdleTimer = 0f;
    private bool isInRecovery = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeedNormal;

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
            else
                Debug.LogError("Player not found! Assign player in inspector or wag GameObject with 'Player'");
        }

        ChangeDirection();

        // Начинаем с анимации idle
        enemyAnimator.SetBool("Walk", false);
    }

    void Update()
    {
        // Обновляем кд атаки
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        // Если находимся в восстановлении после атаки
        if (isInRecovery)
        {
            currentIdleTimer -= Time.deltaTime;
            if (currentIdleTimer <= 0f)
            {
                isInRecovery = false;
                // После восстановления проверяем, что делать дальше
            }
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если атакуем, ничего не делаем
        if (isAttacking)
            return;

        // Если в восстановительном периоде после атаки - остаёмся в idle
        if (isInRecovery)
        {
            enemyAnimator.SetBool("Walk", false);
            agent.SetDestination(transform.position);
            return;
        }

        // Если игрок в зоне обнаружения
        if (distanceToPlayer <= detectionRange)
        {
            // Если в радиусе атаки и можем атаковать
            if (distanceToPlayer <= attackRange && canMove && attackCooldown <= 0f)
            {
                StartAttack();
                return;
            }

            // Если не атакуем и можем двигаться
            if (canMove)
            {
                // Поворачиваемся к игроку
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }

                //Двигаемся к игроку, если не в радиусе атаки
                if (distanceToPlayer > attackRange)
                {
                    agent.SetDestination(player.position);
                    enemyAnimator.SetBool("Walk", true);
                }
                else
                {
                    //Стоим на месте, если в радиусе атаки -idle
                    agent.SetDestination(transform.position); 
                    enemyAnimator.SetBool("Walk", false);
                }
            }
        }
        else
        {
            // Патрулирование
            changeDirectionTimer -= Time.deltaTime;
            if (changeDirectionTimer <= 0f)
            {
                ChangeDirection();
            }

            if (canMove)
            {
                agent.SetDestination(transform.position + randomDirection);
                enemyAnimator.SetBool("Walk", true);
            }
        }
    }

    void ChangeDirection()
    {
        randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized * 10f;
        changeDirectionTimer = Random.Range(minChange, maxChange);
    }

    void StartAttack()
    {
        isAttacking = true;
        canMove = false;
        isAttack = true;

        // Останавливаемся
        agent.SetDestination(transform.position);
        agent.velocity = Vector3.zero;

        // Отключаем анимацию ходьбы и запускаем атаку
        enemyAnimator.SetBool("Walk", false);
        enemyAnimator.SetTrigger("Attack");

        // Запускаем таймер для завершения атаки
        Invoke("FinishAttack", attackAnimationTime);

        // Уменьшаем скорость на время атаки
        agent.speed = moveSpeedAttack;
    }

    void FinishAttack()
    {
        isAttacking = false;
        canMove = true;
        isAttack = false;

        // Устанавливаем кд перед следующей атакой
        attackCooldown = attackCooldownTime;

        // Восстанавливаем нормальную скорость
        agent.speed = moveSpeedNormal;

        // Всегда переходим в idle после атаки
        enemyAnimator.SetBool("Walk", false);

        // Останавливаем движение
        agent.SetDestination(transform.position);

        // Включаем восстановительный период в idle
        isInRecovery = true;
        currentIdleTimer = idleRecoveryTime;
        if (DestroyWalls.invincible == false)
        {
            if (distanceToPlayer <= attackRange)
            {
                Debug.Log(" invincible: " + DestroyWalls.invincible);
                PlayaerControllerSec.PLAYER_HP -= knightBossDamage;
                PHP.value = PlayaerControllerSec.PLAYER_HP;
                
            }
        }
        
        




    }
    
    // Проверка атаки по триггеру
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttacking && attackCooldown <= 0f && !isInRecovery)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                StartAttack();
            }
        }
    }

    // Опционально: для визуальной отладки
    void OnDrawGizmosSelected()
    {
        // Зона обнаружения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Зона атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}