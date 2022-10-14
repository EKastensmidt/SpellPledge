using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NickInstantiator : MonoBehaviourPun
{
    public NickName nickNamePrefab;
    NickName nickName;
    //string _name;
    // Start is called before the first frame update
    private void Start()
    {
        var canvas = GameObject.Find("Canvas");
        nickName = GameObject.Instantiate<NickName>(nickNamePrefab, canvas.transform);
        nickName.SetTarget(transform);
        nickName.SetName(photonView.Owner.NickName);

        if (photonView.IsMine)
        {
            var name = photonView.Owner.NickName;
            //photonView.RPC("UpdateName", RpcTarget.AllBuffered, name);
            UpdateName(name);
        }
        else
        {
            photonView.RPC("RequestName", photonView.Owner,PhotonNetwork.LocalPlayer);
        }

        //if (string.IsNullOrEmpty(_name))
        //{
        //    nickName.SetName(_name);
        //}

        //if (photonView.IsMine)
        //{
        //    var name = photonView.Owner.NickName;
        //    photonView.RPC("UpdateName", RpcTarget.AllBuffered, name);
        //}
    }
    [PunRPC]
    public void RequestName(PhotonView client)
    {
        photonView.RPC("UpdateName", client.Owner, photonView.Owner.NickName);
    }
    [PunRPC]
    public void UpdateName(string name)
    {
        //_name = name;
        if (nickName != null)          
            nickName.SetName(name);
        
    }
}
