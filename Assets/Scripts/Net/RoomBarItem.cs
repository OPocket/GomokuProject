using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomBarItem : MonoBehaviour {

    private NetworkManager manager;
    public Text text;
    private MatchInfoSnapshot info;

    private void Start()
    {
        manager = NetworkManager.singleton;
        if (manager.matchMaker == null)
        {
            manager.StartMatchMaker();
        }
    }

    public void SetBar(MatchInfoSnapshot info)
    {
        this.info = info;
        text.text = info.name;
    }

    public void OnJoinRoomBtnClick()
    {
        manager.matchMaker.JoinMatch(info.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
    }
}
