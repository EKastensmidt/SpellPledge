using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NewBehaviourScript : MonoBehaviour
{
    private void Start()
    {
        string nameRoom = PhotonNetwork.CurrentRoom.Name;
    }
}
