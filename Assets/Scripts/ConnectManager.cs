using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    public static ConnectManager instance;
    public static GameObject localPlayer;
    string gameVersion = "1";

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogErrorFormat(gameObject, "Multiple instance of {0} is not allowed", GetType().Name);
            Destroy(gameObject);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnConnected()
    {
        Debug.Log("PUN Connected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Connected to Master");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Disconnected was called by PUN with reason {0}", cause);

    }

    public void JoinGameRoom()
    {
        var option = new RoomOptions
        {
            MaxPlayers = 6
        };

        PhotonNetwork.JoinOrCreateRoom("Kindom", option, null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Create Room!!");
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            Debug.Log("Joined Room!!");

        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("Joined Room failed!! {0}", message);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(!PhotonNetwork.InRoom)
        {
            return;
        }

        localPlayer = PhotonNetwork.Instantiate("TankPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
        Debug.Log("Player Instance ID: " + localPlayer.GetInstanceID());
    }
}
