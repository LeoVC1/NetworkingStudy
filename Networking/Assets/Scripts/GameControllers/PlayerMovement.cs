using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{

    PhotonView PV;
    public float movementSpeed = 2;
    public float rotationSpeed = 50;
    Animator anim;
    GunShop shop;

    private AvatarSetup avatarSetup;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        Cursor.visible = false;
        avatarSetup = GetComponent<AvatarSetup>();
    }

    
    void Update()
    {
        if (PV.IsMine && ShopController.SC.isShopping == false)
        {
            anim = GetComponentInChildren<Animator>();
            BasicMovement();
            BasicRotation();
        }
    }

    void BasicMovement()
    {
        bool walking = false;
        if (Input.GetKey(KeyCode.W))
        {
            walking = true;
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            walking = true;
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;
        }
        //if (Input.GetKey(KeyCode.A))
        //{
        //    walking = true;
        //    transform.position -= transform.right * movementSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    walking = true;
        //    transform.position += transform.right * movementSpeed * Time.deltaTime;
        //}

        float rotation = Input.GetAxisRaw("Horizontal") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0, rotation, 0));

        if (Input.GetKeyUp(KeyCode.W))
        {
            walking = false;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            //walking = false;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            walking = false;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            //walking = false;
        }
        avatarSetup.animator.SetBool("Walking", walking);
    }

    void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0, mouseX, 0));
    }

}
