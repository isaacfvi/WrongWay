using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trasnparency : MonoBehaviour
{
    [Tooltip("Transparency")]
    public float alpha = 0.5f;
    private SpriteRenderer tree;
    void Start()
    {
        tree = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(tree != null)
        {
            Color color = tree.color;
            color.a = alpha;
            tree.color = color;
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(tree != null)
        {
            Color color = tree.color;
            color.a = 1;
            tree.color = color;
        }
    }
}
