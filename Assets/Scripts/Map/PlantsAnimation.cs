using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlantsAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("IsTouched", true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("IsTouched", false);
    }

}
