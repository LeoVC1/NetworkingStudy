using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom room;
    private PhotonView PV;
    public int multiplayerScene;
    private int currentScene;
    public Text roomName;

    private void Awake()
    {
        if (room == null)
        {
            room = this;
        }
        else
        {
            if (room != this)
            {
                Destroy(room.gameObject);
                room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinisedLoading;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinisedLoading;
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Has joined room");
        PhotonLobby.lobby.nickName.gameObject.SetActive(false);
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        if (!PhotonNetwork.IsMasterClient)
            return;
        StartGame();
    }
    void StartGame()
    {
        Debug.Log("Loading Level");
        PhotonNetwork.LoadLevel(multiplayerScene);
    }
    void OnSceneFinisedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == multiplayerScene)
        {
            CreatePlayer();
        }
    }
    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
    

}