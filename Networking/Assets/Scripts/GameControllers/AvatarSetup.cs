using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class AvatarSetup : MonoBehaviour
{
    PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;

    public float health;

    public Camera myCamera;
    public AudioListener myAL;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            AddCharacter(0, "");
            //PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharacter, PlayerInfo.PI.myNick);
        }
        else
        {
            Destroy(myCamera);
            Destroy(myAL);
        }
    }

    void AddCharacter(int whichCharacter, string myNick)
    {
        characterValue = whichCharacter;
        myCharacter = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Characters", "Bss"), transform.position - new Vector3(0, 0.5f, 0), transform.rotation);
        myCharacter.transform.parent = transform;
        animator = myCharacter.GetComponent<Animator>();
        //myCharacter.GetComponentInChildren<TextMeshPro>().text = myNick;
        //Instantiate(PlayerInfo.PI.allCharacters[whichCharacter], transform
    }
}
