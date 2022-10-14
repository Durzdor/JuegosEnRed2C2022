using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPun
{
    private Instantiator instanceManager;
    private Canvas gameplayCanvas;
    [Range(1, 7)] [SerializeField] private int alligatorsQuantity;
    [SerializeField] private Transform finishLine;

    public static GameManager Instance;

    public void MakeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleton();
        instanceManager = GameObject.Find("Instantiator").GetComponent<Instantiator>();
    }

    public void StartGame()
    {
        if (photonView.IsMine)
            instanceManager.SpawnAlligators(alligatorsQuantity);

        var playerCamera = instanceManager.SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

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

    public Vector3 GetFinishLinePosition()
    {
        return finishLine.position;
    }
}