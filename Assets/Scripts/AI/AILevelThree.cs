using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILevelThree : Player
{
    // 根据位值类型分值
    private Dictionary<string, float> scoreDic = new Dictionary<string, float>();
    //计算深度
    public int DeepCount = 3;

    private void Start()
    {
        // 收益值类型
        // 连五
        scoreDic.Add("aaaaa", float.MaxValue);

        //活四
        scoreDic.Add("_aaaa_", 1000000);

        //冲四
        scoreDic.Add("a_aaa", 15000);
        scoreDic.Add("aa_aa", 15000);
        scoreDic.Add("aaa_a", 15000);
        scoreDic.Add("aaaa_", 15000);
        scoreDic.Add("_aaaa", 15000);

        // 活三
        scoreDic.Add("_aaa_", 10000);
        // 跳活三
        scoreDic.Add("_a_aa_", 9000);
        scoreDic.Add("_aa_a_", 9000);
        // 眠三
        scoreDic.Add("a__aa", 1000);
        scoreDic.Add("a_aa_", 1000);
        scoreDic.Add("a_a_a", 1000);
        scoreDic.Add("aaa__", 1000);
        scoreDic.Add("_aa_a", 1000);
        scoreDic.Add("_a_aa", 1000);
        scoreDic.Add("__aaa", 1000);
        // 活二
        scoreDic.Add("_a__a_", 500);
        scoreDic.Add("__aa__", 500);

        scoreDic.Add("_aa__", 500);
        scoreDic.Add("__aa_", 500);
        scoreDic.Add("_a_a_", 500);
        // 眠二
        scoreDic.Add("aa___", 100);
        scoreDic.Add("a_a__", 100);
        scoreDic.Add("a__a_", 100);
        scoreDic.Add("__a_a", 100);
        scoreDic.Add("___aa", 100);
        scoreDic.Add("_a__a", 100);
        scoreDic.Add("a___a", 100);
    }

    protected override int[] CheckClickPos()
    {
        if (ChessBoard.Instance.ChessStack.Count == 0)
        {
            int[] pos = new int[2];
            // 随机一个位置(-2是为了避免太靠近边界，有可能使对方太过容易赢得比赛)
            pos[0] = Random.Range(2, ChessBoard.Max_LINE-2);
            pos[1] = Random.Range(2, ChessBoard.Max_LINE-2);
            return pos;
        }
        else
        {
            AILevelNode maxNode = null;
            int[,] temGrid = (int[,])ChessBoard.Instance.GetGrid();
            foreach (AILevelNode child in GetChildList(temGrid, true, chessType))
            {
                // 创建树 子节点为非自己，所以用false;
                // 每个grid都为独立不同的方案，为了不影响独立的grid，所以Clone();
                CreateTree(child, (int[,])temGrid.Clone(), false, DeepCount);
                child.score += DoAlphBeta(child, false, DeepCount, float.MinValue, float.MaxValue);
                if (maxNode == null)
                {
                    maxNode = child;
                }
                else
                {
                    if (maxNode.score < child.score)
                    {
                        maxNode = child;
                    }
                }
            }
            return maxNode.pos;
        }
    }
    private float GetMaxScore(int[,] grid, int[] pos, ChessBoard.ChessType type)
    {
        float temScore = 0;
        temScore += CheckOneLine(grid, pos, new int[2] { 1, 0 }, type);
        temScore += CheckOneLine(grid, pos, new int[2] { 0, 1 }, type);
        temScore += CheckOneLine(grid, pos, new int[2] { 1, -1 }, type);
        temScore += CheckOneLine(grid, pos, new int[2] { 1, 1 }, type);
        return temScore;
    }

    // 进行评分 此时grid为假设的grid，在变化中，所以需要传入当前假设情况下的grid；
    protected virtual float CheckOneLine(int[,] grid, int[] pos, int[] offpos, ChessBoard.ChessType type)
    {
        string str = "a";
        int allTimes = 0;
        bool isTurnLeft = false;
        bool leftStop = false;
        bool rightStop = false;

        // 跳过第一次循环，避免重复存储监测点
        int ri = pos[0] + offpos[0], rj = pos[1] + offpos[1];
        int li = pos[0] - offpos[0], lj = pos[1] - offpos[1];
        // 为了避免取到的均为空位，即“_”，所以限制取到空位时，调头取另一端所在位置的类型，若也是空位则再调头，就可取到有效的TimeLimit位数的str值
        while (allTimes < ChessBoard.TimeLimit && (!leftStop || !rightStop))
        {
            // 正向检测
            if (!isTurnLeft)
            {
                if (ri >= 0 && ri < ChessBoard.Max_LINE && rj >= 0 && rj < ChessBoard.Max_LINE && !rightStop)
                {
                    // 如果对应当前所下的棋子颜色
                    if (grid[ri, rj] == (int)type)
                    {
                        str += "a";
                        allTimes++;
                    }
                    // 如果对应为空棋子
                    else if (grid[ri, rj] == (int)ChessBoard.DotType.NONE)
                    {
                        str += "_";
                        allTimes++;
                        if (!leftStop)
                        {
                            isTurnLeft = true;
                        }
                    }
                    // 被堵住了
                    else
                    {
                        rightStop = true;
                        if (!leftStop)
                        {
                            isTurnLeft = true;
                        }
                    }
                    ri += offpos[0];
                    rj += offpos[1];
                }
                else
                {
                    rightStop = true;
                    if (!leftStop)
                    {
                        isTurnLeft = true;
                    }
                }
            }
            // 反向检测
            else
            {
                if (li >= 0 && li < ChessBoard.Max_LINE && lj >= 0 && lj < ChessBoard.Max_LINE && !leftStop)
                {
                    // 如果对应当前所下的棋子颜色
                    if (grid[li, lj] == (int)type)
                    {
                        str = "a" + str;
                        allTimes++;
                    }
                    else if (grid[li, lj] == (int)ChessBoard.DotType.NONE)
                    {
                        str = "_" + str;
                        allTimes++;
                        if (!rightStop)
                        {
                            isTurnLeft = false;
                        }
                    }
                    else
                    {
                        leftStop = true;
                        if (!rightStop)
                        {
                            isTurnLeft = false;
                        }
                    }
                    li -= offpos[0];
                    lj -= offpos[1];
                }
                else
                {
                    leftStop = true;
                    if (!rightStop)
                    {
                        isTurnLeft = false;
                    }
                }
            }
        }
        // 返回当前位置的得分
        float temScore = 0;
        foreach (string ikey in scoreDic.Keys)
        {
            if (str.Contains(ikey))
            {
                temScore = scoreDic[ikey];
                break;
            }
        }
        return temScore;
    }
    // 获取当前受益值最大或者最小Node列表
    private List<AILevelNode> GetChildList(int[,] grid, bool isSelf, ChessBoard.ChessType type)
    {
        List<AILevelNode> temList = new List<AILevelNode>();
        AILevelNode node;
        // 获取三个极大或极小的子节点child；
        for (int i = 0; i < ChessBoard.Max_LINE; i++)
        {
            for (int j = 0; j < ChessBoard.Max_LINE; j++)
            {
                if (grid[i, j] != (int)ChessBoard.DotType.NONE)
                {
                    continue;
                }
                int[] pos = new int[2] { i, j };
                node = new AILevelNode();
                node.chessType = type;
                node.pos = pos;
                // 获取当前点的得分
                if (isSelf)
                {
                    // 最大分（当前位置的初始score = 白+黑的得分）
                    node.score = (GetMaxScore(grid, pos, ChessBoard.ChessType.Black)+ GetMaxScore(grid, pos, ChessBoard.ChessType.White));
                }
                else
                {
                    // 最小分 负号可取得最小值的点
                    node.score = (-GetMaxScore(grid, pos, ChessBoard.ChessType.Black) - GetMaxScore(grid, pos, ChessBoard.ChessType.White));
                }
                // 取出极大极小点
                if (temList.Count < 4)
                {
                    temList.Add(node);
                }
                else
                {
                    foreach (AILevelNode temNode in temList)
                    {
                        if (isSelf)
                        {
                            if (temNode.score < node.score)
                            {
                                temList.Remove(temNode);
                                temList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            if (temNode.score > node.score)
                            {
                                temList.Remove(temNode);
                                temList.Add(node);
                                break;
                            }
                        }
                    }
                }
            }
        }
        return temList;
    }
    // 创建树
    private void CreateTree(AILevelNode node, int[,] grid, bool isSelf, int deep)
    {
        if (deep == 0 || node.score == float.MaxValue)
        {
            return;
        }
        // 设置当前点填充到地图上
        grid[node.pos[0], node.pos[1]] = (int)node.chessType;
        node.child = GetChildList(grid, !isSelf, node.chessType);
        foreach (AILevelNode child in node.child)
        {
            // 创建下一级的节点,Clone()避免修改父节点的grid数据
            CreateTree(child, (int[,])grid.Clone(), !isSelf, deep - 1);
        }
    }
    // 剪枝
    private float DoAlphBeta(AILevelNode node, bool isSelf, int deep, float alpha, float beta)
    {
        if (deep == 0 || node.score == float.MaxValue || node.score == float.MinValue)
        {
            return node.score;
        }
        //alpha剪枝
        if (isSelf)
        {
            foreach (AILevelNode child in node.child)
            {
                alpha = Mathf.Max(alpha, DoAlphBeta(child, !isSelf, deep - 1, alpha, beta));
                // "="的情况下有可能出现更差的结果，所以也直接return;
                if (alpha >= beta)
                {
                    return alpha;
                }
            }
            return alpha;
        }
        // beta剪枝
        else
        {
            foreach (AILevelNode child in node.child)
            {
                beta = Mathf.Min(beta, DoAlphBeta(child, !isSelf, deep - 1, alpha, beta));
                if (alpha >= beta)
                {
                    return beta;
                }
            }
            return beta;
        }
    }
}
