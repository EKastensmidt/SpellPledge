using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private TextMeshProUGUI ping;
    void Start()
    {
        
    }

    void Update()
    {
        UpdatePing();
    }

    private void UpdatePing()
    {
        ping.text = PhotonNetwork.GetPing().ToString();
    }
}
