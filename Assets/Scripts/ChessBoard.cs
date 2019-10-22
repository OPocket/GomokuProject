using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public ChessType curTurn= ChessType.Black;

    // 地图格子
    public int Max_LINE= 15;
    private int[,] grid;

    private bool isGameOver = false;

    public enum ChessType
    {
        Watch,
        White,
        Black
    }

    private void Awake()
    {
        grid = new int[Max_LINE, Max_LINE];
    }

    private void Update()
    {
        if (isGameOver) return;
    }

    private void PlayChess()
    {

    }

}
