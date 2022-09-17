using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardY : MonoBehaviour
{
    private float moveSpeed;
    private float upMax, downMax;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        moveSpeed = 3;
        upMax = 2;
        downMax = -upMax;
    }

    // Update is called once per frame
    void Update()
    {
        pos.y += Time.deltaTime * moveSpeed;
        if (downMax > transform.localPosition.y)
        {
            pos.y = downMax;
            moveSpeed *= -1;
        }
        else if (transform.localPosition.y >upMax)
        {
            pos.y =upMax;
            moveSpeed *= -1;
        }

        transform.position = pos;
    }
}
