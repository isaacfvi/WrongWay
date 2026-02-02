using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Target to follow")]
    public Transform target;

    [Tooltip("Height of box where the player has to be")]
    public float boxHeight = 2;
    [Tooltip("Width of box where the player has to be")]
    public float boxWidth = 2;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -10);
    }

    void LateUpdate()
    {
        Vector3 cameraPos = transform.position;
        Vector3 targetPos = target.position;

        float left = cameraPos.x - boxWidth / 2f;
        float right = cameraPos.x + boxWidth / 2f;
        float bottom = cameraPos.y - boxHeight / 2f;
        float top = cameraPos.y + boxHeight / 2f;

        if (targetPos.x < left)
            cameraPos.x = targetPos.x + boxWidth / 2f;
        else if (targetPos.x > right)
            cameraPos.x = targetPos.x - boxWidth / 2f;

        if (targetPos.y < bottom)
            cameraPos.y = targetPos.y + boxHeight / 2f;
        else if (targetPos.y > top)
            cameraPos.y = targetPos.y - boxHeight / 2f;

        transform.position = cameraPos;
    }
}
