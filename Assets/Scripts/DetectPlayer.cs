using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DetectPlayer : MonoBehaviour
{
    Mesh mesh;
    private Color arcColor;

    //시선추적 해야 함
    float radius = 0f;
    float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        arcColor = new Color(1f, 0f, 0f, 0.1f); //red
        gameObject.GetComponent<MeshRenderer>().material.color = arcColor;
        //gameObject.GetComponent<MeshRenderer>().material.
        this.GetComponent<MeshFilter>().mesh = this.mesh;
    }
    public void setSight(float radius, float angle)
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
            {
                arcColor = new Color(0f, 0f, 0f, 0f); //set sight transparent
                gameObject.GetComponent<MeshRenderer>().material.color = arcColor;
                gameObject.GetComponentInParent<Guard>().Mad(collider.gameObject.GetComponent<Player>());
            }
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
        Debug.Log(hit.collider.name);
        return true;
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        mesh.Clear();
        Vector3 left = Quaternion.AngleAxis(-angle / 2, Vector3.forward) * Vector3.up;
        Vector3 right = Quaternion.AngleAxis(angle / 2, Vector3.forward) * Vector3.up;
        Vector3[] sight = new Vector3[4]
        {
            new Vector3(0,0,0),
            left,
            Vector3.up,
            right
        };
        int[] tris = new int[6]
        {
            0, 2, 1,
            0, 3, 2
        };

        sight = sight.Select(vec => vec * radius).ToArray();
        mesh.vertices = sight;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
#endif
    }
}
