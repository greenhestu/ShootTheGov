using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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
        mousePos.z = 0f;

        Vector3 mouseDir = Camera.main.ScreenToWorldPoint(mousePos);
        mouseDir.z = 0;
        mouseDir = Vector3.Normalize(mouseDir - transform.position);

        // for Debug, refill arrow
        if (Time.timeScale == 0)
        {
            if (Input.GetKey(KeyCode.R))
            {
                Guard.ResetCounter();
                SceneManager.LoadScene(0);
                Time.timeScale = 1f;
            }
        }

        if (Input.GetKey(KeyCode.Z))
        {
            arrowNum += 3;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            arrowCharging = true;
            chargeStart = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space)
         || (Time.time - chargeStart > 2 && arrowCharging) )
        {
            arrowCharging = false;
            Shoot(mouseDir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Guard"))
        {
            Die(collision.gameObject.name);
        }
    }

    private void Die(string enemy)
    {
        Debug.Assert(Time.timeScale != 0);
        Time.timeScale = 0;
        print(string.Format("{0} killed you\nPress 'R' to restart", enemy));
    }

    private void Shoot(Vector3 direction)
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
