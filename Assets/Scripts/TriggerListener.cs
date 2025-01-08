using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerListener : MonoBehaviour
{
    public delegate void TriggerEvent(Collider other);
    public event TriggerEvent OnTriggerEvent;

    void OnTriggerEnter(Collider other)
    {
        // 當觸發進來時，觸發事件
        OnTriggerEvent?.Invoke(other); 
    }
}
