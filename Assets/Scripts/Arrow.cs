using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 direction;
    private float distance;
    private float floatDistance;
    private float speed;

    private const float MAX_DISTANCE = 50f;
    private const float MAX_CHARGE_TIME = 2f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
    }

    public void Init(Vector3 direction, float chargeTime)
    {
        this.direction = direction;
        distance = chargeTime / MAX_CHARGE_TIME * MAX_DISTANCE;
    }

    // Update is called once per frame
    void Update()
    {
        if (distance > floatDistance)
        {
            this.transform.Translate(speed * Time.deltaTime * direction);
            floatDistance += 1;
        }
    }
}
