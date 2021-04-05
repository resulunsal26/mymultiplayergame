using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class cowboy : MonoBehaviourPun
{
    public float movespeed = 5f;
    public GameObject playercam;
    public SpriteRenderer sprite;
    public PhotonView photonView;
    public Animator anim;
    private bool Allowmoving=true;
    public GameObject BulletPrefab;
    public Transform Bulletspawnpointright;
    public Transform Bulletspawnpointleft;
    public Text playername;
    public bool isground;
    public bool dissableinput = false;
    private Rigidbody2D rb;
    [SerializeField]
    private float jumpforce=20;
    public string myname;
    private void Awake()
    {
       
        if (photonView.IsMine)
        {
            gamemanager.instance.localplayer = this.gameObject;
            playercam.SetActive(true);
            playercam.transform.SetParent(null, false);
            playername.text = "You : " + PhotonNetwork.NickName;
            playername.color = Color.green;
            myname = PhotonNetwork.NickName;
        }
        else
        {
            playername.text = photonView.Owner.NickName;
            playername.color = Color.red;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine && !dissableinput)
        {
            checkinputs();
        }
    }
    private void checkinputs()
    {
        if(Allowmoving)
        {
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            transform.position += movement*movespeed*Time.deltaTime;

        }
        if(Input.GetKeyDown(KeyCode.RightControl) && anim.GetBool("IsMove")==false)
        {
           shot();
        }
        else if(Input.GetKeyUp(KeyCode.RightControl))
        {
            anim.SetBool("IsShot", false);
            Allowmoving = true;
        }

        if(Input.GetKeyDown(KeyCode.Space) && isground)
        {
           jump();
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            anim.SetBool("IsMove", true);
        }
        if(Input.GetKeyDown(KeyCode.D)&& anim.GetBool("IsShot")==false)
        {
            playercam.GetComponent<camerafollow>().offset = new Vector3(1.3f, 1.53f,0);
            photonView.RPC("flipsprite_right",RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }
        if (Input.GetKeyDown(KeyCode.A) && anim.GetBool("IsShot") == false)
        {
            playercam.GetComponent<camerafollow>().offset = new Vector3(-1.3f, 1.53f, 0);
            photonView.RPC("flipsprite_left", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }
    }
    [PunRPC]
    private void flipsprite_right()
    {
        sprite.flipX = false;
    }
    [PunRPC]
    private void flipsprite_left()
    {
        sprite.flipX = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "ground")
        {
            isground = true;
            Debug.Log(collision.gameObject.name);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isground = false;

        }
    }

    void jump()
    {
        Vector2 jumpvelocity = new Vector2(0, jumpforce);
        rb.velocity += jumpvelocity;
    }
    private void shot()
    {
        if(sprite.flipX==false)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletPrefab.name,
                new Vector2(Bulletspawnpointright.position.x, Bulletspawnpointright.position.y), Quaternion.identity, 0);
            bullete.GetComponent<mermimanager>().localplayerobje = this.gameObject;

        }
        if (sprite.flipX == true)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletPrefab.name,
               new Vector2(Bulletspawnpointleft.position.x, Bulletspawnpointleft.position.y), Quaternion.identity, 0);
            bullete.GetComponent<mermimanager>().localplayerobje = this.gameObject;
            bullete.GetComponent<mermimanager>().photonView.RPC("changedirection", RpcTarget.AllBuffered);
        }
        anim.SetBool("IsShot", true);
        Allowmoving = false;
    }
   
}
