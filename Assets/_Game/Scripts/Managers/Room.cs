using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviourPun
{
    [SerializeField] private NetManager netManager;
    [SerializeField] private List<TextMeshProUGUI> playerNameTexts;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI roomCountText;
    [SerializeField] private TextMeshProUGUI roomTimerText;
    [SerializeField] private GameObject roomTimerGo;
    [SerializeField] private Button startGameButton;
    [SerializeField] private GameObject connectionGo;
    [SerializeField] private GameObject gameHudGo;

    [Header("Game Start Settings")] [Space(5)] [SerializeField]
    private int autoStartTimer = 5;

    [SerializeField] private int minRequiredPlayers = 3;

    private GameHUD _gameHUD;

    private void Awake()
    {
        _gameHUD = GetComponent<GameHUD>();
        netManager.OnRoomJoinedSuccessfully += SetupLobby;
        netManager.OnRoomLeftSuccessfully += UpdateLobby;
    }

    private void SetupLobby()
    {
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        photonView.RPC("UpdatePlayerNames", RpcTarget.All);
        photonView.RPC("StartGameCheck", PhotonNetwork.MasterClient);
    }

    [PunRPC]
    private void UpdatePlayerNames()
    {
        var players = PhotonNetwork.PlayerList;
        roomCountText.text = $"Players:{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        foreach (var txt in playerNameTexts) txt.text = string.Empty;
        for (var i = 0; i < players.Length; i++) playerNameTexts[i].text = players[i].NickName;
    }

    [PunRPC]
    private void StartGameCheck()
    {
        startGameButton.gameObject.SetActive(PhotonNetwork.CurrentRoom.PlayerCount >= minRequiredPlayers);
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            photonView.RPC("UpdateTimer", RpcTarget.All, autoStartTimer);
            photonView.RPC("UpdateTimerActive", RpcTarget.All, true);
            StartCoroutine(Countdown(autoStartTimer));
        }
        else
        {
            StopAllCoroutines();
            photonView.RPC("UpdateTimerActive", RpcTarget.All, false);
        }
    }

    [PunRPC]
    public void LoadLevel()
    {
        if (photonView.IsMine) photonView.RPC("LoadLevel", RpcTarget.Others);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        connectionGo.SetActive(false);
        GameManager.Instance.StartGame();
        _gameHUD.StartHUD();
        gameHudGo.SetActive(true);
    }

    private IEnumerator Countdown(int duration)
    {
        var count = duration;
        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
            photonView.RPC("UpdateTimer", RpcTarget.All, count);
        }

        LoadLevel();
    }

    [PunRPC]
    private void UpdateTimer(int count)
    {
        roomTimerText.text = $"{count}s";
    }

    [PunRPC]
    private void UpdateTimerActive(bool value)
    {
        roomTimerGo.SetActive(value);
    }
}