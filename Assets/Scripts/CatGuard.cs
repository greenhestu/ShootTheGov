using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGuard : Guard
{
    private int cursor = 0; // current step of path
    private bool isBackward = false; // are we going back the path?
    private bool isDelay = false; // are we rotating?

    void NextCursor()
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
        //Debug.Log(string.Format("x: {0}, y: {1}, z: {2}", transform.rotation.x, transform.rotation.y, transform.rotation.z));
       
        if (isAngry)
        {
            Vector3 playerDir = (dest - transform.position).normalized;
            if (Vector3.Dot(playerDir, transform.up) < 0.9)
            {
                StartCoroutine(rotation(stats.path[cursor]));
            }
        }


        if (!isDelay) // move forward
        {
            float step = stats.speed * Time.deltaTime;

            if (Vector3.Distance(dest, transform.position) < 0.01f)
            {
                NextCursor();
                StartCoroutine(rotation(stats.path[cursor]));
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, dest, step);
        }
    }

    IEnumerator rotation(Point target)
    {
        isDelay = true;
        Vector3 direction = (dest - transform.position).normalized;

        Vector3 axis;
        Vector3 cross = Vector3.Cross(transform.up, dest);
        //Debug.Log(string.Format("x: {0}, y: {1}, z: {2}", cross.x, cross.y, cross.z));
        if (cross.z >= 0)
            axis = new Vector3(0, 0, 1);
        else
            axis = new Vector3(0, 0, -1);

        while(Vector3.Angle(transform.up, direction) > 0.5) //0.5도 보다 작아지면
        {
            transform.Rotate(axis * Time.deltaTime * stats.rotSpeed);
            yield return null;
        }
        isDelay = false;
    }
}
