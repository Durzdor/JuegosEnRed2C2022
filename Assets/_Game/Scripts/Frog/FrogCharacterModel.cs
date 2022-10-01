using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FrogCharacterModel : MonoBehaviourPun
{
    Vector3 centerPoint;
    Vector3 startRC;
    Vector3 endRC;
    public Vector3 endPos;

    public void JumpMove(float fracComplete)
    {
        transform.position = Vector3.Slerp(startRC, endRC, fracComplete);
        transform.position += centerPoint;
    }
    public void IdleMove(Transform platformTransform, PlatformMovement mov)
    {
        switch (mov)
        {
            case PlatformMovement.Horizontal:
                Debug.Log($"HPos T {transform.position}");
                transform.position += new Vector3(platformTransform.position.x, 0, 0);
                Debug.Log($"HPos {new Vector3(platformTransform.position.x, 0, 0)}");
                Debug.Log($"HPos T {transform.position}");
                break;
            case PlatformMovement.Vertical:
                Debug.Log($"VPos T {transform.position}");
                transform.position += new Vector3(0, platformTransform.position.y, 0);
                Debug.Log($"VPos {new Vector3(0, platformTransform.position.y, 0)}");
                Debug.Log($"VPos T {transform.position}");
                break;
            case PlatformMovement.Foward:
                Debug.Log($"ZPos T {transform.position}");
                transform.position += new Vector3(0, 0, platformTransform.position.z);
                Debug.Log($"ZPos {new Vector3(0, 0, platformTransform.position.z)}");
                Debug.Log($"ZPos T {transform.position}");
                break;
            default:
                break;
        }
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
    [PunRPC]
    public void UpdatePosition(Vector3 newPos)
    {
        transform.position = newPos;
    }
    [PunRPC]
    public void UpdateParent(string parentName, bool nullied)
    {
        if (nullied)
            transform.parent = null;
        else
            transform.parent = GameObject.Find(parentName).transform;
    }
}
