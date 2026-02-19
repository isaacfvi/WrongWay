using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    Animator animationController;
    Collider2D hitbox;

    public float cooldownTime = 2f;
    bool canAttack = true;


    void Start()
    {
        animationController = GetComponent<Animator>();
        animationController.SetBool("IsIdle", true);

        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
    }

    public void Attack()
    {
        if (!canAttack) return;
        
        animationController.SetBool("IsIdle", false);
        animationController.SetBool("Attack", true);

        hitbox.enabled = true;

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);

        hitbox.enabled = false;

        animationController.SetBool("Attack", false);
        animationController.SetBool("IsIdle", true);

        canAttack = true;
    }
}
