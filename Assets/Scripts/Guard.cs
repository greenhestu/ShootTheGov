using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using JetBrains.Annotations;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public struct Point
{
    public float x;
    public float y;

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(Point p)
    {
        return this.x == p.x && this.y == p.y;
    }
}

// Unique status of each guard.
[System.Serializable]
public struct Stats
{
    public float speed;
    public float rotSpeed;
    public bool isClockwise;
    public float sAngle;
    public float sRadius;
    public List<Point> path;
}

[System.Serializable]
public struct Guards
{
    public List<Stats> member;
}


public class Guard : MonoBehaviour
{
    static int counter = 0; // # of guards
    public Stats stats;
    protected float slowRatio = 0.5f;
    public bool isLoop = false; // is guard looping circular path?
    public bool isFixed = false;

    protected bool isAngry = false;
    protected Player chasing;

    protected Vector3 dest;

    // just debugging purpose
    void Save(string file)
    {
        string path = Path.Combine(Application.dataPath, "Resources/", file);

        Point p = new Point(3.5f, 3.5f);
        Stats stats;
        stats.speed = 3f;
        stats.rotSpeed = 90f;
        stats.sAngle = 30f;
        stats.sRadius = 3f;
        stats.isClockwise = true;
        stats.path = new List<Point>();
        stats.path.Add(p);
        Guards guards;
        guards.member = new List<Stats>();
        guards.member.Add(stats);
        guards.member.Add(stats);
        guards.member.Add(stats);
        string saveJson = JsonUtility.ToJson(guards, true);
        File.WriteAllText(path, saveJson);
        Debug.Log("success");
    }

    public static void ResetCounter()
    {
        counter = 0;
    }

    // Loads guard path data
    void Load(string file)
    {
        string path = Path.Combine(Application.dataPath, "Resources/", file);
        string loadJson = File.ReadAllText(path);
        Guards guards = JsonUtility.FromJson<Guards>(loadJson);
        stats = guards.member[counter];

        // if path is circular
        if (stats.path[0].Equals(stats.path[stats.path.Count - 1]))
            isLoop = true;

        // if guard don't move
        if (stats.path.Count <= 1)
            isFixed = true;

        counter += 1;

        if (loadJson == null)
        {
            Debug.Log("load failed");
        }
        else
        {
            Debug.Log(string.Format("Guard{0} is created", counter));
        }
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        // this.Save("world1.json");
        this.Load("world1.json");
        Point start = stats.path[0];
        Vector3 sp = new Vector3(start.x, start.y, 0);
        transform.position = sp;

        if (stats.path.Count <= 1) //rotating at start point
        {
            transform.up = Vector3.up;
        }
        else
        {
            Point next =  stats.path[1];
            Vector3 np = new Vector3(next.x, next.y, 0);
            Vector3 direction = np - sp;
            float degree = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.Rotate(Vector3.back * degree);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called when guard found player.
    public void Mad(Player player)
    {
        if (!isAngry)
        {
            isAngry = true;
            chasing = player;
            dest = player.transform.position;
        }
    }
}
