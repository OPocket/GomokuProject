using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Player> playerList = new List<Player>();
    private GameDefine.PATTERN gamePattern = GameDefine.PATTERN.PATTERN_AI1;
    private void Awake()
    {
        // 根据所存储的键值设置跳转的界面
        if (PlayerPrefs.HasKey("Pattern"))
        {
            gamePattern = (GameDefine.PATTERN)PlayerPrefs.GetInt("Pattern");
        }
        // 初始化player的状态
        int iChangeColor = 0;
        if (PlayerPrefs.HasKey("ChangeColor"))
        {
            iChangeColor = PlayerPrefs.GetInt("ChangeColor");
        }
        SetPlayer(gamePattern, iChangeColor);
    }

    public void SetPlayer(GameDefine.PATTERN pattern, int isChangeColor)
    {
        switch (pattern)
        {
            case GameDefine.PATTERN.PATTERN_AI1:
                if (isChangeColor==0)
                {
                    SetPlayer(0, 2);
                }
                else
                {
                    SetPlayer(2, 0);
                }
                break;
            case GameDefine.PATTERN.PATTERN_AI2:
                if (isChangeColor == 0)
                {
                    SetPlayer(0, 3);
                }
                else
                {
                    SetPlayer(3, 0);
                }
                break;
            case GameDefine.PATTERN.PATTERN_AI3:
                if (isChangeColor == 0)
                {
                    SetPlayer(0, 4);
                }
                else
                {
                    SetPlayer(4, 0);
                }
                break;
            case GameDefine.PATTERN.PATTERN_DOUBLE:
                // 隐藏更换先手按钮

                SetPlayer(0, 1);
                break;
            case GameDefine.PATTERN.PATTERN_NET:
                //playerList[0].chessType = ChessBoard.ChessType.Black;
                break;
            default:
                break;
        }
    }
    // 设置黑棋或者白棋玩家
    private void SetPlayer(int index0, int index1)
    {
        playerList[index0].chessType = GameDefine.ChessType.Black;
        playerList[index1].chessType = GameDefine.ChessType.White;
    }

    // 返回首页
    public void BackToStartMenu()
    {
        SceneManager.LoadScene((int)GameDefine.SCENE_NUM.SCENE_MENU);
    }
    // 重新开始
    public void Restart()
    {
        SceneManager.LoadScene((int)GameDefine.SCENE_NUM.SCENE_GAME);
    }
    // 更换先手
    public void ChangeFirstPlayer()
    {
        // 避免第一次进游戏不存在所存储的键值
        if (PlayerPrefs.HasKey("ChangeColor"))
        {
            PlayerPrefs.SetInt("ChangeColor", PlayerPrefs.GetInt("ChangeColor") == 0 ? 1 : 0);
        }
        else
        {
            PlayerPrefs.SetInt("ChangeColor", 1);
        }
        SceneManager.LoadScene((int)GameDefine.SCENE_NUM.SCENE_GAME);
    }
}
