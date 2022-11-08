using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGuard : Guard
{
    private int cursor = 1;
    private bool isBackward = false;
    private bool isDelay = false;

    int NextCursor()
    {
        int nextCur = cursor;

        if (nextCur == stats.path.Count - 1)
        {
            if (isLoop)
                nextCur = 0;
            else
                isBackward = true;
        }
        nextCur = (isBackward) ? nextCur - 1 : nextCur + 1;

        if (nextCur == 0)
            isBackward = false;

        return nextCur;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(string.Format("x: {0}, y: {1}, z: {2}", transform.rotation.x, transform.rotation.y, transform.rotation.z));
        if (isDelay == false)
        {
            float step = stats.speed * Time.deltaTime;
            Vector3 dest = new Vector3(stats.path[cursor].x,stats.path[cursor].y, 0);
            if (Vector3.Distance(dest, transform.position) < 0.01f)
            {
                cursor = NextCursor();
                isDelay = true;
                StartCoroutine(rotation(stats.path[cursor]));
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, dest, step);
        }
    }

    IEnumerator rotation(Point target)
    {
        Vector3 dest = new Vector3(stats.path[cursor].x, stats.path[cursor].y, 0);
        dest = dest - transform.position;

        Vector3 axis;
        Vector3 cross = Vector3.Cross(transform.up, dest);
        //Debug.Log(string.Format("x: {0}, y: {1}, z: {2}", cross.x, cross.y, cross.z));
        if (cross.z >= 0)
            axis = new Vector3(0, 0, 1);
        else
            axis = new Vector3(0, 0, -1);

        while(Vector3.Angle(transform.up, dest) > 0.5) //0.5도 보다 작아지면
        {
            transform.Rotate(axis * Time.deltaTime * stats.rotSpeed);
            yield return null;

        }
        isDelay = false;
    }
}
