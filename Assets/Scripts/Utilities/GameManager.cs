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
    [SerializeField] private TextMeshProUGUI startCountdownText, timeText;
    [SerializeField] private GameObject loseText, winText, waitingForHostText;
    [SerializeField] private Button MasterStartButton;

    private CameraController cameraController;

    private List<Player> playerList;

    private bool isGameStarted = false;
    private float currentTime = 0f;
    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }

    void Start()
    {
        cameraController = GameObject.FindObjectOfType<CameraController>();
        SetStartRequirements();
        playerList = new List<Player>();
        
    }

    void Update()
    {
        SetTime();
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
        startCountdownText.gameObject.SetActive(true);
        startCountdownText.text = "GAME STARTING: 3";
        yield return new WaitForSeconds(1f);
        startCountdownText.text = "GAME STARTING: 2";
        yield return new WaitForSeconds(1f);
        startCountdownText.text = "GAME STARTING: 1";
        yield return new WaitForSeconds(1f);
        startCountdownText.gameObject.SetActive(false);
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

    private int minutes, seconds;

    private void SetTime()
    {
        if (!IsGameStarted) return;
        if (PhotonNetwork.IsMasterClient == false) return;

        currentTime += Time.deltaTime;
        minutes = (int)(currentTime / 60f);
        seconds = (int)(currentTime - minutes * 60f);

        pv.RPC("UpdateTime", RpcTarget.All, minutes, seconds);
    }

    [PunRPC]
    public void UpdateTime(int minutes, int seconds)
    {
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
