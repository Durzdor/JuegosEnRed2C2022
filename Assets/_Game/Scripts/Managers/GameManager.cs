using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    Instantiator instanceManager;
    [Range(1,7)]
    [SerializeField] int alligatorsQuantity;

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
        if (photonView.IsMine)
            instanceManager.SpawnAlligators(alligatorsQuantity);
        instanceManager.SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
    }
    public Vector3 RespawnPlayer(int player)
    {
        return instanceManager.GetPlayerSpawnPoint(player);
    }
    public void GotFinishLine(int player)
    {
        photonView.RPC("PlayerFinish", RpcTarget.All, player);
    }
    void PlayerWin()
    {
        //player win
        Debug.Log("You Won");
    }
    void PlayerLose()
    {
        //player lose
        Debug.Log("You Lost");
    }
    [PunRPC]
    public void PlayerFinish(int player)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == player)
            PlayerWin();
        else
            PlayerLose();
    }
}
