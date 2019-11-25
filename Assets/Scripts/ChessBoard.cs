using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ChessBoard : MonoBehaviour
{
    private GameDefine.ChessType curTurn = GameDefine.ChessType.Black;

    // 地图格子
    public const int Max_LINE= 15;
    public const int TimeLimit = 7;
    private int[,] grid;
    // 棋子
    public GameObject[] chessPrefabs;
    public float timer =0.0f;
    // 限定的反应时间
    public float PER_TIME = 0.3f; 
    private bool isGameOver = true;
    // 统一管理棋子
    private Transform[,] chessMap = new Transform[Max_LINE, Max_LINE];
    // 悔棋存储
    private Stack<Transform> chessStack = new Stack<Transform>();

    private static ChessBoard _instance;

    public static ChessBoard Instance { get { return _instance; } }
    public GameDefine.ChessType CurTurn { get { return curTurn; } }
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }
    public Stack<Transform> ChessStack { get { return chessStack; } }

    // 结算界面
    public GameObject OverPanel;
    public Text ResultText;
    // 下棋点的标识
    public GameObject effectChess;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        grid = new int[Max_LINE, Max_LINE];
        effectChess.SetActive(false);
    }

    private void Update()
    {
        // 标识跟随
        if (chessStack.Count > 0)
        {
            if (!effectChess.activeSelf) effectChess.SetActive(true);
            effectChess.transform.position = chessStack.Peek().position;
        }

        if (IsGameOver)
        {
            return;
        }
        timer += Time.deltaTime;
    }

    public void PlayChess(int[] pos)
    {
        if (pos == null || IsGameOver) return;
        // 限制当前值在0和Max_LINE之间
        //pos[0] = Mathf.Clamp(pos[0], 0, Max_LINE);
        //pos[1] = Mathf.Clamp(pos[1], 0, Max_LINE);
        // 枚举是整数类型，返回的哈希值刚好是整数值
        if (grid[pos[0], pos[1]] != (int)GameDefine.DotType.NONE)
        {
            Debug.Log("grid的值为:"+grid[pos[0], pos[1]]);
            return;
        }
        int i = Mathf.FloorToInt(Max_LINE / 2);
        if (curTurn == GameDefine.ChessType.Black)
        {
            // 生成黑棋子
            GameObject prefabsB = Instantiate(chessPrefabs[0], new Vector3(pos[0] - i, pos[1] - i, 0), Quaternion.identity);
            Debug.Log("生成了黑色棋子");
            chessMap[pos[0], pos[1]] = prefabsB.transform;
            ChessStack.Push(prefabsB.transform);
            grid[pos[0], pos[1]] = (int)GameDefine.DotType.BLACK;
            CheckChess(pos);
            curTurn = GameDefine.ChessType.White;
            timer = 0.0f;
        }
        else if (curTurn == GameDefine.ChessType.White)
        {
            // 生成白棋子
            GameObject prefabsW = Instantiate(chessPrefabs[1], new Vector3(pos[0] - i, pos[1] - i, 0), Quaternion.identity);
            Debug.Log("生成了白色棋子");
            chessMap[pos[0], pos[1]] = prefabsW.transform;
            ChessStack.Push(prefabsW.transform);
            grid[pos[0], pos[1]] = (int)GameDefine.DotType.WHITE;
            CheckChess(pos);
            curTurn = GameDefine.ChessType.Black;
            timer = 0.0f;
        }    
    }

    public int[,] GetGrid()
    {
        return grid;
    }

    // 检查是否可以消除
    private void CheckChess(int[] pos)
    {
        List<Transform> temList  = CheckAllLine(pos);
        if (temList.Count >= 4)
        {
            // 游戏结束
            isGameOver = true;
            Debug.Log("游戏结束");
            if (CurTurn == GameDefine.ChessType.White)
            {
                ResultText.text = "白棋胜";
            }
            else if(CurTurn == GameDefine.ChessType.Black)
            {
                ResultText.text = "黑棋胜";
            }
            OverPanel.SetActive(true);
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
            if (grid[i, j] == (int)curTurn)
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
            if (grid[i, j] == (int)curTurn)
            {
                temList.Add(chessMap[i, j]);
            }
            else
            {
                break;
            }
        }
        // 返回List中未包含自身
        return temList;
    }
    // 悔棋操作
    public void ChessBack()
    {
        if (ChessStack.Count >= 2)
        {
            effectChess.SetActive(false);
            // 删除最近一次自己和对方的棋子,所以执行两次DoBack；
            DoBack();
            DoBack();
        }
        Debug.Log("ChessStack.Count:" + ChessStack.Count);
    }
    private void DoBack()
    {
        Transform temTs = ChessStack.Pop();
        int i = Mathf.FloorToInt(Max_LINE / 2);
        int posX = (int)temTs.position.x + i;
        int posY = (int)temTs.position.y + i;
        grid[posX, posY] = (int)GameDefine.DotType.NONE;
        chessMap[posX, posY] = null;
        Destroy(temTs.gameObject);
        temTs = null;
    }
}
