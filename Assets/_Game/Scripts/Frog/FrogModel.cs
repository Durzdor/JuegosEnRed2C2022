using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogModel : MonoBehaviour
{
    public Vector3 endPos;
    [SerializeField] float journeyTime = 1f;

    public float startTime;
    public float fracComplete;
    public Vector3 centerPoint;
    public Vector3 startRC;
    public Vector3 endRC;

    // El Movimiento funciona perfecto, pero cuando mas cerca del 1 en fracComplete este mas instantaneo es el salto, no pude hacer q eso no pase
    void Update()
    {
        fracComplete = (Time.time - startTime) / journeyTime;
        if (fracComplete >= 1)
            startTime = Time.time;
    }
    public void Move()
    {
        transform.position = Vector3.Slerp(startRC, endRC, fracComplete);
        transform.position += centerPoint;
    }
    public void GetCenter()
    {
        endPos = transform.position + transform.forward * 1.5f;
        centerPoint = (transform.position + endPos) * .5f;
        centerPoint -= Vector3.up;
        startRC = transform.position - centerPoint;
        endRC = endPos - centerPoint;
    }
}
