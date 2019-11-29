using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class GunsSetup : MonoBehaviour
{

    public static GunsSetup GunsS;
    PhotonView PV;
    public int gunValue;
    public GameObject myGun;
    public GameObject myGunVisible;
    public GunInfo myGunInfo;
    public Transform myGunLocal;
    public AvatarCombat avatarCombat;

    public GameObject[] allGuns;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            return;
        }
        if (GunsS == null)
        {
            GunsS = this;
        }
    }

    public void BuyGuns(int whichGun)
    {
        if (!PV.IsMine)
        {
            return;
        }
        if (myGun == null)
        {
            PV.RPC("RPC_BuyGun", RpcTarget.AllBuffered, whichGun, PV.ViewID);
            myGunInfo = myGun.GetComponent<GunInfo>();
            myGunVisible = Instantiate(allGuns[whichGun], myGunLocal.transform.position, transform.rotation, myGunLocal);
        }
        else
        {
            Destroy(myGunVisible);
            PV.RPC("RPC_BuyGun", RpcTarget.AllBuffered, whichGun, PV.ViewID);
            myGunInfo = myGun.GetComponent<GunInfo>();
            myGunVisible = Instantiate(allGuns[whichGun], myGunLocal.transform.position, transform.rotation, myGunLocal);
        }
        myGun.SetActive(false);
    }

    [PunRPC]
    void RPC_BuyGun(int whichGun, int viewId)
    {
        gunValue = whichGun;
        Destroy(PhotonView.Find(viewId).gameObject.GetComponentInChildren<GunsSetup>().myGun);
        myGun = Instantiate(allGuns[whichGun], transform.position, transform.rotation, transform);
        avatarCombat.AssignGun();
    }
}