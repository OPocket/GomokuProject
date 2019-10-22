using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ChessBoard.ChessType chessType = ChessBoard.ChessType.Black;

    private void Start()
    {
        
    }

    private void Update()
    {
        CheckClickPos();
    }

    // 获取点击的棋盘坐标位置
    private Vector2 CheckClickPos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(Mathf.RoundToInt(clickPos.x + 7), Mathf.RoundToInt(clickPos.y + 7));
        }
        return null;
    }
}
