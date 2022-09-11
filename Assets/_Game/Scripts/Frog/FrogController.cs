using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    FrogModel model;
    FrogViewer view;
    bool moving = false;
    public float time;

    void Awake()
    {
        model = GetComponent<FrogModel>();
        model.startTime = Time.time;
    }
    void Update()
    {
        time = Time.time;
        if (transform.position == model.endPos && moving)
            moving = false;
        if (Input.GetKeyDown(KeyCode.Space) && !moving)
        {
            model.GetCenter();
            moving = true;
        }
        if (moving)
            model.Move();
    }
}
