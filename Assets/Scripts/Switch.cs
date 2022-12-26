using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    bool s;
    public UnityEvent switchOn;

    // Start is called before the first frame update
    void Start()
    {
        s = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("sw");
        s = !s;
        if (s) switchOn.Invoke();
    }
}
