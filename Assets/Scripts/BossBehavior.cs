using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossBehavior : MonoBehaviour
{
    private Canvas _canvas;  // 指定要生成 Image 的 Canvas
    private Transform player; // 目標玩家
    public GameObject[] WeaknessSpots;
    private GameObject WeaknessSpot;

    public bool[] isHit = { false, false, false, false, false };
    public GameObject AttackHitBox;
    public GameObject AttackHitBox2;
    public GameObject WeakImagePrefab;
    private GameObject[] WeakImage= { null, null, null, null, null };
    public float detectionRange; // 偵測範圍
    public float moveSpeed; // 移動速度
    public float turnSpeed; // 轉向速度
    public float attackRange; // 攻擊距離

    private Animator animator;
    private bool isChasing = false;
    private bool isSpotted = false;
    private bool isDead = false;
    public AudioClip DeadAudioClip;
    public GameObject Bosstext;


    void Start()
    {
        animator = GetComponent<Animator>(); // 獲取動畫控制器


        player = GameObject.Find("PlayerArmature").GetComponent<Transform>();
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();


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
        }

        if (isChasing)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (CheckAllTrue() && !isDead)
        {
            AudioSource.PlayClipAtPoint(DeadAudioClip, transform.position);
            animator.SetTrigger("isDead");
            isDead = true;
        }
    }

    private bool CheckAllTrue()
    {
        foreach (bool value in isHit)
        {
            if (!value)
            {
                return false;  // 如果有任何一個值是 false，返回 false
            }
        }
        return true;  // 所有值都是 true，返回 true
    }

    public void OnSpot()
    {
        if (isSpotted) return;
        isSpotted = true;
        int i = 0;
        foreach (bool value in isHit)
        {
            if (!value)
            {
                WeakImage[i] = Instantiate(WeakImagePrefab, _canvas.transform);
                WeaknessControll weaknessControll = WeakImage[i].GetComponent<WeaknessControll>();
                weaknessControll.target3DObject = WeaknessSpots[i].transform;
                WeaknessSpots[i].SetActive(true);
            }
            i++;
        }


        TriggerListener triggerListener0 = WeaknessSpots[0].GetComponent<TriggerListener>();
        if (triggerListener0 != null)
        {
            triggerListener0.OnTriggerEvent += HandleTriggerEnter0;
        }
        TriggerListener triggerListener1 = WeaknessSpots[1].GetComponent<TriggerListener>();
        if (triggerListener1 != null)
        {
            triggerListener1.OnTriggerEvent += HandleTriggerEnter1;
        }
        TriggerListener triggerListener2 = WeaknessSpots[2].GetComponent<TriggerListener>();
        if (triggerListener2 != null)
        {
            triggerListener2.OnTriggerEvent += HandleTriggerEnter2;
        }
        TriggerListener triggerListener3 = WeaknessSpots[3].GetComponent<TriggerListener>();
        if (triggerListener3 != null)
        {
            triggerListener3.OnTriggerEvent += HandleTriggerEnter3;
        }
        TriggerListener triggerListener4 = WeaknessSpots[4].GetComponent<TriggerListener>();
        if (triggerListener4 != null)
        {
            triggerListener4.OnTriggerEvent += HandleTriggerEnter4;
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
            if (!animator.GetBool("isAttacking") && !animator.GetBool("isAttacking2"))
            {


                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }

            animator.SetBool("isWalking", true);
        }
        else
        {
            if (!animator.GetBool("isAttacking") && !animator.GetBool("isAttacking2"))
            {
            AttackPlayer(); // 開始攻擊
            }
        }
    }

    private void AttackPlayer()
    {
        if (Random.Range(0, 2) < 1)
        {
            animator.SetBool("isAttacking2", true); // 播放攻擊動畫
        }
        else
        {
            animator.SetBool("isAttacking", true); // 播放攻擊動畫
        }

    }

    private void AttackHappen(AnimationEvent animationEvent)
    {
        AttackHitBox.SetActive(true);
    }


    private void AttackEnd(AnimationEvent animationEvent)
    {
        AttackHitBox.SetActive(false);
        animator.SetBool("isAttacking", false); // 停止攻擊
        animator.SetBool("isAttacking2", false);
    }

    private IEnumerator BossSay(string bossSay)
    {
        Bosstext.GetComponent<Text>().text = bossSay;
        Bosstext.SetActive(true);
        yield return new WaitForSeconds(3);
        Bosstext.SetActive(false);
    }

    void HandleTriggerEnter0(Collider other)
    {
        if (other.gameObject.name == "Mystic_hammer")
        {
            isHit[0] = true;
            Destroy(WeakImage[0]);
            AudioSource.PlayClipAtPoint(DeadAudioClip,transform.position);
            StartCoroutine(BossSay("補全你的籌碼 推出來All in"));
            WeaknessSpots[0].SetActive(false);
            
        }
    }
    void HandleTriggerEnter1(Collider other)
    {
        if (other.gameObject.name == "Mystic_hammer")
        {
            isHit[1] = true;
            Destroy(WeakImage[1]);
            AudioSource.PlayClipAtPoint(DeadAudioClip,transform.position);
            StartCoroutine(BossSay("要拿我跟你比 就是龜兔賽跑"));
            WeaknessSpots[1].SetActive(false);
        }
    }
    void HandleTriggerEnter2(Collider other)
    {
        if (other.gameObject.name == "Mystic_hammer")
        {
            isHit[2] = true;
            Destroy(WeakImage[2]);
            AudioSource.PlayClipAtPoint(DeadAudioClip,transform.position);
            StartCoroutine(BossSay("如果害怕就別來參與這遊戲"));
            WeaknessSpots[2].SetActive(false);
        }
    }
    void HandleTriggerEnter3(Collider other)
    {
        if (other.gameObject.name == "Mystic_hammer")
        {
            isHit[3] = true;
            Destroy(WeakImage[3]);
            AudioSource.PlayClipAtPoint(DeadAudioClip,transform.position);
            StartCoroutine(BossSay("快把你累積的實力 在舞台上揮灑"));
            WeaknessSpots[3].SetActive(false);
        }
    }
    void HandleTriggerEnter4(Collider other)
    {
        if (other.gameObject.name == "Mystic_hammer")
        {
            isHit[4] = true;
            Destroy(WeakImage[4]);
            AudioSource.PlayClipAtPoint(DeadAudioClip,transform.position);
            StartCoroutine(BossSay("年輕又沒資歷 別來參加"));
            WeaknessSpots[4].SetActive(false);
        }
    }



    void Die()
    {
        player.gameObject.GetComponent<PlayerController>().StartSlotAnimation(Random.Range(20, 22) * 10000);
        Destroy(this.gameObject);
        SceneManager.LoadScene("End");
    }
    

}
