using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public void LoadSecondScene()
    {
        SceneManager.LoadScene("Stage1");  // 使用場景名稱
        // 或 SceneManager.LoadScene(1);  // 使用場景索引
    }

    // 退出遊戲
    public void QuitGame()
    {
        Debug.Log("遊戲退出");  // 方便在編輯器中確認呼叫成功
        Application.Quit();
    }
}
