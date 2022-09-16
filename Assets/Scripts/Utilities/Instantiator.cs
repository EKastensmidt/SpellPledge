using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Instantiator : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    void Start()
    {
        PhotonNetwork.Instantiate("Player", spawn.position, Quaternion.identity);
    }
}
