using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;

    public int mySelectedCharacter;

    public GameObject[] exampleModel;

    private int previousExample = 0;

    public GameObject[] allCharacters;

    public Material[] prefabMaterials;

    public string myNick;

    private void OnEnable()
    {
        if(PI == null)
        {
            PI = this;
        }
        else
        {
            if(PI != this)
            {
                Destroy(PI.gameObject);
                PI = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MyCharacter"))
        {
            mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");
        }
        else
        {
            mySelectedCharacter = 0;
            PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);
        }
    }
    private void Update()
    {
        myNick = PhotonLobby.lobby.nickName.text;
    }

    public void ChangeExample(int value)
    {
        exampleModel[previousExample].SetActive(false);
        exampleModel[value].SetActive(true);
    }
}
