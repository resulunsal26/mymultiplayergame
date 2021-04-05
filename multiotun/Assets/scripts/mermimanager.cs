using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class mermimanager : MonoBehaviourPun
{
    public bool movingdirection;
    public float movespeed=8f;
    public float bulletdamage=0.3f;
    public string killername;
    public GameObject localplayerobje;
   
    void Start()
    {
        if(photonView.IsMine)
        {
            killername = localplayerobje.GetComponent<cowboy>().myname;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!movingdirection)
        {
            transform.Translate(Vector2.right * movespeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * movespeed * Time.deltaTime);
        }
    }
    [PunRPC]
    public void changedirection()
    {
        movingdirection = true;
    }
    [PunRPC]
    void destroy()
    {
        Destroy(this.gameObject); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!photonView.IsMine)
        {
            return;
        }
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();
        if(target!=null && (!target.IsMine))
        {
            if(target.tag=="Player")
            {
                target.RPC("healthupdate", RpcTarget.AllBuffered, bulletdamage);
                if(target.GetComponent<health>().playerhealth <=0)
                {
                    Player gotkilled = target.Owner;
                    target.RPC("YouGotKilledBy", gotkilled, killername);
                    target.RPC("YouKilled", localplayerobje.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                }
            }
            GetComponent<PhotonView>().RPC("destroy",RpcTarget.AllBuffered);
        }
    }
}
