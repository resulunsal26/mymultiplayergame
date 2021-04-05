using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class gamemanager : MonoBehaviourPunCallbacks
{
    public GameObject playerprefab;
    public GameObject canvas;
    public GameObject scenecam;
    public static gamemanager instance = null;
    private float timeamount;
    public GameObject respawnuı;
    public Text spawntimer;
    private bool startspawn;
    [HideInInspector]
    public GameObject localplayer;
    public GameObject killgotkilledfeedbox;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(startspawn)
        {
            Startspawn();
        }
    }
    public void spawnplayer()
    {
        float randomvalue = Random.Range(-5f, 5f);
        PhotonNetwork.Instantiate(playerprefab.name, new Vector2(playerprefab.transform.position.x * randomvalue, playerprefab.transform.position.y), Quaternion.identity,0);
        canvas.SetActive(false);
        scenecam.SetActive(false);
    }
          
    public void enablerespawn()
    {
        timeamount = 5;
        startspawn = true;
        respawnuı.SetActive(true);

    }
    public void Startspawn()
    {
        timeamount -= Time.deltaTime;
        spawntimer.text ="Respawn in : "+ timeamount.ToString();
        if(timeamount<=0)
        {
            respawnuı.SetActive(false);
            startspawn = false;
            playerrelocation();
            localplayer.GetComponent<health>().enableinputs();
            localplayer.GetComponent<PhotonView>().RPC("review", RpcTarget.AllBuffered);
        }
    }
    
    public void playerrelocation()
    {
        float randomposition= Random.Range(-5f, 5f);
        localplayer.transform.localPosition = new Vector2(randomposition,2f);

    }

}
