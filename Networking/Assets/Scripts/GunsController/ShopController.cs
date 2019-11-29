using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ShopController : MonoBehaviour
{
    public GameObject[] guns;
    public TextMeshProUGUI[] numberInfo;
    public Image[] barInfo;
    public Image gunIcon;
    public Sprite[] gunsImages;

    public GameObject gunInfoPanel;
    public GameObject panel;

    public static ShopController SC;

    public bool isShopping;

    private void Start()
    {
        if(SC == null)
        {
            SC = this;
        }
    }

    public void ShowGunInfo(int whichGun)
    {
        GunInfo gun = guns[whichGun].GetComponent<GunInfo>();

        numberInfo[0].text = gun.damage.ToString();
        numberInfo[1].text = gun.bulletsCapacity.ToString();
        numberInfo[2].text = gun.fireRate.ToString();
        numberInfo[3].text = (gun.reloadTime * 20).ToString();

        barInfo[0].fillAmount = gun.damage / 100;
        barInfo[1].fillAmount = gun.bulletsCapacity / 100;
        barInfo[2].fillAmount = gun.fireRate / 100;
        barInfo[3].fillAmount = (gun.reloadTime * 20) / 100;

        gunIcon.overrideSprite = gunsImages[whichGun];
        gunInfoPanel.SetActive(true);
    }

    public void HideGunInfo()
    {
        gunInfoPanel.SetActive(false);
    }

    public void OnMouseOver(int whichGun)
    {
        SC.ShowGunInfo(whichGun);
    }

    public void OnMouseGetOut()
    {
        SC.HideGunInfo();
    }

    public void OnGunClick(int whichGun)
    {
        GunsSetup.GunsS.BuyGuns(whichGun);
        Cursor.visible = false;
        isShopping = false;
        SC.panel.SetActive(false);
        gunInfoPanel.SetActive(false);
    }

}
