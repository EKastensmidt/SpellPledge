using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomInterface : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI ping, text;
    [SerializeField] private GameObject projectileCD, shotgunCD, blinkCD;

    private TextMeshProUGUI projectileCountdown, shotgunCountdown, blinkCountdown;
    private bool isProjectileOnCoolDown = false, isShotGunOnCooldown = false, isBlinkOnCooldown = false;

    public bool IsProjectileOnCoolDown { get => isProjectileOnCoolDown; set => isProjectileOnCoolDown = value; }
    public bool IsShotGunOnCooldown { get => isShotGunOnCooldown; set => isShotGunOnCooldown = value; }
    public bool IsBlinkOnCooldown { get => isBlinkOnCooldown; set => isBlinkOnCooldown = value; }

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
        blinkCountdown = blinkCD.GetComponentInChildren<TextMeshProUGUI>();
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
            case "Blink":
                isBlinkOnCooldown = cdCheck;
                break;
        }
    }

    public void UpdateSkillUI(float projectileCooldown, float shotgunCooldown, float blinkCooldown)
    {
        //Projectile
        if(isProjectileOnCoolDown)
        {
            projectileCD.SetActive(true);
            projectileCountdown.text = ChangeTimeDisplay(projectileCooldown);
        }
        else
        {
            projectileCD.SetActive(false);
        }

        //Shotgun
        if (isShotGunOnCooldown)
        {
            shotgunCD.SetActive(true);
            shotgunCountdown.text = ChangeTimeDisplay(shotgunCooldown);
        }
        else
        {
            shotgunCD.SetActive(false);
        }

        //Blink
        if (IsBlinkOnCooldown)
        {
            blinkCD.SetActive(true);
            blinkCountdown.text = ChangeTimeDisplay(blinkCooldown);
        }
        else
        {
            blinkCD.SetActive(false);
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
