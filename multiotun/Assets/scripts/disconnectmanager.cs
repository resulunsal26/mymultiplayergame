using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class disconnectmanager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject canvas, menubtn, reconnectbtn;
    [SerializeField] private Text statustxt;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
   
    void Update()
    {
        if(Application.internetReachability==NetworkReachability.NotReachable)
        {
            canvas.SetActive(true);
            if(SceneManager.GetActiveScene().buildIndex==0)
            {
                reconnectbtn.SetActive(true);
                statustxt.text = "Lost connection to photon ,please try to reconnect";
            }
             if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                menubtn.SetActive(true);
                statustxt.text = "Lost connection to photon ,please try to reconnect in the main menu ";

            }
        }
    }
    public void onclicktryconnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void onclickmenu()
    {
        PhotonNetwork.LoadLevel(0);
    }
    public override void OnConnectedToMaster()
    {
        if(canvas.activeSelf)
        {
            canvas.SetActive(false);
            reconnectbtn.SetActive(false);
            menubtn.SetActive(false);

        }
    }
}
