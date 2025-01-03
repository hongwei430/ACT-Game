using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string World1; // 要加载的场景名称
    private bool isPlayerNearby = false; // 检测玩家是否在门附近

    void Update()
    {
        // 检查玩家是否在门附近且按下 E 键
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            LoadScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检测进入触发器的是否是玩家
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player is near the door. Press 'E' to enter.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 当玩家离开触发器时
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left the door.");
        }
    }

    private void LoadScene()
    {
        // 切换到指定场景
        if (!string.IsNullOrEmpty(World1))
        {
            Debug.Log($"Loading scene: {World1}");
            SceneManager.LoadScene("World 1");
        }
        else
        {
            Debug.LogError("Scene name is not set!");
        }
    }
}
