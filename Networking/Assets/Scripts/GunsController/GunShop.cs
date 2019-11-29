using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunShop : MonoBehaviour
{
    public PhotonView PV;

    private void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.B) && PlayerRespawn.PR.dead == false)
            {
                if(ShopController.SC.panel.activeSelf == true)
                {
                    ShopController.SC.isShopping = false;
                    ShopController.SC.panel.SetActive(false);
                    Cursor.visible = false;
                }
                else
                {
                    GameSetup.GS.shoppingText.SetActive(false);
                    ShopController.SC.isShopping = true;
                    Cursor.visible = true;
                    ShopController.SC.panel.SetActive(true);
                }
            }
        }
    }

}
