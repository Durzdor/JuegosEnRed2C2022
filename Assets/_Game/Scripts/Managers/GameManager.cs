using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPun
{
    private Instantiator instanceManager;
    private Canvas gameplayCanvas;
    [Range(1, 7)] [SerializeField] private int alligatorsQuantity;
    [SerializeField] private Transform finishLine;
    [SerializeField] private GameHUD gameHud;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

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
        gameHud.CallEndScreen();
    }
    public Vector3 GetFinishLinePosition()
    {
        return finishLine.position;
    }
}