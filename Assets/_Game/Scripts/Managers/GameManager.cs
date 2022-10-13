using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPun
{
    private Instantiator instanceManager;
    private Canvas gameplayCanvas;
    [Range(1, 7)] [SerializeField] private int alligatorsQuantity;

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
        photonView.RPC("PlayerFinish", RpcTarget.All, player);
    }

    private void PlayerWin()
    {
        var text = GameObject.Find("EndText").GetComponentInChildren<TextMeshProUGUI>();
        text.text = "You WON!";
        Debug.Log("You Won");
    }

    private void PlayerLose()
    {
        var text = GameObject.Find("EndText").GetComponentInChildren<TextMeshProUGUI>();
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