using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ChessBoard.ChessType chessType = ChessBoard.ChessType.Black;

    protected virtual void FixedUpdate()
    {
        if (ChessBoard.Instance.IsGameOver) return;
        // 当前对应走棋方
        if (chessType == ChessBoard.Instance.CurTurn && ChessBoard.Instance.timer>=ChessBoard.Instance.PER_TIME)
        {
            ChessBoard.Instance.PlayChess(CheckClickPos());
        }
    }

    // 获取点击的棋盘坐标位置
    protected virtual int[] CheckClickPos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 范围限制
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int max = ChessBoard.Max_LINE;
            int roundX = Mathf.RoundToInt(clickPos.x + max / 2);
            int roundY = Mathf.RoundToInt(clickPos.y + max / 2);
            if (roundX >= 0 && roundX<max && roundY >= 0 && roundY < max)
            {
                // 判断当前位置是否是空位置
                if(ChessBoard.Instance.GetGrid()[roundX, roundY] == (int)ChessBoard.DotType.NONE)
                {
                    return new int[] { roundX, roundY };
                }
            }
        }
        return null;
    }
}
