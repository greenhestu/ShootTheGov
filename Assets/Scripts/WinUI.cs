using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        Debug.Log("Next Btn Pushed");
        Destroy(gameObject);
        GameManager.Instance.NextStage();
    }
}
