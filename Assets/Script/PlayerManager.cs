using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using JetBrains.Annotations;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    public PhotonView pv;


    public void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public void Start()
    {
        if (pv.IsMine) 
        {
            CreatePlayerController();
        
        }

    }
    public void CreatePlayerController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }

}
