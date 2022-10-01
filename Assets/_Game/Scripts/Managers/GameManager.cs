using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    Instantiator instanceManager;

    static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        instanceManager = GameObject.Find("Instantiator").GetComponent<Instantiator>();
    }
    void Start()
    {
        instanceManager.SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
    }
}