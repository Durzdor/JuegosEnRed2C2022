using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FrogCharacterModel : MonoBehaviourPun
{
    private Vector3 centerPoint;
    private Vector3 startRC;
    private Vector3 endRC;
    public Vector3 endPos;
    private Vector3 finishLinePos;
    public float _finishLineDistance;
    public float FinishLineDistance => _finishLineDistance;//(float)Math.Round((decimal)_finishLineDistance, 2);

    private void Awake()
    {
        finishLinePos = GameManager.Instance.GetFinishLinePosition();
        _finishLineDistance = Vector3.Distance(transform.position, finishLinePos);
    }

    public void JumpMove(float fracComplete)
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
        if (isRotating) transform.Rotate(new Vector3(0, 90, 0));
    }

    public void RotateLeft(bool isRotating)
    {
        if (isRotating) transform.Rotate(new Vector3(0, -90, 0));
    }

    [PunRPC]
    public void UpdatePosition(Vector3 newPos)
    {
        transform.position = newPos;
        _finishLineDistance = Vector3.Distance(transform.position, finishLinePos);
        GameManager.Instance.photonView.RPC("UpdatePlayerDistances", RpcTarget.MasterClient, photonView.Owner.NickName, _finishLineDistance);
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