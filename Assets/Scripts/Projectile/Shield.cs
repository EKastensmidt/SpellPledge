using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Shield : MonoBehaviourPun
{
    private GameObject player;
    private PhotonView pv;
    public GameObject Player { get => player; set => player = value; }

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (!pv.IsMine) return;
        transform.position = player.transform.position;
    }
}
