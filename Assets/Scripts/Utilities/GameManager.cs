using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private TextMeshProUGUI ping;
    [SerializeField] private GameObject loseText, winText, waitingForHostText;
    [SerializeField] private Button MasterStartButton;

    [SerializeField] private float secondsToStart = 3f;
    private CameraController cameraController;

    private List<Player> playerList;

    private bool isGameStarted = false;
    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }

    void Start()
    {
        cameraController = GameObject.FindObjectOfType<CameraController>();
        SetStartRequirements();
        playerList = new List<Player>();
    }

    void Update()
    {
        UpdatePing();

        Debug.Log(playerList.Count);
    }

    private void UpdatePing()
    {
        ping.text = PhotonNetwork.GetPing().ToString();
    }

    public void SetWinner(Player character)
    {
        var player = character.PV.Owner;
        pv.RPC("UpdateWinner", RpcTarget.All, player);
    }

    public void SetLoser(Player character)
    {
        var player = character.PV.Owner;
        pv.RPC("UpdateLoser", RpcTarget.All, player);

        playerList = GameObject.FindObjectsOfType<Player>().ToList();
        if (playerList.Count == 1)
        {
            SetWinner(playerList[0]);
        }
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

    [PunRPC]
    public void UpdateWinner(Photon.Realtime.Player client)
    {
        if (PhotonNetwork.LocalPlayer == client)
        {
            winText.SetActive(true);
        }
    }

    private void SetStartRequirements()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MasterStartButton.gameObject.SetActive(true);
        }
        else
        {
            waitingForHostText.SetActive(true);
        }
    }

    public void StartGame()
    {
        pv.RPC("GameStarted", RpcTarget.All);
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(secondsToStart);
        isGameStarted = true;
        playerList = GameObject.FindObjectsOfType<Player>().ToList();
    }

    [PunRPC]
    public void GameStarted()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MasterStartButton.gameObject.SetActive(false);
        }
        else
        {
            waitingForHostText.SetActive(false);
        }
        StartCoroutine(WaitToStart());
    }
}
