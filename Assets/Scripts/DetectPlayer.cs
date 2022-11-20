using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    //시선추적 해야 함
    float radius = 0f;
    float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void setRadius(float radius, float angle)
    {
        this.radius = radius;  
        this.angle = angle;
        CircleCollider2D sight = gameObject.GetComponent<CircleCollider2D>();
        sight.radius = this.radius;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (IsVisible(GameObject.Find("Player").transform.position, angle))
                gameObject.GetComponent<Guard>().Mad(collider.gameObject.GetComponent<Player>());
        }
    }

    private bool IsVisible(Vector3 location, float sight)
    {
        Vector3 direction = location - transform.position;
        float angle = Vector3.Angle(direction, transform.up);
        if (angle < sight)
            return !IsMasked(direction);

        return false;
    }

    private bool IsMasked(Vector3 direction)
    {
        int mask = 1 << LayerMask.NameToLayer("Default");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, mask);
        if (hit.collider.name == "Player")
        {
            Debug.DrawRay(transform.position, direction, Color.red);
            return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
