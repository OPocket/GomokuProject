using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetWorkingUI : MonoBehaviour {

    public void StartHost()
    {
        NetworkManager.singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.singleton.networkAddress = GameObject.Find("IpBtn").GetComponentInChildren<InputField>().text;
        NetworkManager.singleton.StartClient();
    }

    public void StopHost()
    {
        NetworkManager.singleton.StopHost();
    }

    public void OffLineSet()
    {
        //GameObject.Find("HostBtn").GetComponent<Button>().onClick.AddListener(StartHost);
        //GameObject.Find("ClientBtn").GetComponent<Button>().onClick.AddListener(StartClient);
    }

    public void OnLineSet()
    {
        //GameObject.Find("BackToMenuBtn").GetComponent<Button>().onClick.AddListener(StopHost);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case (int)GameDefine.SCENE_NUM.SCENE_LOBBY:
                OffLineSet();
                break;
            case (int)GameDefine.SCENE_NUM.SCENE_NET:
                OnLineSet();
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
