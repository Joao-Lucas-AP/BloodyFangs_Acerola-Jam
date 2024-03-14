using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject bulletLight;
    public AudioSource audioSound;
    public bool isDead = false;

    [Header("Attack rate time")]
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    [Header("Movement")]
    float HorizontalMovement;
    public float MovementSpeed = 10f;
    public bool isFlipped = false;

    [Header("Health")]
    public int health = 1;

    [Header("UI")]
    public GameObject gameOverPanel;

    void Start()
    {
        bulletLight.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        HorizontalMovement = Input.GetAxisRaw("Horizontal");

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                audioSound.Play();
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if(isDead == true)
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
    public void Attack()
    {
        animator.SetTrigger("Shoot");
        canMove = false;
    }

    void BulletEvent()
    {
        var bullet = Instantiate(bulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = BulletSpawn.right * bulletSpeed;
        bulletLight.SetActive(true);
    }

    void Death()
    {
        gameOverPanel.SetActive(true);

        //Tirar isso da build!!
         Debug.Log("Está morto!!!");
    }

    public void BackToMove()
    {
        canMove = true;
    }

    public void LightsOff()
    {
        bulletLight.SetActive(false);
    }
}
