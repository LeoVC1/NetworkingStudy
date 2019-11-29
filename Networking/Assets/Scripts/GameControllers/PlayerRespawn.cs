using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRespawn : MonoBehaviour
{

    PhotonView PV;
    AvatarSetup avatar;
    AvatarCombat avatarCombat;
    public GunsSetup guns;

    public static PlayerRespawn PR;
    public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            return;
        }
        if(PR == null)
        {
            PR = this;
        }
        avatar = GetComponent<AvatarSetup>();
        avatarCombat = GetComponent<AvatarCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        if(avatar.health <= 0 && dead == false)
        {
            guns.myGunInfo.bullets = 0;
            guns.myGunInfo.bulletsLeft = 0;
            Destroy(guns.myGunVisible);
            dead = true;
            avatarCombat.gunState = GunStateMachine.NONE;
            PV.RPC("RPC_Death", RpcTarget.All, PV.ViewID);
            StartCoroutine("Respawn");
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        avatar.health = 100;
        dead = false;
        PV.RPC("RPC_Respawn", RpcTarget.All, PV.ViewID);
    }

    [PunRPC]
    void RPC_Death(int viewId)
    {
        GameObject avatarGameObjectRPC = PhotonView.Find(viewId).gameObject;
        Destroy(avatarGameObjectRPC.GetComponent<PlayerRespawn>().guns.myGun);
        avatarGameObjectRPC.GetComponent<PlayerRespawn>().avatar.myCharacter.SetActive(false);
        avatarGameObjectRPC.GetComponent<BoxCollider>().enabled = false;
        avatarGameObjectRPC.GetComponent<Rigidbody>().useGravity = false;
    }

    [PunRPC]
    void RPC_Respawn(int viewId)
    {
        GameObject avatarGameObjectRPC = PhotonView.Find(viewId).gameObject; 
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        avatarGameObjectRPC.transform.SetPositionAndRotation(GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation);
        avatarGameObjectRPC.GetComponent<PlayerRespawn>().avatar.myCharacter.SetActive(true);
        avatarGameObjectRPC.GetComponent<BoxCollider>().enabled = true;
        avatarGameObjectRPC.GetComponent<Rigidbody>().useGravity = true;
    }
}
