using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private TextMeshProUGUI ping;
    [SerializeField] private GameObject loseText, winText;
    private CameraController cameraController;
    void Start()
    {
        cameraController = GameObject.FindObjectOfType<CameraController>();
    }

    void Update()
    {
        UpdatePing();
    }

    public void SetLoser(Player character)
    {
        var player = character.PV.Owner;
        pv.RPC("UpdateLoser", RpcTarget.All, player);
    }

    [PunRPC]
    public void UpdateLoser(Photon.Realtime.Player client)
    {
        if (PhotonNetwork.LocalPlayer == client)
        {
            loseText.SetActive(true);
            cameraController.SetColorGradingOnOff(true);
        }
    }

    private void UpdatePing()
    {
        ping.text = PhotonNetwork.GetPing().ToString();
    }
}
