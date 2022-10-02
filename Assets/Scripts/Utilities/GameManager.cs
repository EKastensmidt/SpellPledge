using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameManager : MonoBehaviour
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

    public void WinScreen()
    {
        winText.SetActive(true);
    }

    public void LoseScreen()
    {
        cameraController.SetColorGradingOnOff(true);
        loseText.SetActive(true);
    }

    private void UpdatePing()
    {
        ping.text = PhotonNetwork.GetPing().ToString();
    }
}
