using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AlligatorController : MonoBehaviourPun
{
    public float jumpTimeCount;
    AlligatorModel model;
    [SerializeField] float jumpInterval;

    void Awake()
    {
        model = GetComponent<AlligatorModel>();
        if (!photonView.IsMine)
            Destroy(this);
    }
    void Start()
    {
        photonView.RPC("UpdatePosition", RpcTarget.Others, transform.position);
    }
    void Update()
    {
        JumpLoop();
    }
    void JumpLoop()
    {
        if (jumpTimeCount >= jumpInterval)
        {
            jumpTimeCount = 0;
            model.Jump();
        }
        else
            jumpTimeCount += Time.deltaTime;
    }
}
