using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tanks
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        static MainMenu instance;
        private GameObject mUI;
        private Button mJoinGameButton;

        void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

            mUI = transform.FindAnyChild<Transform>("UI").gameObject;
            mJoinGameButton = transform.FindAnyChild<Button>("JoinGameButton");

            mUI.SetActive(true);
            mJoinGameButton.interactable = false;
;       }

        public override void OnConnectedToMaster()
        {
            mJoinGameButton.interactable = true;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            mUI.SetActive(!PhotonNetwork.InRoom);
        }
    }
}