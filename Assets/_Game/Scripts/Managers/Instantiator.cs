using UnityEngine;
using Photon.Pun;
public class Instantiator : MonoBehaviourPun
{
    [SerializeField] Transform player1SpawnPoint;
    [SerializeField] Transform player2SpawnPoint;
    [SerializeField] Transform player3SpawnPoint;
    [SerializeField] Transform player4SpawnPoint;

    public void SpawnPlayer(int player)
    {
        switch (player)
        {
            case 1:
                PhotonNetwork.Instantiate("Frog", player1SpawnPoint.position, Quaternion.identity);
                break;
            case 2:
                PhotonNetwork.Instantiate("Frog", player2SpawnPoint.position, Quaternion.identity);
                break;
            case 3:
                PhotonNetwork.Instantiate("Frog", player3SpawnPoint.position, Quaternion.identity);
                break;
            case 4:
                PhotonNetwork.Instantiate("Frog", player4SpawnPoint.position, Quaternion.identity);
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