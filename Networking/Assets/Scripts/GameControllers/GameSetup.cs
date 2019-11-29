using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    public Transform[] spawnPoints;

    public Text playerHealth;
    public Image playerHealthBar;

    public GameObject escButtons;

    public Image reloading;
    public TextMeshProUGUI bulletsLeft;
    public TextMeshProUGUI bullets;
    public GameObject shoppingText;

    private void OnEnable()
    {
        if(GS == null)
        {
            GS = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ShopController.SC.isShopping == false)
        {
            if (escButtons.activeSelf == false)
            {
                Cursor.visible = true;
                escButtons.SetActive(true);
            }
            else
            {
                Cursor.visible = false;
                escButtons.SetActive(false);
            }
        }
    }

    public void OnButtonQuit()
    {
        Application.Quit();
    }
}
