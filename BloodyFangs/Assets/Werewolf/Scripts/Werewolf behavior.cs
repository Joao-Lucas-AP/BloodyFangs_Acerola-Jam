using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Werewolfbehavior : MonoBehaviour
{
    [Header("Chasing mechanics")]
    Transform playerTransform;

    [SerializeField]
    float aggroRange;
    [SerializeField]
    float safeRange;
    [SerializeField]
    float attackRange;

    [SerializeField]
    float moveSpeed;

    Rigidbody2D rB2D;

    bool canMove = true;

    public AudioSource audioSource;

    [Header("Animator")]
    public Animator animator;

    [Header("Health")]
    [SerializeField] int maxHealth = 15;
    int currentHealth;

    [Header("Attack")]
    [SerializeField] float attackDamage;
    CircleCollider2D attackCollider;
    float nextAttackTime = 0f;
    public float attackRate = 1f;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        rB2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        attackCollider = GetComponent<CircleCollider2D>();
        attackCollider.enabled = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (currentHealth <= 0)
        {
            audioSource.Play();
            Die();
        }

        if (canMove == true)
        {
            print("Distance to player is: " + distanceToPlayer);

            if (distanceToPlayer <= aggroRange)
            {
                ChasePlayer();
            }

            if (distanceToPlayer >= safeRange)
            {
                StopChasing();
            }

            if (distanceToPlayer <= attackRange)
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
        if (canMove == false)
        {
            rB2D.velocity = Vector2.zero;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isDead == true)
        {
            animator.SetBool("IsWalking", false);
            this.enabled = false;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        canMove = false;
    }

    public void AttackDamageEvent()
    {
        attackCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            player.GetComponent<PlayerMovement>().isDead = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            currentHealth--;
        }
    }

    //Death event code
    void Die()
    {
        Debug.Log("Enemy died!");
        rB2D.velocity = Vector2.zero;
        animator.SetBool("IsDead", true);
        StopAttack();

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        this.enabled = false;
    }

    //Aggro system
    void ChasePlayer()
    {
        animator.SetBool("IsWalking", true);

        if (transform.position.x < playerTransform.position.x)
        {
            //Enemy is to the left side of player, so it moves to the right
            rB2D.velocity = new Vector2(moveSpeed, 0f);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (transform.position.x > playerTransform.position.x)
        {
            //Enemy is to the right side of player, so it moves to the left
            rB2D.velocity = new Vector2(-moveSpeed, 0f);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    void StopChasing()
    {
        animator.SetBool("IsWalking", false);
        rB2D.velocity = Vector2.zero;
    }

    //Function called by animator to make enemy walk when attack animation finishes
    public void CanMoveEvent()
    {
        canMove = true;
    }

    public void StopAttack()
    {
        attackCollider.enabled = false;
    }
}
