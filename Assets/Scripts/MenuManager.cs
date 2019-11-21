using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void SetPattern(int pattern)
    {
        PlayerPrefs.SetInt("Pattern", pattern);
        if (pattern == (int)GameDefine.PATTERN.PATTERN_NET)
        {
            SceneManager.LoadScene((int)GameDefine.SCENE_NUM.SCENE_NET);
        }
        else
        {
            SceneManager.LoadScene((int)GameDefine.SCENE_NUM.SCENE_GAME);
        }
    }
}
