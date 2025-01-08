using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaknessControll : MonoBehaviour
{
    public Camera mainCamera;  // 主攝影機
    public Transform target3DObject;  // 3D 物件
    public RectTransform uiElement;  // UI 中的 Image 或其他元素
    public float minScale;  // 最小縮放比例
    public float maxScale;  // 最大縮放比例
    public float minDistance;  // 縮放的最近距離
    public float maxDistance;  // 縮放的最遠距離
    public RectTransform image1;  // 第一個 Image
    public RectTransform image2;  // 第二個 Image

    public float rotationSpeed = 50f;  // 旋轉速度
    public float moveRangeNum;
    private Vector2 moveRange = new Vector2(10f, 10f);  // 移動範圍
    public float moveSpeed = 2f;  // 移動平滑度 (越小越平滑)

    private Vector2 targetPosition1;  // 目標位置1
    private Vector2 targetPosition2;  // 目標位置2

    private Vector2 offsetPosition1;  
    private Vector2 offsetPosition2;  

    private float lerpTime1 = 0f;
    private float lerpTime2 = 0f;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;  // 自動獲取主攝影機
        }
    }

    void Update()
    {
        if (mainCamera == null || target3DObject == null || uiElement == null)
            return;

        // 將 3D 世界座標轉換為螢幕空間座標
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target3DObject.position);
        

        // 如果物件在攝影機視野內
        if (screenPos.z > 0)
        {
            uiElement.gameObject.SetActive(true);  // 顯示 UI 元素
            uiElement.position = screenPos + new Vector3(offsetPosition1.x,offsetPosition1.y,0);  // 更新 UI 元素位置並加上偏移
            image2.gameObject.SetActive(true);  // 顯示 UI 元素
            image2.position = screenPos + new Vector3(offsetPosition2.x,offsetPosition2.y,0);  // 更新 UI 元素位置並加上偏移

            float distance = Vector3.Distance(mainCamera.transform.position, target3DObject.position);
            float scaleFactor = Mathf.Lerp(maxScale, minScale, Mathf.InverseLerp(minDistance, maxDistance, distance));
            moveRange = scaleFactor * moveRangeNum * new Vector2(1f, 1f);
            uiElement.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            image2.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        else
        {
            uiElement.gameObject.SetActive(false);  // 隱藏 UI 元素
            image2.gameObject.SetActive(false);  // 隱藏 UI 元素
        }

        RotateImage(uiElement);
        RotateImage(image2);

        MoveImageWithLerp(uiElement, ref targetPosition1, ref lerpTime1, ref offsetPosition1);
        MoveImageWithLerp(image2, ref targetPosition2, ref lerpTime2, ref offsetPosition2);
    }

    private void RotateImage(RectTransform image)
    {
        image.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void MoveImageWithLerp(RectTransform image, ref Vector2 targetPosition, ref float lerpTime, ref Vector2 offsetPosition)
    {
        lerpTime += Time.deltaTime * moveSpeed;
        offsetPosition = Vector2.Lerp(offsetPosition, targetPosition, lerpTime);

        if (Vector2.Distance(offsetPosition, targetPosition) < 1f)
        {
            SetRandomTargetPosition(ref targetPosition);
            lerpTime = 0f;  // 重置 Lerp 時間
        }
    }

    private void SetRandomTargetPosition(ref Vector2 targetPosition)
    {
        Vector2 randomOffset = new Vector2(Random.Range(-moveRange.x, moveRange.x), Random.Range(-moveRange.y, moveRange.y));
        targetPosition = randomOffset;
    }
}
