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
    protected float[,] soreValue = new float[ChessBoard.Max_LINE, ChessBoard.Max_LINE];
    protected virtual void Start()
    {
        // 评分类型
        scoreDic.Add("_aa_", 100);
        scoreDic.Add("_aa", 50);
        scoreDic.Add("aa_", 50);

        scoreDic.Add("_aaa_", 1000);
        scoreDic.Add("_aaa", 500);
        scoreDic.Add("aaa_", 500);

        scoreDic.Add("_aaaa_", 10000);
        scoreDic.Add("_aaaa", 5000);
        scoreDic.Add("aaaa_", 5000);

        scoreDic.Add("_aaaaa_", float.MaxValue);
        scoreDic.Add("aaaaa", float.MaxValue);
        scoreDic.Add("aaaaa_", float.MaxValue);
        scoreDic.Add("_aaaaa", float.MaxValue);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override int[] CheckClickPos()
    {
        int[] pos = new int[] { Random.Range(0, ChessBoard.Max_LINE), Random.Range(0, ChessBoard.Max_LINE) };
        if (ChessBoard.Instance.ChessStack.Count == 0)
        {
            // 随机一个位置
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
                    if (ChessBoard.Instance.GetDotType(i, j) == ChessBoard.DotType.NONE.GetHashCode())
                    {
                        temScore = GetMaxScore(new int[]{i, j}, ChessBoard.ChessType.Black);
                        if (temScore > maxScore)
                        {
                            maxScore = temScore;
                            pos[0] = i;
                            pos[1] = j;
                        }
                        temScore = GetMaxScore(new int[] { i, j }, ChessBoard.ChessType.White);
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
        return soreValue[pos[0], pos[1]] = Mathf.Max(CheckOneLine(pos, new int[2] { 1, 0 }, type), CheckOneLine(pos, new int[2] { 0, 1 }, type), CheckOneLine(pos, new int[2] { 1, -1 }, type), CheckOneLine(pos, new int[2] { 1, 1 }, type));
    }

    // 进行评分
    protected virtual float CheckOneLine(int[] pos, int[] offpos, ChessBoard.ChessType type)
    {
        string str = "a";
        // 正向检测：跳过第一次循环，避免重复存储监测点
        for (int i = pos[0] + offpos[0], j = pos[1] + offpos[1]; i >= 0 && i < ChessBoard.Max_LINE && j >= 0 && j < ChessBoard.Max_LINE; i += offpos[0], j += offpos[1])
        {
            // 如果对应当前所下的棋子颜色
            if (ChessBoard.Instance.GetDotType(i, j) == type.GetHashCode())
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
            if (ChessBoard.Instance.GetDotType(i, j) == type.GetHashCode())
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
        foreach (string ikey in scoreDic.Keys)
        {
            if (ikey.Equals(str))
            {
                temScore = scoreDic[ikey];
            }
        }
        return temScore;
    }
}
