using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogCharacterController : MonoBehaviour
{
    FrogCharacterModel model;
    FrogCharacterViewer view;
    bool moving = false;
    float jumpTime;
    [SerializeField] float jumpSpeedRatio = .05f;

    void Awake()
    {
        model = GetComponent<FrogCharacterModel>();
    }
    void Update()
    {
        if (transform.position == model.endPos && moving)
        {
            moving = false;
            jumpTime = 0;
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
            model.Move(jumpTime);
        }
    }
}
