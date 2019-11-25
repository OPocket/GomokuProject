using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetPlayer : NetworkBehaviour
{
    [SyncVar]
    public GameDefine.ChessType chessType = GameDefine.ChessType.Black;
    public Text text;
    private Button backBtn;


    private void Start()
    {
        text = GameObject.Find("Canvas/TemText").GetComponent<Text>();
        text.text += "  :"+isLocalPlayer;
        if (isLocalPlayer)
        {
            CmdSetPlayer();
            if (chessType != GameDefine.ChessType.Watch)
            {
                backBtn = GameObject.Find("Canvas/BackBtn").GetComponent<Button>();
                backBtn.onClick.AddListener(OnBackBtnClick);
            }
            else
            {
                backBtn.gameObject.SetActive(false);
            }
        }
        Debug.Log(Network.player.ipAddress);
    }

    private void FixedUpdate()
    {
        text.text = "isLocalPlayer:" + isLocalPlayer;
        //if (NetChessBoard.Instance.isGameOver) return;
        // 设置悔棋按钮
        if (isLocalPlayer)
        {
            SetBackBtn();
        }
        if (chessType != GameDefine.ChessType.Watch && isLocalPlayer && NetChessBoard.Instance.isGameOver)
        {
            NetChessBoard.Instance.GameEnd();
        }
        // 当前对应走棋方
        if (chessType == NetChessBoard.Instance.curTurn && NetChessBoard.Instance.timer >= NetChessBoard.Instance.PER_TIME  && isLocalPlayer && NetChessBoard.Instance.PlayerNumber>=2)
        {
            int[] temPos = CheckClickPos();
            if (temPos == null) return;
            CmdChess(temPos);
        }
    }
    // 客户端发出指令，在服务器调用该方法
    [Command]
    public void CmdSetPlayer()
    {
        NetChessBoard.Instance.PlayerNumber++;
        Debug.Log("PlayerNumber:" + NetChessBoard.Instance.PlayerNumber);
        if (NetChessBoard.Instance.PlayerNumber == 1)
        {
            text.text += "  设置黑棋玩家";
            chessType = GameDefine.ChessType.Black;
        }
        else if (NetChessBoard.Instance.PlayerNumber == 2)
        {
            text.text += "  设置白棋玩家";
            chessType = GameDefine.ChessType.White;
        }
        else
        {
            text.text += "  设置观众玩家";
            chessType = GameDefine.ChessType.Watch;
        }
    }

    [Command]
    public void CmdChess(int[] pos)
    {
        NetChessBoard.Instance.PlayChess(pos);
    }

    // 获取点击的棋盘坐标位置
    private int[] CheckClickPos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 范围限制
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int max = NetChessBoard.Max_LINE;
            int roundX = Mathf.RoundToInt(clickPos.x + max / 2);
            int roundY = Mathf.RoundToInt(clickPos.y + max / 2);
            if (roundX >= 0 && roundX < max && roundY >= 0 && roundY < max)
            {
                // 判断当前位置是否是空位置
                if (NetChessBoard.Instance.GetGrid()[roundX, roundY] == (int)GameDefine.DotType.NONE)
                {
                    return new int[] { roundX, roundY };
                }
            }
        }
        return null;
    }
    // 更换先手
    //public void ChangeColor()
    //{
    //    chessType = chessType == GameDefine.ChessType.Black ? GameDefine.ChessType.White : GameDefine.ChessType.Black;
    //}
    public void OnBackBtnClick()
    {
        CmdChessBack();
    }

    [Command]
    public void CmdChessBack()
    {
        NetChessBoard.Instance.ChessBack();
    }
    // 根据是否是观众来判断是否显示悔棋按钮
    private void SetBackBtn()
    {
        if (chessType == GameDefine.ChessType.Watch)
        {
            return;
        }
        else if (chessType == NetChessBoard.Instance.curTurn && NetChessBoard.Instance.isGameOver==false)
        {
            if (backBtn.gameObject.activeSelf) backBtn.interactable = true;
        }
        else
        {
            if (backBtn.gameObject.activeSelf) backBtn.interactable = false;
        }
        text.text = chessType+"   "+ NetChessBoard.Instance.curTurn + "     NetChessBoard.Instance.ChessStack.Count:" + NetChessBoard.Instance.ChessStack.Count;
    }
}
