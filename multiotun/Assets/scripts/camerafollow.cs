using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public float speed = 25f;
    public float intervelocity;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetposition;
    public Vector2 minboundary=new Vector2(-2.65f,0);
    public Vector2 maxboundary = new Vector2(3.88f, 0.8f);

    void Start()
    {
        targetposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            Vector3 posz = transform.position;
            posz.z = target.transform.position.z;
            Vector3 targetdirection = (target.transform.position - posz);
            intervelocity = targetdirection.magnitude * speed;
            targetposition = transform.position + (targetdirection.normalized * intervelocity * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position,targetposition+offset,0.25f);
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minboundary.x, minboundary.x), 
                Mathf.Clamp(transform.position.y, minboundary.y, minboundary.y), 
                transform.position.z
                );
        }
    }
}
