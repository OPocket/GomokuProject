using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 边界情况处理在高级AI中处理
 * 平局情况处理统一处理
 */

public class AILevelOne : Player
{
    // 根据位值类型分值
    protected Dictionary<string, float> scoreDic = new Dictionary<string, float>();
    // 存储分数
    protected float[,] scoreValue = new float[ChessBoard.Max_LINE, ChessBoard.Max_LINE];
    protected virtual void Start()
    {
        // 评分类型
        scoreDic.Add("_aaaaa_", float.MaxValue);
        scoreDic.Add("aaaaa", float.MaxValue);
        scoreDic.Add("aaaaa_", float.MaxValue);
        scoreDic.Add("_aaaaa", float.MaxValue);

        scoreDic.Add("_aaaa_", 10000);
        scoreDic.Add("_aaaa", 5000);
        scoreDic.Add("aaaa_", 5000);

        scoreDic.Add("_aaa_", 1000);
        scoreDic.Add("_aaa", 500);
        scoreDic.Add("aaa_", 500);
 
        scoreDic.Add("_aa_", 100);
        scoreDic.Add("_aa", 50);
        scoreDic.Add("aa_", 50);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override int[] CheckClickPos()
    {
        int[] pos = new int[2];
        if (ChessBoard.Instance.ChessStack.Count == 0)
        {
            // 随机一个位置(-2是为了避免太靠近边界，有可能使对方太过容易赢得比赛)
            pos[0] = Random.Range(2, ChessBoard.Max_LINE - 2);
            pos[1] = Random.Range(2, ChessBoard.Max_LINE - 2);
            return pos;
        }
        else
        {
            float maxScore = 0.0f;
            for (int i = 0; i < ChessBoard.Max_LINE; i++)
            {
                for (int j = 0; j < ChessBoard.Max_LINE; j++)
                {
                    float temScore = 0;
                    // 如果当前位置可填充棋子
                    if (ChessBoard.Instance.GetGrid()[i, j] == (int)ChessBoard.DotType.NONE)
                    {
                        // 优先评判自己棋子的收益值，因为出现等值情况时候，自己可以先下手取胜
                        temScore = GetMaxScore(new int[]{i, j}, ChessBoard.ChessType.Black)+GetMaxScore(new int[] { i, j }, ChessBoard.ChessType.White);
                        if (temScore > maxScore)
                        {
                            maxScore = temScore;
                            pos[0] = i;
                            pos[1] = j;
                        }
                    }
                }
            }
            return pos;
        }
    }
    private float GetMaxScore(int[] pos, ChessBoard.ChessType type)
    {
        float temScore = 0;
        temScore +=CheckOneLine(pos, new int[2] { 1, 0 }, type);
        temScore += CheckOneLine(pos, new int[2] { 0, 1 }, type);
        temScore += CheckOneLine(pos, new int[2] { 1, -1 }, type);
        temScore += CheckOneLine(pos, new int[2] { 1, 1 }, type);
        return scoreValue[pos[0], pos[1]] = temScore;
    }

    // 进行评分
    protected virtual float CheckOneLine(int[] pos, int[] offpos, ChessBoard.ChessType type)
    {
        string str = "a";
        // 正向检测：跳过第一次循环，避免重复存储监测点
        for (int i = pos[0] + offpos[0], j = pos[1] + offpos[1]; i >= 0 && i < ChessBoard.Max_LINE && j >= 0 && j < ChessBoard.Max_LINE; i += offpos[0], j += offpos[1])
        {
            // 如果对应当前所下的棋子颜色
            if (ChessBoard.Instance.GetGrid()[i, j] == (int)type)
            {
                str += "a";
            }
            else
            {
                str += "_";
                break;
            }
        }
        // 反向检测：跳过第一次循环，避免重复存储监测点
        for (int i = pos[0] - offpos[0], j = pos[1] - offpos[1]; i >= 0 && i < ChessBoard.Max_LINE && j >= 0 && j < ChessBoard.Max_LINE; i -= offpos[0], j -= offpos[1])
        {
            // 如果对应当前所下的棋子颜色
            if (ChessBoard.Instance.GetGrid()[i, j] == (int)type)
            {
                str = "a" + str;
            }
            else
            {
                str = "_" + str;
                break;
            }
        }
        // 返回当前位置的得分
        float temScore = 0;
        // 因为后面的分值高，所以采用倒序查询，减少遍历次数
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
}
