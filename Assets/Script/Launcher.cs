using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;


public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text ErrorMessage;
    [SerializeField] Transform roomListContend;
    [SerializeField] GameObject roomItemPrefab;
    [SerializeField] GameObject BotonStart;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerItemprefab;
    public static Launcher Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        Debug.Log("Conectando");
        PhotonNetwork.ConnectUsingSettings();
        MenuManager.Instance.OpenMenuName("Loading");
        
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectando");
        PhotonNetwork.JoinLobby();
        MenuManager.Instance.OpenMenuName("Home");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectando al lobby");
        MenuManager.Instance.OpenMenuName("Home");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
       
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);

        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenuName("Room");
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        foreach(Transform playerT in PlayerListContent)
        {
            Destroy(playerT.gameObject);
        }
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i <players.Count(); i++)
        {
            Instantiate(PlayerItemprefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        
         BotonStart.SetActive(PhotonNetwork.IsMasterClient);
     
    
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ErrorMessage.text = "Error al crear la sala" + message;
        MenuManager.Instance.OpenMenuName("Error");
    }

    public void JoinRoom(RoomInfo _info)
    {
        PhotonNetwork.JoinRoom(_info.Name);
        MenuManager.Instance.OpenMenuName("Loading");
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenuName("Home");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform transform in roomListContend)
        {
            Destroy(transform.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomItemPrefab, roomListContend).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        BotonStart.SetActive(PhotonNetwork.IsMasterClient);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerItemprefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


}
