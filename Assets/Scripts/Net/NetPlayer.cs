using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetPlayer : NetworkBehaviour
{
    [SyncVar]
    public GameDefine.ChessType chessType = GameDefine.ChessType.Black;

    private void Start()
    {
        if (isLocalPlayer)
        {
            CmdSetPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (NetChessBoard.Instance.IsGameOver) return;
        // 当前对应走棋方
        if (Input.GetMouseButtonDown(0)&&chessType == NetChessBoard.Instance.curTurn && NetChessBoard.Instance.timer >= NetChessBoard.Instance.PER_TIME )
        {
            CmdChess(CheckClickPos());
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
            chessType = GameDefine.ChessType.Black;
        }
        else if (NetChessBoard.Instance.PlayerNumber == 2)
        {
            chessType = GameDefine.ChessType.White;
        }
        else
        {
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
        return null;
    }
    // 更换先手
    //public void ChangeColor()
    //{
    //    chessType = chessType == GameDefine.ChessType.Black ? GameDefine.ChessType.White : GameDefine.ChessType.Black;
    //}
}
