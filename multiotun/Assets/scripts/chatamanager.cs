using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;



public class chatamanager : MonoBehaviourPun, IPunObservable
{
    public PhotonView photonview;
    public GameObject bubblespech;
    public Text chattext;
    public cowboy player;
    InputField chatınput;
    private bool dissablesend;
    private void Awake()
    {
        chatınput = GameObject.Find("InputField").GetComponent<InputField>();
    }


    void Update()
    {
        if (photonview.IsMine)
        {
            if (chatınput.isFocused)
            {
                player.dissableinput = true;
            }
            else
            {
                player.dissableinput = false;
            }
            if (!dissablesend && chatınput.isFocused)
            {
                if (chatınput.text != "" && chatınput.text.Length > 1 && Input.GetKeyDown(KeyCode.Space))
                {
                    photonview.RPC("sendmessage", RpcTarget.AllBuffered, chatınput.text);
                    bubblespech.SetActive(true);
                    chatınput.text = "";
                    dissablesend = true;
                }
            }
        }
    }
    [PunRPC]
    void sendmessage(string msg)
    {
        chattext.text = msg;
        StartCoroutine(hidebubblespech());
    }
    IEnumerator hidebubblespech()
    {
        yield return new WaitForSeconds(3f);
        dissablesend = false;
        bubblespech.SetActive(false);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(bubblespech.activeSelf);
        }
        else if (stream.IsReading)
        {
            bubblespech.SetActive((bool)stream.ReceiveNext());
        }
    }
    
    

}
