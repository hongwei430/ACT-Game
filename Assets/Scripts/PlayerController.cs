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
    public AudioClip CoinAudioClip;
    public Text slotText;  // Text 元件
    public float targetNumber = 100;  // 最終顯示的數字
    public float scrollSpeed = 0.05f;  // 每次變更的時間間隔
    private float currentNumber = 0;
    public GameObject Boss;
    public GameObject Bosstext;
    public bool SeeBoss = false;

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
        }
        if (Input.GetMouseButton(1))  // 1 表示滑鼠右鍵
        {
            InteractWithGolem();
        }
        if (Input.GetMouseButtonUp(1))
        {
            SetTargetAlpha(0);
        }


        if (Mathf.Abs(currentAlpha - targetAlpha) > 0.01f)
        {
            currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * alphaChangeSpeed);
            SetAlpha(currentAlpha);
        }

        if (currentNumber > 35000 && !SeeBoss)
        {
            SeeBoss = true;
            StartCoroutine(ToBoss());
            Boss.SetActive(true);
            Bosstext.SetActive(true);
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

    public void StartSlotAnimation(float newTargetNumber)
    {
        targetNumber += newTargetNumber;
        StartCoroutine(AnimateSlotNumber());
        AudioSource.PlayClipAtPoint(CoinAudioClip, transform.position);
    }
    private IEnumerator ToBoss()
    {

        yield return new WaitForSeconds(3);
        Bosstext.SetActive(false);
        transform.position = new Vector3 (140,10,40);
    }

    private IEnumerator AnimateSlotNumber()
    {
        while (currentNumber != targetNumber)
        {
            // 模擬數字滾動效果
            currentNumber = Mathf.MoveTowards(currentNumber, targetNumber, 100);
            slotText.text = "¥" + currentNumber.ToString();
            yield return new WaitForSeconds(scrollSpeed);
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
            if (target.CompareTag("Boss"))
            {
                BossBehavior bossScript = target.GetComponent<BossBehavior>();
                if (bossScript != null)
                {
                    bossScript.OnSpot();  // 調用特定函式
                }
            }
        }
    }
}
