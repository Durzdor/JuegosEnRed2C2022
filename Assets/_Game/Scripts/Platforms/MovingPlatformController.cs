using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovingPlatformController : MonoBehaviourPun
{
    Vector3 startPos;
    float moveTime;
    bool wait;
    float waitTimeCount = 0;
    [SerializeField] bool waitOnEnd;
    [SerializeField] float waitTime;
    [SerializeField] float moveSpeedRatio = .003f;
    [SerializeField] Vector3 endPos;
    [SerializeField] PlatformType type;
    public PlatformMovement movement;

    void Start()
    {
        if (!photonView.IsMine)
            Destroy(this);
        startPos = transform.position;
        switch (movement)
        {
            case PlatformMovement.Horizontal:
                endPos.y = startPos.y;
                endPos.z = startPos.z;
                break;
            case PlatformMovement.Vertical:
                endPos.x = startPos.x;
                endPos.z = startPos.z;
                break;
            case PlatformMovement.Foward:
                endPos.x = startPos.x;
                endPos.y = startPos.y;
                break;
            default:
                break;
        }
    }
    void Update()
    {
        if (wait)
        {
            waitTimeCount += Time.deltaTime;
            wait = waitTimeCount >= waitTime ? false : true;
        }
        else
            Movement();
    }
    void Movement()
    {
        moveTime += moveSpeedRatio;
        if (transform.position == endPos)
        {
            moveTime = 0;
            waitTimeCount = 0;
            wait = waitOnEnd;
            switch (type)
            {
                case PlatformType.Looper:
                    Vector3 aux = endPos;
                    endPos = startPos;
                    startPos = aux;
                    break;
                case PlatformType.OneWay:
                    transform.position = startPos;
                    break;
                default:
                    break;
            }
        }
        transform.position = Vector3.Lerp(startPos, endPos, moveTime);
    }
}
