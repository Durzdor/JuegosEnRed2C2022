using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FrogCharacterController : MonoBehaviourPun
{
    FrogCharacterModel model;
    FrogCharacterViewer view;
    bool moving = false;
    bool canJump = true;
    float jumpTime;
    Vector3 finishLinePos;
    [SerializeField] float jumpSpeedRatio = .05f;

    private float _finishLineDistance;
    public float FinishLineDistance => _finishLineDistance;

    void Awake()
    {
        model = GetComponent<FrogCharacterModel>();
        if (!photonView.IsMine)
            Destroy(this);
    }
    void Start()
    {
        finishLinePos = GameManager.Instance.GetFinishLinePosition();
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, model.endPos) < .1f && moving)
        {
            moving = false;
            jumpTime = 0;
            photonView.RPC("UpdatePosition", RpcTarget.Others, transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !moving && canJump)
        {
            model.GetCenter();
            moving = true;
        }
        model.RotateRight(Input.GetKeyDown(KeyCode.D));
        model.RotateLeft(Input.GetKeyDown(KeyCode.A));

        if (moving)
        {
            jumpTime += jumpSpeedRatio;
            model.JumpMove(jumpTime);
        }

        _finishLineDistance = Vector3.Distance(transform.position, finishLinePos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.layer)
        {
            case (int)AllLayers.Platform:
                canJump = true;
                transform.parent = collision.transform;
                photonView.RPC("UpdateParent", RpcTarget.Others, collision.gameObject.name, false);
                break;
            case (int)AllLayers.Alligator:
            case (int)AllLayers.Water:
                transform.position = GameManager.Instance.RespawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
                break;
            case (int)AllLayers.FinishLine:
                GameManager.Instance.GotFinishLine(PhotonNetwork.LocalPlayer.ActorNumber);
                break;
            default:
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == (int)AllLayers.Platform)
        {
            canJump = false;
            transform.parent = null;
            photonView.RPC("UpdateParent", RpcTarget.Others, collision.gameObject.name, true);
        }
    }
}
