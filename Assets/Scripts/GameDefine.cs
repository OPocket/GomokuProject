using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
    // 模式名称
    public enum PATTERN
    {
        PATTERN_AI1,
        PATTERN_AI2,
        PATTERN_AI3,
        PATTERN_DOUBLE,
        PATTERN_NET
    }
    // 场景名称
    public enum SCENE_NUM
    {
        SCENE_MENU,
        SCENE_GAME,
        SCENE_NET,
        SCENE_LOBBY
    }
    // 玩家类型
    public enum ChessType
    {
        Watch,
        Black,
        White
    }
    // 地图上点的类型
    public enum DotType
    {
        NONE = 0,       // 空白
        BLACK,          // 填充了黑棋
        WHITE           // 填充了白棋
    }
}
