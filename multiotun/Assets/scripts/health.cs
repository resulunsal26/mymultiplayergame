using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class health : MonoBehaviourPun
{
    public Image fillimage;
    public float playerhealth = 1;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D collider;
    public GameObject playercanvas;
    public cowboy playerscript;
    public GameObject killgotkilledtext;
    public void checkhealth()
    {
        if(photonView.IsMine && playerhealth<=0)
        {
            gamemanager.instance.enablerespawn();
            playerscript.dissableinput = true;
            GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    public void death()
    {
        rb.gravityScale = 0;
        collider.enabled = false;
        sr.enabled = false;
        playercanvas.SetActive(false);
    }
    [PunRPC]
    public void healthupdate(float damage)
    {
        fillimage.fillAmount -= damage;
        playerhealth = fillimage.fillAmount;
        checkhealth();
    }
    [PunRPC]
    public void review()
    {
        rb.gravityScale = 1;
        collider.enabled = true;
        sr.enabled = true;
        playercanvas.SetActive(true);
        fillimage.fillAmount = 1;
        playerhealth = 1;
    }
    public void enableinputs()
    {
        playerscript.dissableinput = false;
    }
    [PunRPC]
    public void YouGotKilledBy(string name)
    {
        GameObject go = Instantiate(killgotkilledtext, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(gamemanager.instance.killgotkilledfeedbox.transform, false);
        go.GetComponent<Text>().text = "You Got Killed by :" + name;
        go.GetComponent<Text>().color =Color.red;
    }
    [PunRPC]
    public void YouKilled(string name)
    {
        GameObject go = Instantiate(killgotkilledtext, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(gamemanager.instance.killgotkilledfeedbox.transform, false);
        go.GetComponent<Text>().text = "You Kill : " + name;
        go.GetComponent<Text>().color = Color.green;
    }

}
