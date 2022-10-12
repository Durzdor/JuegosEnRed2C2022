using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("Game Timer")] [Space(5)] [SerializeField]
    private TextMeshProUGUI gameTimer;

    [SerializeField] private int minutes;
    [SerializeField] private int seconds;

    [Header("Position Table")] [Space(5)] [SerializeField]
    private List<GameObject> tablePositionsList;

    [Header("Game End References")] [Space(5)] [SerializeField]
    private List<GameObject> gameEndPositionsGoList;

    [SerializeField] private List<TextMeshProUGUI> gameEndNamesList;
    [SerializeField] private GameObject gameResultScreenGo;
    [SerializeField] private TextMeshProUGUI secondPlaceNumberText;
    [SerializeField] private TextMeshProUGUI thirdPlaceNumberText;
    [SerializeField] private TextMeshProUGUI fourthPlaceNumberText;
    [SerializeField] private Image winnerFrogImage;
    [SerializeField] private Image secondFrogImage;
    [SerializeField] private Image thirdFrogImage;
    [SerializeField] private Image fourthFrogImage;


    private PhotonView _photonView;
    private int _currSec;
    private int _currMin;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (_photonView.IsMine)
            StartTimer(seconds, minutes);
        else
            _photonView.RPC("UpdateTimer", RpcTarget.All, _currMin, _currSec);

        _photonView.RPC("UpdateTableVisibility", RpcTarget.All);
        _photonView.RPC("UpdateTableNames", RpcTarget.All);
    }

    private void StartTimer(int sec, int min)
    {
        _currSec = sec;
        _currMin = min;
        _photonView.RPC("UpdateTimer", RpcTarget.All, _currMin, _currSec);
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (_currSec > 0 || _currMin > 0)
        {
            yield return new WaitForSeconds(1);
            _currSec--;
            if (_currMin > 0)
            {
                if (_currSec < 0)
                {
                    _currMin--;
                    _currSec += 60;
                }
            }
            else
            {
                if (_currSec <= 0) _photonView.RPC("EndScreenPopUp", RpcTarget.All, true);
            }

            _photonView.RPC("UpdateTimer", RpcTarget.All, _currMin, _currSec);
        }
    }

    [PunRPC]
    private void UpdateTimer(int min, int sec)
    {
        gameTimer.text = $"{min}m:{sec}s";
    }

    [PunRPC]
    private void EndScreenPopUp(bool value)
    {
        gameResultScreenGo.SetActive(value);
    }

    [PunRPC]
    private void UpdateTableVisibility()
    {
        var playerList = PhotonNetwork.PlayerList;
        foreach (var pos in tablePositionsList) pos.SetActive(false);
        foreach (var pos in gameEndPositionsGoList) pos.SetActive(false);
        for (var i = 0; i < playerList.Length; i++)
        {
            tablePositionsList[i].SetActive(true);
            gameEndPositionsGoList[i].SetActive(true);
        }
    }

    private void UpdateTableNames()
    {
        var playerList = PhotonNetwork.PlayerList;
        for (var i = 0; i < gameEndNamesList.Count; i++) gameEndNamesList[i].text = playerList[i].NickName;
    }
}