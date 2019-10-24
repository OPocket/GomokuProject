using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChessBoard : MonoBehaviour
{
    private ChessType curTurn = ChessType.Black;

    // 地图格子
    public const int Max_LINE= 15;
    private int[,] grid;
    // 棋子
    public GameObject[] chessPrefabs;
    public float timer =0.0f;
    // 限定的反应时间
    public float PER_TIME = 0.3f; 
    private bool isGameOver = false;
    // 统一管理棋子
    private Transform[,] chessMap = new Transform[Max_LINE, Max_LINE];
    // 悔棋存储
    private Stack<Transform> chessStack = new Stack<Transform>();

    private static ChessBoard _instance;

    public static ChessBoard Instance { get => _instance;}
    public ChessType CurTurn { get => curTurn;}
    public bool IsGameOver { get => isGameOver;}
    public Stack<Transform> ChessStack { get => chessStack;}

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

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        grid = new int[Max_LINE, Max_LINE];
    }

    private void Update()
    {
        if (IsGameOver) return;
        timer += Time.deltaTime;
    }

    public void PlayChess(int[] pos)
    {
        if (pos == null || IsGameOver) return;
        // 限制当前值在0和Max_LINE之间
        //pos[0] = Mathf.Clamp(pos[0], 0, Max_LINE);
        //pos[1] = Mathf.Clamp(pos[1], 0, Max_LINE);
        // 枚举是整数类型，返回的哈希值刚好是整数值
        if (grid[pos[0], pos[1]] != DotType.NONE.GetHashCode())
        {
            Debug.Log("grid的值为:"+grid[pos[0], pos[1]]);
            return;
        }
        int i = Mathf.FloorToInt(Max_LINE / 2);
        if (curTurn == ChessType.Black)
        {
            // 生成黑棋子
            GameObject prefabsB = Instantiate(chessPrefabs[0], new Vector3(pos[0] - i, pos[1] - i, 0), Quaternion.identity);
            Debug.Log("生成了黑色棋子");
            chessMap[pos[0], pos[1]] = prefabsB.transform;
            ChessStack.Push(prefabsB.transform);
            grid[pos[0], pos[1]] = DotType.BLACK.GetHashCode();
            CheckChess(pos);
            curTurn = ChessType.White;
            timer = 0.0f;
        }
        else if (curTurn == ChessType.White)
        {
            // 生成白棋子
            GameObject prefabsW = Instantiate(chessPrefabs[1], new Vector3(pos[0] - i, pos[1] - i, 0), Quaternion.identity);
            Debug.Log("生成了白色棋子");
            chessMap[pos[0], pos[1]] = prefabsW.transform;
            ChessStack.Push(prefabsW.transform);
            grid[pos[0], pos[1]] = DotType.WHITE.GetHashCode();
            CheckChess(pos);
            curTurn = ChessType.Black;
            timer = 0.0f;
        }    
    }

    public int GetDotType(int x, int y)
    {
        return grid[x, y];
    }

    // 检查是否可以消除
    private void CheckChess(int[] pos)
    {
        List<Transform> temList  = CheckAllLine(pos);
        Debug.Log(temList.Count);
        if (temList.Count >= 4)
        {
            // 游戏结束
            isGameOver = true;
            Debug.Log("游戏结束");
        }
    }

    // 检测所有方向
    private List<Transform> CheckAllLine(int[] pos)
    {
        List<Transform> saveList = new List<Transform>();
        List<Transform> temList0 = CheckOneLine(pos, new int[2] { 1, 0 });
        List<Transform> temList1 = CheckOneLine(pos, new int[2] { 0, 1 });
        List<Transform> temList2 = CheckOneLine(pos, new int[2] { 1, -1 });
        List<Transform> temList3 = CheckOneLine(pos, new int[2] { 1, 1 });
        AddChessList(saveList, temList0);
        AddChessList(saveList, temList1);
        AddChessList(saveList, temList2);
        AddChessList(saveList, temList3);
        return saveList;
    }
    // 把符合消除方向上的对应棋子加入到总列表中
    private void AddChessList(List<Transform> allList, List<Transform> temList)
    {
        if (temList != null && temList.Count >= 4)
        {
            allList.AddRange(temList);
        }
    }
    // 检测一条线
    private List<Transform> CheckOneLine(int[] pos, int[] offpos)
    {
        List<Transform> temList = new List<Transform>();
        // 正向检测：跳过第一次循环，避免重复存储监测点
        for (int i = pos[0]+offpos[0], j = pos[1]+offpos[1]; i >=0&&i<Max_LINE&&j >= 0 && j <Max_LINE; i+=offpos[0],j+=offpos[1])
        {
            // 如果对应当前所下的棋子颜色
            if (grid[i, j] == curTurn.GetHashCode())
            {
                temList.Add(chessMap[i, j]);
            }
            else
            {
                break;
            }
        }
        // 反向检测：跳过第一次循环，避免重复存储监测点
        for (int i = pos[0] - offpos[0], j = pos[1] - offpos[1]; i >= 0 && i < Max_LINE && j >= 0 && j < Max_LINE; i -= offpos[0], j -= offpos[1])
        {
            // 如果对应当前所下的棋子颜色
            if (grid[i, j] == curTurn.GetHashCode())
            {
                temList.Add(chessMap[i, j]);
            }
            else
            {
                break;
            }
        }
        return temList;
    }
    // 悔棋操作
    public void ChessBack()
    {
        if (ChessStack.Count > 0)
        {
            Transform temTs = ChessStack.Pop();
            int i = Mathf.FloorToInt(Max_LINE / 2);
            int posX = (int)temTs.position.x + i;
            int posY = (int)temTs.position.y + i;
            grid[posX, posY] = DotType.NONE.GetHashCode();
            chessMap[posX, posY] = null;
            Destroy(temTs.gameObject);
        }
    }
}
