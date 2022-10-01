using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FrogCharacterController : MonoBehaviourPun
{
    FrogCharacterModel model;
    FrogCharacterViewer view;
    bool moving = false;
    float jumpTime;
    GameObject platformStanding;
    PlatformMovement platformMovement;
    [SerializeField] float jumpSpeedRatio = .05f;

    void Awake()
    {
        model = GetComponent<FrogCharacterModel>();
        if (!photonView.IsMine)
            Destroy(this);
    }
    void Update()
    {
        if (transform.position == model.endPos && moving)
        {
            moving = false;
            jumpTime = 0;
            photonView.RPC("UpdatePosition", RpcTarget.Others, transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !moving)
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
        else
        {
            //if (platformStanding)
                //model.IdleMove(platformStanding.transform, platformMovement);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            //platformStanding = collision.gameObject;
            //platformMovement = collision.gameObject.GetComponent<MovingPlatformController>().movement;
            //Debug.Log($"movement type {platformMovement}");
            transform.parent = collision.transform;
            photonView.RPC("UpdateParent", RpcTarget.Others, collision.gameObject.name, false);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            transform.parent = null;
            photonView.RPC("UpdateParent", RpcTarget.Others, collision.gameObject.name, true);
            //platformStanding = null;
        }
    }
}
