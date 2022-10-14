using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Instantiator : MonoBehaviour
{
    [SerializeField] private Transform[] playersSpawnPoints;
    [SerializeField] private List<Transform> alligatorsSpawnPoints;
    [SerializeField] private GameObject cameraPrefab;

    public FrogCharacterModel SpawnPlayerRef(int player)
    {
        var obj = PhotonNetwork.Instantiate($"Player{player}Object", playersSpawnPoints[player - 1].position,
            Quaternion.identity);
        var model = obj.GetComponent<FrogCharacterModel>();
        SpawnPlayerCamera(model);
        return model;
    }

    public void SpawnPlayerCamera(FrogCharacterModel frog)
    {
        var cam = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        cam.GetComponent<CameraController>().SetTarget(frog.transform);
    }

    public Camera SpawnPlayer(int player)
    {
        GameObject playerObject;
        GameObject playerCamera;

        playerObject = PhotonNetwork.Instantiate("Player" + player + "Object", playersSpawnPoints[player - 1].position,
            Quaternion.identity);
        playerCamera = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        playerCamera.GetComponent<CameraController>().SetTarget(playerObject.transform);

        return playerCamera.GetComponent<Camera>();
    }

    public void SpawnAlligators(int alligatorsQuantity)
    {
        if (alligatorsQuantity <= 0) return;
        for (var i = 0; i < alligatorsQuantity; i++)
        {
            var index = Random.Range(0, alligatorsSpawnPoints.Count);
            var sp = alligatorsSpawnPoints[index];

            PhotonNetwork.Instantiate("AligatorObject", sp.position, Quaternion.identity);

            alligatorsSpawnPoints.RemoveAt(index);
        }
    }

    public void CustomSpawn(string prefabname, Vector3 spawnPosition, Vector3 rotation)
    {
        PhotonNetwork.Instantiate(prefabname, spawnPosition,
            rotation == Vector3.zero ? Quaternion.identity : Quaternion.Euler(rotation));
    }

    public Vector3 GetPlayerSpawnPoint(int player)
    {
        return playersSpawnPoints[player - 1].position;
    }
}