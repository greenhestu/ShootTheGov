using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
    }

    public void Init(Vector3 direction)
    {
        this.direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(speed * direction * Time.deltaTime);
    }
}
