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
        int length = stats.path.Count;
        int nextCur = cursor;

        if (nextCur == length - 1)
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
        if (!isFixed) NextCursor();

        float trans = Mathf.Max(transform.localScale.x, transform.localScale.y);
        gameObject.GetComponent<DetectPlayer>().setRadius(stats.sRadius/trans, stats.sAngle);
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
                transform.Rotate(transform.forward * Vector3.Angle(transform.up, playerDir));
            }
        }
        else if (isFixed) // rotating at start point
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * stats.rotSpeed);
            return;
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
        //Debug.Log(string.Format("cross x: {0}, y: {1}, z: {2}", cross.x, cross.y, cross.z));
        if (cross.z > 0)
            axis = new Vector3(0, 0, 1);
        else
            axis = new Vector3(0, 0, -1);

        while (Vector3.Cross(transform.up, direction).z * axis.z >= 0 || Vector3.Dot(transform.up, direction) < 0) 
        {
            Vector3 rot = axis * Time.deltaTime * stats.rotSpeed;
            transform.Rotate(axis);
            yield return null;
        } 

        transform.Rotate(axis * Vector3.Angle(transform.up, direction));
        isRotating = false;
    }

    private void OnDrawGizmos()
    {
        Handles.color = arcColor;
        Handles.DrawSolidArc(transform.position, Vector3.forward, transform.up, stats.sAngle / 2, stats.sRadius);
        Handles.DrawSolidArc(transform.position, Vector3.forward, transform.up, -stats.sAngle / 2, stats.sRadius);
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
