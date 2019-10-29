using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILevelTwo : AILevelOne
{
    protected override void Start()
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

    protected override float CheckOneLine(int[] pos, int[] offpos, ChessBoard.ChessType type)
    {
        string str = "a";
        int allTimes = 0;
        bool isTurnLeft = false;
        bool leftStop = false;
        bool rightStop = false;

        // 跳过第一次循环，避免重复存储监测点
        int ri = pos[0]+offpos[0], rj = pos[1]+offpos[1];
        int li = pos[0]- offpos[0], lj = pos[1]- offpos[1];
        // 为了避免取到的均为空位，即“_”，所以限制取到空位时，调头取另一端所在位置的类型，若也是空位则再调头，就可取到有效的TimeLimit位数的str值
        while (allTimes < ChessBoard.TimeLimit&& (!leftStop || !rightStop))
        {
            // 正向检测
            if (!isTurnLeft)
            {
                if (ri >= 0 && ri < ChessBoard.Max_LINE && rj >= 0 && rj < ChessBoard.Max_LINE && !rightStop)
                {
                    // 如果对应当前所下的棋子颜色
                    if (ChessBoard.Instance.GetGrid()[ri, rj] == (int)type)
                    {
                        str += "a";
                        allTimes++;
                    }
                    // 如果对应为空棋子
                    else if (ChessBoard.Instance.GetGrid()[ri, rj] == (int)ChessBoard.DotType.NONE)
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
                    if (ChessBoard.Instance.GetGrid()[li, lj] == (int)type)
                    {
                        str = "a" + str;
                        allTimes++;
                    }
                    else if (ChessBoard.Instance.GetGrid()[li, lj] == (int)ChessBoard.DotType.NONE)
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
}
