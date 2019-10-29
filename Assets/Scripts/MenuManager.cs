using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void SetPattern(int pattern)
    {
        PlayerPrefs.SetInt("Pattern", pattern);
        SceneManager.LoadScene((int)GameDefine.SCENE_NUM.SCENE_GAME);
    }
}
