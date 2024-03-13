using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Attack mechanics")]
    public Transform BulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public int attackDamage = 1;
    bool canMove = true;

    [Header("Attack rate time")]
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    [Header("Movement")]
    float HorizontalMovement;
    public float MovementSpeed = 10f;
    public bool isFlipped = false;

    [Header("Health")]
    public bool isDead = false;

    void Start()
    {

    }

    void Update()
    {
        HorizontalMovement = Input.GetAxisRaw("Horizontal");

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GetComponent<UIPlayer>().hp -= 25f;
            canMove = false;

            if (GetComponent<UIPlayer>().hp <= 0)
            {
                Death();
            }
        }
    }

    public void TakeDamageFromEnemies(float damage)
    {
        GetComponent<UIPlayer>().hp -= damage;
        canMove = false;

        if (GetComponent<UIPlayer>().hp <= 0)
        {
            Death();
        }
    }

    //Movement update session
    void FixedUpdate()
    {
        if (canMove == true)
        {
            transform.position += new Vector3(HorizontalMovement * MovementSpeed * Time.deltaTime, 0, 0);
            if (HorizontalMovement > 0)
            {
                animator.SetBool("IsWalking", true);
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                isFlipped = false;
            }
            if (HorizontalMovement < 0)
            {
                animator.SetBool("IsWalking", true);
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                isFlipped = true;
            }
            if (HorizontalMovement == 0)
            {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    //Attack code
    void Attack()
    {
        animator.SetTrigger("Shoot");
        var bullet = Instantiate(bulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
        canMove = false;
        bullet.GetComponent<Rigidbody2D>().velocity = BulletSpawn.right * bulletSpeed;
    }

    void Death()
    {
         GetComponent<BoxCollider2D>().enabled = false;
         GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
         isDead = true;
         this.enabled = false;
    }

    public void BackToMove()
    {
        canMove = true;
    }
}
