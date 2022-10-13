using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("General")] [Space(5)] [SerializeField]
    private List<Sprite> imageReferenceList;

    [Header("Game Timer")] [Space(5)] [SerializeField]
    private TextMeshProUGUI gameTimer;

    [SerializeField] private int minutes;
    [SerializeField] private int seconds;

    [Header("Position Table")] [Space(5)] [SerializeField]
    private List<GameObject> tablePositionsGoList; // To only show players connected

    [SerializeField] private List<TextMeshProUGUI> tablePositionsNamesList;
    [SerializeField] private List<Image> tablePositionsImagesList;

    [Header("Game End References")] [Space(5)] [SerializeField]
    private List<GameObject> gameEndPositionsGoList; // To only show players connected

    [SerializeField] private List<TextMeshProUGUI> gameEndNamesList;
    [SerializeField] private List<Image> gameEndImagesList;
    [SerializeField] private GameObject gameResultScreenGo;

    private PhotonView _photonView;
    private int _currSec;
    private int _currMin;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void StartHUD()
    {
        if (_photonView.IsMine)
            StartTimer(seconds, minutes);
        else
            _photonView.RPC("UpdateTimer", RpcTarget.All, _currMin, _currSec);

        _photonView.RPC("UpdateTableVisibility", RpcTarget.All);
        _photonView.RPC("UpdateTableNames", RpcTarget.All);
        _photonView.RPC("UpdateTableImages", RpcTarget.All);
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
        foreach (var pos in tablePositionsGoList) pos.SetActive(false);
        foreach (var pos in gameEndPositionsGoList) pos.SetActive(false);
        foreach (var pos in tablePositionsNamesList) pos.gameObject.SetActive(false);
        foreach (var pos in gameEndNamesList) pos.gameObject.SetActive(false);
        for (var i = 0; i < playerList.Length; i++)
        {
            tablePositionsGoList[i].SetActive(true);
            gameEndPositionsGoList[i].SetActive(true);
            tablePositionsNamesList[i].gameObject.SetActive(true);
            gameEndNamesList[i].gameObject.SetActive(true);
        }
    }

    [PunRPC]
    private void UpdateTableNames()
    {
        var playerList = PhotonNetwork.PlayerList;
        for (var i = 0; i < playerList.Length; i++)
        {
            // Aca se lee el orden de los jugadores ¬
            tablePositionsNamesList[i].text = playerList[i].NickName;
            gameEndNamesList[i].text = playerList[i].NickName;
        }
    }

    [PunRPC]
    private void UpdateTableImages()
    {
        var playerList = PhotonNetwork.PlayerList;
        for (var i = 0; i < playerList.Length; i++)
        {
            // Aca se lee el orden de los jugadores ¬
            tablePositionsImagesList[i].sprite = imageReferenceList[i];
            gameEndImagesList[i].sprite = imageReferenceList[i];
        }
    }
}