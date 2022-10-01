using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed;

    public Arrow arrow;
    private float lastShoot;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5;
        rb = GetComponent<Rigidbody2D>();
        lastShoot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(x, y) * moveSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            shoot();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Guard")
        {
            print(collision.gameObject.name);
        }
    }

    private void shoot()
    {
        if (Time.time - lastShoot > 1)
        {
            Arrow a = Instantiate<Arrow>(arrow, this.transform.position, Quaternion.identity);
            a.Init(new Vector3(1, 1, 0));
            lastShoot = Time.time;
        }
    }
}
