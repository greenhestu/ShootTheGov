using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatGuard : Guard
{
    private int cursor = 0; // current step of path
    private bool isBackward = false; // are we going back the path?
    private bool isRotating = false; // are we rotating?
    void NextCursor() //Lookup next path
    {
        int nextCur = cursor;

        if (nextCur == stats.path.Count - 1)
        {
            if (isLoop)
                nextCur = 0;
            else
                isBackward = true;
        }
        nextCur += (isBackward) ? -1 : 1;

        if (nextCur == 0)
            isBackward = false;

        cursor = nextCur;
        dest = new Vector3(stats.path[cursor].x, stats.path[cursor].y, 0);
    }

    protected override void Start()
    {
        base.Start();
        NextCursor();
    }

    // Update is called once per frame
    void Update()
    {

        if (isAngry)
        {
            dest = chasing.transform.position;

            // too close
            if (Vector3.Distance(dest, transform.position) < 0.01f)
            {
                if (!isAngry)
                {
                    NextCursor();
                    StartCoroutine(rotation(stats.path[cursor]));
                }
            }

            // angle fault
            Vector3 playerDir = (dest - transform.position).normalized;
            if (Vector3.Dot(playerDir, transform.up.normalized) < 0.9)
            {
                StartCoroutine(rotation(stats.path[cursor]));
            }
        }

        else
        {
            if (isRotating) return;

            // too close
            if (Vector3.Distance(dest, transform.position) < 0.01f)
            {
                NextCursor();
                StartCoroutine(rotation(stats.path[cursor]));
            }
        }

        // chase
        float step = stats.speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, dest, step);
    }

    IEnumerator rotation(Point target) // rotate in place
    {
        isRotating = true;
        Vector3 direction = (dest - transform.position).normalized;

        Vector3 axis;
        Vector3 cross = Vector3.Cross(transform.up, direction);
        //Debug.Log(string.Format("x: {0}, y: {1}, z: {2}", cross.x, cross.y, cross.z));
        if (cross.z >= 0)
            axis = new Vector3(0, 0, 1);
        else
            axis = new Vector3(0, 0, -1);

        while (Vector3.Cross(transform.up, direction).z * cross.z >= 0)
        {
            transform.Rotate(axis * Time.deltaTime * stats.rotSpeed);
            yield return null;
        }

        transform.Rotate(axis * Vector3.Angle(transform.up, direction));
        isRotating = false;
    }

    private void OnDrawGizmos()
    {
        Handles.color = arcColor;
        Handles.DrawSolidArc(transform.position, Vector3.forward, transform.up, sightAngle / 2, sightRadius);
        Handles.DrawSolidArc(transform.position, Vector3.forward, transform.up, -sightAngle / 2, sightRadius);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Wall"))
        {
            stats.speed *= slowRatio;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Wall"))
        {
            stats.speed *= 1/slowRatio;
        }
    }
}
