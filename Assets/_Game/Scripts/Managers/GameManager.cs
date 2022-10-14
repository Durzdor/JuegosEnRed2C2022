using System;
using System.Collections.Generic;
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
    [SerializeField] private NetManager netManager;


    public List<string> PlayerReferences { get; private set; }
    private List<float> _playerTableDistances;
    public List<int> PlayerTablePositions { get; private set; }
    private List<GameObject> _frogObj = new List<GameObject>();

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
        PlayerReferences = new List<string>();
        _playerTableDistances = new List<float>();
        PlayerTablePositions = new List<int>();
        MakeSingleton();
        instanceManager = GameObject.Find("Instantiator").GetComponent<Instantiator>();
        //netManager.OnPlayerConnect += SpawnPlayerObject;
    }

    public void StartGame()
    {
        if (!photonView.IsMine) return;
        instanceManager.SpawnAlligators(alligatorsQuantity);

        // PlayerPositions();

        for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var target = PhotonNetwork.PlayerList[i];
            photonView.RPC("SpawnPlayerObject", target, i + 1);
        }
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

    [PunRPC]
    private void SpawnPlayerObject(int playerCount)
    {
        var frogCont = instanceManager.SpawnPlayerRef(playerCount);
        photonView.RPC("PlayerReferenceModify", RpcTarget.MasterClient, frogCont.photonView.Owner.NickName,
            frogCont.FinishLineDistance);

        //PlayerReferenceModify(frogCont.photonView.Owner.NickName, frogCont.FinishLineDistance);
    }

    [PunRPC]
    private void PlayerReferenceModify(string playerName, float playerDistance)
    {
        PlayerReferences.Add(playerName);
        // Agrega todas las ranas a una lista de pos
        _playerTableDistances.Add(playerDistance);
    }

    private void PlayerPositions()
    {
        // Guarda los valores de distancia de las ranas en otro array
        var correctPositions = new float[_playerTableDistances.Count];
        for (var i = 0; i < _playerTableDistances.Count; i++)
            correctPositions[i] = _playerTableDistances[i];
        // Ordena de menor a mayor
        Quicksort.Sort(correctPositions);
        // Genera nueva tabla auxiliar para ordenar las posiciones
        var playerNumberPosition = new List<int>();
        foreach (var dist in correctPositions)
            for (var i = 0; i < _playerTableDistances.Count; i++)
                if (dist == _playerTableDistances[i])
                    playerNumberPosition.Add(i);
        // Pongo todos los valores en la original
        // for (var i = 0; i < playerNumberPosition.Count; i++)
        // {
        //     PlayerTablePositions[i] = playerNumberPosition[i];
        // }
        PlayerTablePositions = playerNumberPosition;
    }
}