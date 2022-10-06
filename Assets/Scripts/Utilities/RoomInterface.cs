using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomInterface : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI ping, text;
    [SerializeField] private GameObject projectileCD, shotgunCD;

    private TextMeshProUGUI projectileCountdown, shotgunCountdown;
    private bool isProjectileOnCoolDown = false, isShotGunOnCooldown = false;

    public bool IsProjectileOnCoolDown { get => isProjectileOnCoolDown; set => isProjectileOnCoolDown = value; }
    public bool IsShotGunOnCooldown { get => isShotGunOnCooldown; set => isShotGunOnCooldown = value; }

    private void Start()
    {
        UpdateInterface();
        setSkillCD();
    }
    private void Update()
    {
        UpdatePing();
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

    private void UpdatePing()
    {
        ping.text = PhotonNetwork.GetPing().ToString();
    }

    private void setSkillCD()
    {
        projectileCountdown = projectileCD.GetComponentInChildren<TextMeshProUGUI>();
        shotgunCountdown = shotgunCD.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void IsSkillOnCD(string skillName, bool cdCheck)
    {
        switch (skillName)
        {
            case "Projectile":
                isProjectileOnCoolDown = cdCheck;
                break;
            case "Shotgun":
                isShotGunOnCooldown = cdCheck;
                break;
        }
    }

    public void UpdateSkillUI(float projectileCooldown, float shotgunCooldown)
    {
        //projectile
        if(isProjectileOnCoolDown)
        {
            projectileCD.SetActive(true);
            projectileCountdown.text = ChangeTimeDisplay(projectileCooldown);
        }
        else
        {
            projectileCD.SetActive(false);
        }

        //shotgun
        if (isShotGunOnCooldown)
        {
            shotgunCD.SetActive(true);
            shotgunCountdown.text = ChangeTimeDisplay(shotgunCooldown);
        }
        else
        {
            shotgunCD.SetActive(false);
        }
    }

    private string ChangeTimeDisplay(float currentTime)
    {
        int minutes = (int)(currentTime / 60f);
        int seconds = (int)(currentTime - minutes * 60f);
        int cents = (int)((currentTime - (int)currentTime) * 100f);
        return string.Format("{0:00}:{1:00}", seconds, cents);
    }
}
