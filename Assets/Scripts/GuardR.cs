using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardR : MonoBehaviour
{
    public float rotSpeed;
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 90;
    }

    // Update is called once per frame
    void Update()
    {
        dir = new Vector3(0, 0, rotSpeed);
        transform.Rotate(dir * Time.deltaTime);
    }
}
