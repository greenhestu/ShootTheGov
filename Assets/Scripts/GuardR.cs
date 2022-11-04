using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardR : Guard
{
    private Vector3 dir;

    // Update is called once per frame
    void Update()
    {
        dir = new Vector3(0, 0, this.stats.rotSpeed);
        transform.Rotate(dir * Time.deltaTime);
    }
}
