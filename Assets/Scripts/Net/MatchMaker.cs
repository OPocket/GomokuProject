using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MatchMaker : MonoBehaviour {

    private NetworkManager manager;
    public string roomName;
    private List<GameObject> roomBars = new List<GameObject>();
    [SerializeField]
    private GameObject barPrefab;
    [SerializeField]
    private Transform barParent;

    private void Start()
    {
        if (!manager)
        {
            manager = NetworkManager.singleton;
            if (manager.matchMaker == null)
            {
                manager.StartMatchMaker();
            }
        }
        // 初始化房间列表
        OnRefreshBtnClick();
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }

    public void OnCreateRoomBtnClick()
    {
        manager.matchMaker.CreateMatch(roomName,3,true,"","","",0,0,manager.OnMatchCreate);
    }

    public void OnRefreshBtnClick()
    {
        manager.matchMaker.ListMatches(0,10,"",true,0,0,OnMatchList);
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData)
    {
        if (!success)
        {
            Debug.Log("请求列表失败");
        }
        else
        {
            ClearRoomList();
            foreach (MatchInfoSnapshot temData in responseData)
            {
                GameObject temBar = Instantiate(barPrefab, barParent);
                RoomBarItem temItem = temBar.GetComponent<RoomBarItem>();
                temItem.SetBar(temData);
                roomBars.Add(temBar);
            }
        }
    }

    private void ClearRoomList()
    {
        for (int i = 0; i < roomBars.Count; i++)
        {
            Destroy(roomBars[i]);
        }
        roomBars.Clear();
    }
}
