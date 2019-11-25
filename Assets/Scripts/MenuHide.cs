using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHide : MonoBehaviour
{
    public void DoHide()
    {
        gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("Pattern") == (int)GameDefine.PATTERN.PATTERN_NET)
        {
            NetChessBoard.Instance.isGameOver = false;
            Debug.Log("IsGameOver = false");
        }
        else
        {
            ChessBoard.Instance.IsGameOver = false;
        }
    }
}
