using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AlligatorModel : MonoBehaviourPun
{
    Rigidbody rb;
    [SerializeField] float jumpForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    [PunRPC]
    public void UpdatePosition(Vector3 newPos)
    {
        transform.position = newPos;
    }
}
