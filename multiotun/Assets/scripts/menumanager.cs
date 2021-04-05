using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class menumanager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject usernamescreen, connectscreen,createusernamebtn;
    [SerializeField] private InputField usernameinput,createroomintput,joinroominputfield;
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
         
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public override void OnJoinedLobby()
    {
       if(!connectscreen.activeSelf)
        {
            usernamescreen.SetActive(true);
        }
        Debug.Log("connected");
       
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void onclicknamebtn()
    {
        PhotonNetwork.NickName = usernameinput.text;
        usernamescreen.SetActive(false);
        connectscreen.SetActive(true);
    }
    public void Onnamefieldchange()
    {
        if(usernameinput.text.Length>=4)
        {
            createusernamebtn.SetActive(true);
        }
        else
        {
            createusernamebtn.SetActive(false);
        }
    }
    public void onclickjoinroom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(joinroominputfield.text, ro, TypedLobby.Default);
    }
    public void onclickcreateroom()
    {
        PhotonNetwork.CreateRoom(createroomintput.text, new RoomOptions { MaxPlayers = 4 },null);
    }
}
