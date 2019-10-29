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
            SetPlayer(gamePattern);
        }
    }

    public void SetPlayer(GameDefine.PATTERN pattern)
    {
        switch (pattern)
        {
            case GameDefine.PATTERN.PATTERN_AI1:
                playerList[0].chessType = ChessBoard.ChessType.Black;
                playerList[2].chessType = ChessBoard.ChessType.White;
                break;
            case GameDefine.PATTERN.PATTERN_AI2:
                playerList[0].chessType = ChessBoard.ChessType.Black;
                playerList[3].chessType = ChessBoard.ChessType.White;
                break;
            case GameDefine.PATTERN.PATTERN_AI3:
                playerList[0].chessType = ChessBoard.ChessType.Black;
                playerList[4].chessType = ChessBoard.ChessType.White;
                break;
            case GameDefine.PATTERN.PATTERN_DOUBLE:
                playerList[0].chessType = ChessBoard.ChessType.Black;
                playerList[1].chessType = ChessBoard.ChessType.White;
                break;
            case GameDefine.PATTERN.PATTERN_NET:
                //playerList[0].chessType = ChessBoard.ChessType.Black;
                break;
            default:
                break;
        }
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
}
