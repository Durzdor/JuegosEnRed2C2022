using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class NetManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI status;
    
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        button.interactable = false;
        status.text = "Connecting to Master...";
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        status.text = "Connecting to Lobby...";
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        status.text = "Connection Failed.";
    }
    
    public override void OnJoinedLobby()
    {
        button.interactable = true;
        status.text = "Connected to Lobby.";
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        status.text = "Lobby Failed.";
    }

    public void Connect()
    {
        var options = new RoomOptions
        {
            MaxPlayers = 4,
            IsOpen = true,
            IsVisible = true
        };
        PhotonNetwork.JoinOrCreateRoom("Frogger", options, TypedLobby.Default);
        button.interactable = false;
    }

    public override void OnCreatedRoom()
    {
        status.text = "Created Room.";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        status.text = "Created Room failed.";
        button.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        status.text = "Join Room failed.";
        button.interactable = true;
    }
}
