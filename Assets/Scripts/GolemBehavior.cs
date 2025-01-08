using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehavior : MonoBehaviour
{
    private Canvas _canvas;  // 指定要生成 Image 的 Canvas
    private Transform player; // 目標玩家
    public GameObject[] WeaknessSpots;
    private GameObject WeaknessSpot;
    public GameObject AttackHitBox;
    public GameObject WeakImagePrefab;
    private GameObject WeakImage;
    public float detectionRange; // 偵測範圍
    public float moveSpeed; // 移動速度
    public float turnSpeed; // 轉向速度
    public float attackRange; // 攻擊距離

    private Animator animator;
    private bool isChasing = false;
    private bool isSpotted = false;
    private bool isDead = false;
    public AudioClip DeadAudioClip;


    void Start()
    {
        animator = GetComponent<Animator>(); // 獲取動畫控制器


        player = GameObject.Find("PlayerArmature").GetComponent<Transform>();
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        WeaknessSpot = WeaknessSpots[Random.Range(0, 4)];WeaknessSpot = WeaknessSpots[Random.Range(0, 4)];

    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true; // 進入追逐狀態
        }
        else
        {
            isChasing = false;
            isSpotted = false;
            Destroy(WeakImage);
            WeaknessSpot.SetActive(false);
        }

        if (isChasing)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }


    public void OnSpot()
    {
        if (isSpotted) return;
        isSpotted = true;
        WeakImage = Instantiate(WeakImagePrefab, _canvas.transform);
        WeaknessControll weaknessControll = WeakImage.GetComponent<WeaknessControll>();

        
        weaknessControll.target3DObject = WeaknessSpot.transform;
        WeaknessSpot.SetActive(true);

        TriggerListener triggerListener = WeaknessSpot.GetComponent<TriggerListener>();
        if (triggerListener != null)
        {
            triggerListener.OnTriggerEvent += HandleTriggerEnter;
        }
    }



    private void ChasePlayer(float distanceToPlayer)
    {
        // 旋轉面向玩家
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);


        // 移動至玩家
        if (distanceToPlayer > attackRange && !isDead)
        {
            if (!animator.GetBool("isAttacking"))
            {


                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }

            animator.SetBool("isWalking", true);
        }
        else
        {
            AttackPlayer(); // 開始攻擊
        }
    }

    private void AttackPlayer()
    {
        animator.SetBool("isAttacking", true); // 播放攻擊動畫
    }

    private void AttackHappen(AnimationEvent animationEvent)
    {
        AttackHitBox.SetActive(true);
    }


    private void AttackEnd(AnimationEvent animationEvent)
    {
        AttackHitBox.SetActive(false);
        animator.SetBool("isAttacking", false); // 停止攻擊
    }

    void HandleTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Mystic_hammer")
        {
            AudioSource.PlayClipAtPoint(DeadAudioClip,transform.position);
            animator.SetTrigger("isDead");
            isDead = true;
        }
    }

    void Die()
    {
        player.gameObject.GetComponent<PlayerController>().StartSlotAnimation(Random.Range(1, 22)*100);
        Destroy(WeakImage);
        Destroy(this.gameObject);
    }

}
