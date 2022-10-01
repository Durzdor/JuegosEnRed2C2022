using UnityEngine;
using Photon.Pun;
public class Instantiator : MonoBehaviour
{
    [SerializeField] Transform player1SpawnPoint;
    [SerializeField] Transform player2SpawnPoint;
    [SerializeField] Transform player3SpawnPoint;
    [SerializeField] Transform player4SpawnPoint;
    [SerializeField] GameObject cameraPrefab;

    public void SpawnPlayer(int player)
    {
        GameObject playerObject;
        GameObject playerCamera;

        switch (player)
        {
            case 1:
                playerObject = PhotonNetwork.Instantiate("PlayerObject", player1SpawnPoint.position, Quaternion.identity);
                playerCamera = GameObject.Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
                playerCamera.GetComponent<CameraController>().SetTarget(playerObject.transform);
                break;
            case 2:
                playerObject = PhotonNetwork.Instantiate("PlayerObject", player2SpawnPoint.position, Quaternion.identity);
                playerCamera = GameObject.Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
                playerCamera.GetComponent<CameraController>().SetTarget(playerObject.transform);
                break;
            case 3:
                playerObject = PhotonNetwork.Instantiate("PlayerObject", player3SpawnPoint.position, Quaternion.identity);
                playerCamera = GameObject.Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
                playerCamera.GetComponent<CameraController>().SetTarget(playerObject.transform);
                break;
            case 4:
                playerObject = PhotonNetwork.Instantiate("PlayerObject", player4SpawnPoint.position, Quaternion.identity);
                playerCamera = GameObject.Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
                playerCamera.GetComponent<CameraController>().SetTarget(playerObject.transform);
                break;
            default:
                break;
        }
    }
    public void CustomSpawn(string prefabname, Vector3 spawnPosition, Vector3 rotation)
    {
        PhotonNetwork.Instantiate(prefabname, spawnPosition, rotation == Vector3.zero ? Quaternion.identity : Quaternion.Euler(rotation));
    }
}