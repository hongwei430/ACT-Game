using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;  // 拖入主攝影機或在 Start 中自動取得
    public Image LockinImage;
    private float currentAlpha;  // 当前Alpha值
    private float targetAlpha;   // 目标Alpha值
    private float alphaChangeSpeed = 2.2f;  // 透明度变化速度

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (LockinImage != null)
        {
            currentAlpha = LockinImage.color.a;  // 初始化当前Alpha
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))  // 1 表示滑鼠右鍵
        {
            SetTargetAlpha(0.25f);
            Debug.Log("0.25");
        }
        if (Input.GetMouseButton(1))  // 1 表示滑鼠右鍵
        {
            InteractWithGolem();
        }
        if(Input.GetMouseButtonUp(1))
        {
            SetTargetAlpha(0);
            Debug.Log("0");
        }
        

        if (Mathf.Abs(currentAlpha - targetAlpha) > 0.01f)
        {
            currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * alphaChangeSpeed);
            SetAlpha(currentAlpha);
        }
    }
    public void SetTargetAlpha(float alpha)
    {
        targetAlpha = Mathf.Clamp(alpha, 0f, 1f);  // 确保目标Alpha在0到1之间
    }
    private void SetAlpha(float alpha)
    {
        if (LockinImage != null)
        {
            Color color = LockinImage.color;
            color.a = alpha;
            LockinImage.color = color;
        }
    }

    void InteractWithGolem()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1))
        {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("Golem"))
            {
                GolemBehavior golemScript = target.GetComponent<GolemBehavior>();
                if (golemScript != null)
                {
                    golemScript.OnSpot();  // 調用特定函式
                }
            }
        }
    }
}
