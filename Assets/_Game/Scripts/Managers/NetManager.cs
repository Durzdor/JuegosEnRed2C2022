using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class NetManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI status;
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TMP_InputField maxPlayersCount;
    [SerializeField] TMP_InputField roomName;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        button.interactable = false;
        status.text = "Connecting to Master...";
    }
    public void Connect()
    {
        if (string.IsNullOrEmpty(roomName.text) || string.IsNullOrWhiteSpace(roomName.text))
            return;
        if (string.IsNullOrEmpty(nickname.text) || string.IsNullOrWhiteSpace(nickname.text))
            return;
        if (string.IsNullOrEmpty(maxPlayersCount.text) || string.IsNullOrWhiteSpace(maxPlayersCount.text))
            return;

        button.interactable = false;
        PhotonNetwork.NickName = nickname.text;

        var options = new RoomOptions
        {
            MaxPlayers = byte.Parse(maxPlayersCount.text),
            IsOpen = true,
            IsVisible = true
        };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        status.text = "Connecting to Lobby...";
    }
    public override void OnJoinedLobby()
    {
        button.interactable = true;
        status.text = "Connected to Lobby.";
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
    public override void OnDisconnected(DisconnectCause cause)
    {
        status.text = "Connection Failed.";
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        status.text = "Join Room failed.";
        button.interactable = true;
    }
}
