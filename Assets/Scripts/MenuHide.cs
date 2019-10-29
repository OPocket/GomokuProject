using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHide : MonoBehaviour
{
    public void DoHide()
    {
        gameObject.SetActive(false);
        ChessBoard.Instance.IsGameOver = false;
    }
}
