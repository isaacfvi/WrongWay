using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FootPrintZone : MonoBehaviour
{
    [Tooltip("Footprint Prefab")]
    public GameObject footprintPrefab;
    [Tooltip("Time between footprints")]
    public float timeToFootprint = 0.5f;

    [Tooltip("Footprint Holder")]
    public GameObject footprintHolder;

    private float timer;
    private Transform currentTarget;

    void FixedUpdate()
    {
        
        if (currentTarget == null) return;
        timer += Time.fixedDeltaTime;

        if (timer >= timeToFootprint)
        {
            Instantiate(
                footprintPrefab,
                currentTarget.position,
                Quaternion.identity,
                footprintHolder.transform
            );

            timer = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        currentTarget = collision.transform;
        timer = 0f;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == currentTarget)
        {
            currentTarget = null;
            timer = 0f;
        }
    }
}
