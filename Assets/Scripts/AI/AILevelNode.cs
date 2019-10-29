using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILevelNode
{
    public float score = 0.0f;
    public int[] pos;
    public List<AILevelNode> child;
    public ChessBoard.ChessType chessType;
}
