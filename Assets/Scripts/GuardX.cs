using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardX : MonoBehaviour
{
    private float moveSpeed;
    private float leftMax, rightMax;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        moveSpeed = 3;
        rightMax = 3;
        leftMax = -rightMax;
    }

    // Update is called once per frame
    void Update()
    {
        pos.x += Time.deltaTime * moveSpeed;
        if (leftMax > transform.localPosition.x) {
            pos.x = leftMax;
            moveSpeed *= -1;
        } else if (transform.localPosition.x > rightMax) {
            pos.x = rightMax;
            moveSpeed *= -1;
        }

        transform.position = pos;
    }
}
