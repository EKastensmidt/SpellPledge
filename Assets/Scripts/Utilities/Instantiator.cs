using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class Instantiator : MonoBehaviour
{
    [SerializeField] private List<Transform> spawns;
    void Start()
    {
        List<Photon.Realtime.Player> playerList = PhotonNetwork.PlayerList.ToList();
        switch (playerList.Count)
        {
            case 1:
                PhotonNetwork.Instantiate("Player", spawns[0].position, Quaternion.identity);
                break;
            case 2:
                PhotonNetwork.Instantiate("Player2", spawns[1].position, Quaternion.identity);
                break;
            case 3:
                PhotonNetwork.Instantiate("Player3", spawns[2].position, Quaternion.identity);
                break;
            case 4:
                PhotonNetwork.Instantiate("Player4", spawns[3].position, Quaternion.identity);
                break;
        }
    }
}
