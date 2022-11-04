using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor.UI;
using JetBrains.Annotations;
using static UnityEngine.EventSystems.EventTrigger;

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
}

[System.Serializable]
public struct Stats
{
    public float speed;
    public float rotSpeed;
    public List<Point> path;
}

[System.Serializable]
public struct Guards
{
    public List<Stats> member;
}


public class Guard : MonoBehaviour
{
    static int counter = 0;
    public Stats stats;

    // just debug purpose
    void Save(string file)
    {
        string path = Path.Combine(Application.dataPath, "Resources/", file);

        Point p = new Point(3.5f, 3.5f);
        Stats stats;
        stats.speed = 3f;
        stats.rotSpeed = 90f;
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

    void Load(string file)
    {
        string path = Path.Combine(Application.dataPath, "Resources/", file);

        string loadJson = File.ReadAllText(path);
        Guards guards = JsonUtility.FromJson<Guards>(loadJson);
        stats = guards.member[counter];

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
    void Start()
    {
        counter += 1;
        // this.Save("world1.json");
        this.Load("world1.json");
        Point start = stats.path[0];
        transform.position = new Vector3(start.x, start.y, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
