using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPun
{
    Instantiator instanceManager;
    Canvas gameplayCanvas;
    GameObject finishPanel;
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
        finishPanel = GameObject.Find("FinishPanel");
        finishPanel.SetActive(false);

        if (photonView.IsMine)
            instanceManager.SpawnAlligators(alligatorsQuantity);

        Camera playerCamera = instanceManager.SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

        gameplayCanvas = GameObject.Find("GameplayCanvas").GetComponent<Canvas>();
        gameplayCanvas.worldCamera = playerCamera;
        gameplayCanvas.planeDistance = 1;
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
        finishPanel.SetActive(true);
        TextMeshProUGUI text = GameObject.Find("EndText").GetComponentInChildren<TextMeshProUGUI>();
        text.text = "You WON!";
        Debug.Log("You Won");
    }
    void PlayerLose()
    {
        finishPanel.SetActive(true);
        TextMeshProUGUI text = GameObject.Find("EndText").GetComponentInChildren<TextMeshProUGUI>();
        text.text = "You LOST";
        Debug.Log("You Lost");
    }
    public void BackToMenu()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
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
