using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed;

    public Arrow arrowPrefab;
    private int arrowNum;
    private bool arrowCharging;
    private float chargeStart;
    private float lastShoot;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5;
        rb = GetComponent<Rigidbody2D>();

        arrowNum = 3;
        lastShoot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(x, y) * moveSpeed;

        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseDir = Vector3.Normalize(mousePos - transform.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            arrowCharging = true;
            chargeStart = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {

            shoot(mouseDir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Guard")
        {
            print(collision.gameObject.name);
        }
    }

    private void shoot(Vector3 direction)
    {
        if (arrowNum == 0) return;

        if (Time.time - lastShoot > 1)
        {
            Arrow a = Instantiate<Arrow>(arrowPrefab, this.transform.position, Quaternion.identity);
            a.Init(direction, Time.time - chargeStart);
            lastShoot = Time.time;
            arrowNum--;
        }
    }
}
