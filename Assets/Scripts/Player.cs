using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameDefine.ChessType chessType = GameDefine.ChessType.Black;
    // AI不需要悔棋
    private GameObject backBtnObj;
    private GameObject changeColorObj;
    private GameDefine.PATTERN pattern;
     
    protected virtual void Start()
    {
        backBtnObj = GameObject.Find("Canvas/BackBtn");
        changeColorObj = GameObject.Find("Canvas/ChangeButton");
        Debug.Log("backBtn");
        if (PlayerPrefs.HasKey("Pattern"))
        {
            pattern = (GameDefine.PATTERN)PlayerPrefs.GetInt("Pattern");
            if (pattern == GameDefine.PATTERN.PATTERN_DOUBLE)
            {
                if (backBtnObj)
                {
                    backBtnObj.SetActive(false);
                }
                if (changeColorObj)
                {
                    changeColorObj.SetActive(false);
                }
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (ChessBoard.Instance.IsGameOver) return;
        // 当前对应走棋方
        if (chessType == ChessBoard.Instance.CurTurn && ChessBoard.Instance.timer >= ChessBoard.Instance.PER_TIME)
        {
            ChessBoard.Instance.PlayChess(CheckClickPos());
        }
        // 是否置黑悔棋按钮
        if (pattern != GameDefine.PATTERN.PATTERN_DOUBLE)
        {
            // 避免未下棋时，显示悔棋按钮
            SetBackBtn();
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
            if (roundX >= 0 && roundX < max && roundY >= 0 && roundY < max)
            {
                // 判断当前位置是否是空位置
                if (ChessBoard.Instance.GetGrid()[roundX, roundY] == (int)GameDefine.DotType.NONE)
                {
                    return new int[] { roundX, roundY };
                }
            }
        }
        return null;
    }
    // 更换先手
    public virtual void ChangeColor()
    {
        chessType = chessType == GameDefine.ChessType.Black ? GameDefine.ChessType.White : GameDefine.ChessType.Black;
    }
    // 根据是否是观众来判断是否显示悔棋按钮
    protected virtual void SetBackBtn()
    {
        if (chessType == GameDefine.ChessType.Watch)
        {
            return;
        }
        else if (chessType == ChessBoard.Instance.CurTurn && ChessBoard.Instance.ChessStack.Count >= 2)
        {
            if (backBtnObj.activeSelf) backBtnObj.GetComponent<Button>().interactable = true;
        }
        else
        {
            if (backBtnObj.activeSelf) backBtnObj.GetComponent<Button>().interactable = false;
        }
    }
}
