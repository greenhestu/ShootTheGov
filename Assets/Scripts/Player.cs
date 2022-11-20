using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed;
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprite = new Sprite[4]; // < v ^ >

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
        spriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (x != 0 || y != 0)
            if (Mathf.Abs(x) < Mathf.Abs(y))
                spriteRenderer.sprite = (y > 0) ? sprite[2] : sprite[1];
            else
                spriteRenderer.sprite = (x > 0) ? sprite[3] : sprite[0];

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Guard"))
        {
            if(collider.GetType() == typeof(BoxCollider2D))
                Die(collider.gameObject.name);
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

            // not intelligent
            arrowNum--;
            GameObject arrowNumUI = GameObject.Find("ArrowNum");
            Text t = arrowNumUI.GetComponent<Text>();
            Debug.Log(t);
            t.text = arrowNum.ToString();
        }
    }
}
