using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 direction;
    private float distance;
    private float floatDistance;
    private float speed;

    // constants
    private const float SPEED = 10f;
    private const float MAX_DISTANCE = 18f;
    private const float MAX_CHARGE_TIME = 2f;

    // Start is called before the first frame update
    void Start()
    {
        speed = SPEED;
    }

    public void Init(Vector3 direction, float chargeTime)
    {
        this.direction = direction;
        chargeTime = chargeTime > MAX_CHARGE_TIME ? MAX_CHARGE_TIME : chargeTime;
        distance = chargeTime / MAX_CHARGE_TIME * MAX_DISTANCE;

        float mouseDirEuler = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        // mouseDirEuler *= direction.y > 0 ? 1 : -1;
        transform.Rotate(0, 0, mouseDirEuler - 45);
    }

    // Update is called once per frame
    void Update()
    {
        if (distance > floatDistance)
        {
            transform.Translate(speed * Time.deltaTime * direction, Space.World);
            floatDistance += speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);

        if (other.gameObject.TryGetComponent(out KingCat king))
        {
            Debug.Log("Stage Win");
            king.Die();
            GameManager.Instance.WinStage();
        }
    }
}
