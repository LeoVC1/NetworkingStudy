using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GunStateMachine
{
    SHOOTING,
    IDLE,
    NONE,
    RELOADING,
}
public class AvatarCombat : MonoBehaviour
{

    public GunStateMachine gunState;
    PhotonView PV;
    AvatarSetup avatarSetup;

    public Transform rayOrigin;

    public Text healthDisplay;
    public Image healthBarDisplay;

    public Image reloading;
    public TextMeshProUGUI bulletsLeft;
    public TextMeshProUGUI bullets;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            return;
        }
        healthDisplay = GameSetup.GS.playerHealth;
        healthBarDisplay = GameSetup.GS.playerHealthBar;
        avatarSetup = GetComponent<AvatarSetup>();
        gunState = GunStateMachine.NONE;
        reloading = GameSetup.GS.reloading;
        bulletsLeft = GameSetup.GS.bulletsLeft;
        bullets = GameSetup.GS.bullets;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        if(PlayerRespawn.PR.dead == true)
        {
            bulletsLeft.text = "";
            bullets.text = "";
        }
        else
        {
            //bulletsLeft.text = GunsSetup.GunsS.myGunInfo.bulletsLeft.ToString();
           // bullets.text = "/" + GunsSetup.GunsS.myGunInfo.bullets.ToString();
        }
        healthDisplay.text = "HP: " + avatarSetup.health.ToString() + "%";
        healthBarDisplay.fillAmount = avatarSetup.health / 100f;
        switch (gunState)
        {
            case GunStateMachine.IDLE:
                StartCoroutine("GunStatement");
                break;
            case GunStateMachine.SHOOTING:

                break;
            case GunStateMachine.NONE:
                StopCoroutine("GunStatement");
                StopCoroutine("GunReloading");
                break;
            case GunStateMachine.RELOADING:
                break;
        }
    }

    [PunRPC]
    void RPC_Shooting(float damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.transform.tag == "Player")
            {
                hit.transform.gameObject.GetComponent<AvatarSetup>().health -= damage;
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
    }

    public void AssignGun()
    {
        if (PV.IsMine)
        {
            gunState = GunStateMachine.IDLE;

        }
    }

    IEnumerator GunStatement()
    {
        if (PV.IsMine)
        {
            gunState = GunStateMachine.SHOOTING;
            while (GunsSetup.GunsS.myGunInfo.bulletsLeft > 0 && ShopController.SC.isShopping == false)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    StartCoroutine("GunReloading");
                    StopCoroutine("GunStatement");
                }
                if (Input.GetMouseButton(0))
                {
                    PV.RPC("RPC_Shooting", RpcTarget.All, GunsSetup.GunsS.myGunInfo.damage);
                    GunsSetup.GunsS.myGunInfo.bulletsLeft--;
                    bulletsLeft.text = GunsSetup.GunsS.myGunInfo.bulletsLeft.ToString();
                    bullets.text = "/" + GunsSetup.GunsS.myGunInfo.bullets.ToString();
                    GunsSetup.GunsS.myGun.GetComponent<Animation>().Play("Shoot");
                    yield return new WaitForSeconds(GunsSetup.GunsS.myGunInfo.delay);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    gunState = GunStateMachine.IDLE;
                    StopCoroutine("GunStatement");
                    yield return null;
                }
                yield return null;
            }
            if(ShopController.SC.isShopping == false)
            {
                StartCoroutine("GunReloading");
            }
            yield return new WaitForSeconds(GunsSetup.GunsS.myGunInfo.reloadTime);
        }
        
    }

    IEnumerator GunReloading()
    {
        if (PV.IsMine)
        {
            gunState = GunStateMachine.RELOADING;
            while (reloading.fillAmount < 1)
            {
                reloading.fillAmount += 0.1f;
                yield return new WaitForSeconds(GunsSetup.GunsS.myGunInfo.reloadTime / 10f);
            }
            GunsSetup.GunsS.myGunInfo.bullets += GunsSetup.GunsS.myGunInfo.bulletsLeft;
            if (GunsSetup.GunsS.myGunInfo.bullets < GunsSetup.GunsS.myGunInfo.bulletsCapacity)
            {
                GunsSetup.GunsS.myGunInfo.bulletsLeft = GunsSetup.GunsS.myGunInfo.bullets;
                GunsSetup.GunsS.myGunInfo.bullets = 0;
            }
            else
            {
                GunsSetup.GunsS.myGunInfo.bulletsLeft = GunsSetup.GunsS.myGunInfo.bulletsCapacity;
                GunsSetup.GunsS.myGunInfo.bullets -= GunsSetup.GunsS.myGunInfo.bulletsCapacity;
            }
            reloading.fillAmount = 0;
            gunState = GunStateMachine.IDLE;
            StopCoroutine("GunReloading");
        }
    }
}
