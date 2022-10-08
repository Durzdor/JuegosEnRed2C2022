using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameTimer;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;

    private PhotonView _photonView;
    private int _currSec;
    private int _currMin;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        gameTimer.text = $"{minutes}m:{seconds}s";
    }

    private void Start()
    {
        if (_photonView.IsMine)
            StartTimer();
        else
            _photonView.RPC("UpdateTimer", RpcTarget.All, _currMin, _currSec);
    }

    private void StartTimer()
    {
        StartCoroutine(Countdown(minutes, seconds));
    }

    private IEnumerator Countdown(int min, int sec)
    {
        _currSec = sec;
        _currMin = min;
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
                if (_currSec <= 0) Debug.LogError("Game completed");
            }

            _photonView.RPC("UpdateTimer", RpcTarget.All, _currMin, _currSec);
        }
    }

    [PunRPC]
    private void UpdateTimer(int min, int sec)
    {
        gameTimer.text = $"{min}m:{sec}s";
    }
}