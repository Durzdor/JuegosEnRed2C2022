using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Instantiator : MonoBehaviour
{
    [SerializeField] Transform[] playersSpawnPoints;
    [SerializeField] List<Transform> alligatorsSpawnPoints;
    [SerializeField] GameObject cameraPrefab;

    public void SpawnPlayer(int player)
    {
        GameObject playerObject;
        GameObject playerCamera;

        playerObject = PhotonNetwork.Instantiate("PlayerObject", playersSpawnPoints[player - 1].position, Quaternion.identity);
        playerCamera = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        playerCamera.GetComponent<CameraController>().SetTarget(playerObject.transform);
    }
    public void SpawnAlligators(int alligatorsQuantity)
    {
        if (alligatorsQuantity <= 0) return;
        for (int i = 0; i < alligatorsQuantity; i++)
        {
            int index = Random.Range(0, alligatorsSpawnPoints.Count);
            Transform sp = alligatorsSpawnPoints[index];

            PhotonNetwork.Instantiate("AligatorObject", sp.position, Quaternion.identity);

            alligatorsSpawnPoints.RemoveAt(index);
        }
    }
    public void CustomSpawn(string prefabname, Vector3 spawnPosition, Vector3 rotation)
    {
        PhotonNetwork.Instantiate(prefabname, spawnPosition, rotation == Vector3.zero ? Quaternion.identity : Quaternion.Euler(rotation));
    }
    public Vector3 GetPlayerSpawnPoint(int player)
    {
        return playersSpawnPoints[player - 1].position;
    }
}