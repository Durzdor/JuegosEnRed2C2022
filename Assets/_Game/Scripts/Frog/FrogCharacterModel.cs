using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogCharacterModel : MonoBehaviour
{
    Vector3 centerPoint;
    Vector3 startRC;
    Vector3 endRC;
    public Vector3 endPos;

    public void Move(float fracComplete)
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
    public void RotateRight(bool isRotating)
    {
        if (isRotating)
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }
    }
    public void RotateLeft(bool isRotating)
    {
        if (isRotating)
        {
            transform.Rotate(new Vector3(0, -90, 0));
        }
    }
}
