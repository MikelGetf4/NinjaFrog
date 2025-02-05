using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rig;
    public float jumpForce;
    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            rig = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            Camera.main.transform.SetParent(transform);
            Camera.main.transform.position = transform.position + (Vector3.up) + (transform.forward*-10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().IsMine) {
            rig.velocity = (transform.right * speed * Input.GetAxis("Horizontal")) + (transform.up * rig.velocity.y);

            if(rig.velocity.x > 0.1f && GetComponent<SpriteRenderer>().flipX)
            {
                GetComponent<PhotonView>().RPC("RotateSprite", RpcTarget.All, false);
            }
            else if (rig.velocity.x < 0.1f && GetComponent<SpriteRenderer>().flipX)
            {
                GetComponent<PhotonView>().RPC("RotateSprite", RpcTarget.All, true);
            }


            if (Input.GetButtonDown("Jump"))
            {
                rig.AddForce(transform.up * jumpForce);
            }

            if (rig.velocity.x > 0.1f)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (rig.velocity.x < 0.1f)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }


            anim.SetFloat("VelocityX", Mathf.Abs(rig.velocity.x));
            anim.SetFloat("VelocityY", rig.velocity.y);
        }
    }
    [PunRPC]
    public void RotateSprite(bool rotate)
    {
        GetComponent<SpriteRenderer>().flipX = rotate;
    }
}
