using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
using UnityEngine.Rendering.PostProcessing;

public class LavaCollider : MonoBehaviourPun
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private int lavaDmg = 2;
    [SerializeField] private float lavaDmgInterval = 1f;

    [SerializeField] private PostProcessVolume postProcessVolume;
    private Bloom bloom;
    [SerializeField] private float maxBloom = 5f;
    private bool isAddingUp = true;
    [SerializeField] private float addBloomAmount = 1f;

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out bloom);
    }

    private void Update()
    {
        SetBloom();
    }

    private void SetBloom()
    {
        Debug.Log(bloom.intensity.value);
        if (isAddingUp)
        {
            if(bloom.intensity.value >= maxBloom)
            {
                isAddingUp = false;
            }
            bloom.intensity.value += addBloomAmount * Time.deltaTime;
        }
        else
        {
            if (bloom.intensity.value <= 0f)
            {
                isAddingUp = true;
            }
            bloom.intensity.value -= addBloomAmount * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pv.IsMine) return;

        Player playerHit = collision.GetComponent<Player>();
        if (playerHit)
        {
            playerHit.IsOnLava = true;
            StartCoroutine(TakingDamage(playerHit));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!pv.IsMine) return;

        Player playerHit = collision.GetComponent<Player>();
        if (playerHit)
        {
            playerHit.IsOnLava = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!pv.IsMine) return;

        Player playerHit = collision.GetComponent<Player>();
        if (playerHit)
        {
            playerHit.IsOnLava = false;
        }
    }

    public IEnumerator TakingDamage(Player player)
    {
        while (player.IsOnLava)
        {
            player.TakeDamage(lavaDmg);
            yield return new WaitForSeconds(lavaDmgInterval);
        }
        yield return null;
    }
}
