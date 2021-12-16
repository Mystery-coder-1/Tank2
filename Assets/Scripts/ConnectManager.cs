using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Tanks;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    public static ConnectManager instance;
    public static GameObject localPlayer;
    string gameVersion = "1";
    private GameObject defaultSpawnPoint;

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

        defaultSpawnPoint = new GameObject("Defalut SpawnPoint");
        defaultSpawnPoint.transform.position = new Vector3(0, 0, 0);
        defaultSpawnPoint.transform.SetParent(transform, false);
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

        var spawnPoint = GetRandomSpawnPoint();

        localPlayer = PhotonNetwork.Instantiate("TankPlayer", spawnPoint.position, spawnPoint.rotation, 0);
        Debug.Log("Player Instance ID: " + localPlayer.GetInstanceID());
    }

    public static List<GameObject> GetAllObjectsOfTypeInScene<T>()
    {
        var objectsInScene = new List<GameObject>();
        foreach (var go in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            if (go.hideFlags == HideFlags.NotEditable ||
                go.hideFlags == HideFlags.HideAndDontSave)
                continue;
            if (go.GetComponent<T>() != null)
                objectsInScene.Add(go);
        }
        return objectsInScene;
    }

    private Transform GetRandomSpawnPoint()
    {
        var spawnPoints = GetAllObjectsOfTypeInScene<SpawnPoint>();
        return spawnPoints.Count == 0
        ? defaultSpawnPoint.transform
        : spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
    }

}
