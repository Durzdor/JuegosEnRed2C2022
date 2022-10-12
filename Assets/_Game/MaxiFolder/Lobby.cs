using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    [SerializeField] private NetManager netManager;
    [SerializeField] private List<TextMeshProUGUI> playerNameTexts;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCountText;
    [SerializeField] private TextMeshProUGUI lobbyTimerText;
    [SerializeField] private GameObject lobbyTimerGo;
    [SerializeField] private Button startGameButton;

    private PhotonView _photonView;
    private const int MIN_REQUIRED_PLAYERS = 3;
    private const int AUTO_START_TIMER = 5;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        netManager.OnRoomJoinedSuccessfully += SetupLobby;
        netManager.OnRoomLeftSuccessfully += UpdateLobby;
    }

    private void SetupLobby()
    {
        lobbyNameText.text = PhotonNetwork.CurrentRoom.Name;
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        _photonView.RPC("UpdateValues", RpcTarget.All);
        _photonView.RPC("StartGameCheck", PhotonNetwork.MasterClient);
    }

    [PunRPC]
    private void UpdateValues()
    {
        var players = PhotonNetwork.PlayerList;
        lobbyCountText.text = $"Players:{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        foreach (var txt in playerNameTexts) txt.text = string.Empty;
        for (var i = 0; i < players.Length; i++) playerNameTexts[i].text = players[i].NickName;
    }

    [PunRPC]
    private void StartGameCheck()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= MIN_REQUIRED_PLAYERS)
            startGameButton.gameObject.SetActive(enabled);
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            _photonView.RPC("UpdateTimer", RpcTarget.All, AUTO_START_TIMER);
            _photonView.RPC("UpdateTimerActive", RpcTarget.All, true);
            StartCoroutine(Countdown(AUTO_START_TIMER));
        }
        else
        {
            StopAllCoroutines();
            _photonView.RPC("UpdateTimerActive", RpcTarget.All, false);
        }
    }

    [PunRPC]
    public void LoadLevel()
    {
        if (_photonView.IsMine) _photonView.RPC("LoadLevel", RpcTarget.Others);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(1);
    }

    private IEnumerator Countdown(int duration)
    {
        var count = duration;
        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
            _photonView.RPC("UpdateTimer", RpcTarget.All, count);
        }

        if (_photonView.IsMine) _photonView.RPC("LoadLevel", RpcTarget.Others);
        PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    private void UpdateTimer(int count)
    {
        lobbyTimerText.text = $"{count}s";
    }

    [PunRPC]
    private void UpdateTimerActive(bool value)
    {
        lobbyTimerGo.SetActive(value);
    }
}