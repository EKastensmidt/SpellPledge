using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomInterface : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI text;
    private void Start()
    {
        UpdateInterface();
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        string nameRoom = PhotonNetwork.CurrentRoom.Name;
        string maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        string countPlayers = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

        text.text = nameRoom + " " + countPlayers + "/" + maxPlayers;
    }

}
