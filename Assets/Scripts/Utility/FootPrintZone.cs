using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FootPrintZone : MonoBehaviour
{
    public GameObject footprintPrefab;
    public float timeToFootprint = 0.5f;

    private float timer;
    private Transform currentTarget;

    void FixedUpdate()
    {
        
        if (currentTarget == null) return;
        Debug.Log("Rodando: " + timer);
        timer += Time.fixedDeltaTime;

        if (timer >= timeToFootprint)
        {
            Instantiate(
                footprintPrefab,
                currentTarget.position,
                Quaternion.identity
            );

            timer = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entrou");
        currentTarget = collision.transform;
        timer = 0f;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Saiu");
        if (collision.transform == currentTarget)
        {
            currentTarget = null;
            timer = 0f;
        }
    }
}
