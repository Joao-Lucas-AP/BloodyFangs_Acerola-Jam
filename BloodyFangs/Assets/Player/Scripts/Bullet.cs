using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    public GameObject bloodFX;

    void Update()
    {
        Destroy(gameObject, 2f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Werewolf"))
        {
            var bloodEffect = Instantiate(bloodFX, gameObject.transform.position, gameObject.transform.rotation);
        }
        Destroy(gameObject);
    }
}
