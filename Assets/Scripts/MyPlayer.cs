using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    //在Unity的2D物理系统中，向刚体添加扭矩时：
    //负扭矩(-1f)导致顺时针旋转
    //正扭矩(1f)导致逆时针旋转

    
    private bool forwardDirection;//创建一个布尔值来储存变量是否摁下前进键
    private float turnDirection;

    public MyBullet bulletprefab; // 子弹预制体
    Rigidbody2D rb;


    public float forwardSpeed = 1f; //前进速度
    public float rotationSpeed = 0.1f; //旋转速度

    public MyGameManager myGameManager; // 游戏管理器引用
    public float respawnInvulnerability = 3f;

    private void Awake()
    {
        myGameManager = FindObjectOfType<MyGameManager>();
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        forwardDirection=Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ;

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
        {
            turnDirection = 1f; //逆时针旋转
        } 
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
        {
            turnDirection = -1f; //顺时针旋转
        } 
        else 
        {
            turnDirection = 0f; //不旋转
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0)) // 假设使用空格和左键发射子弹
        {
           Shoot(); // 调用射击方法
            
        }
    }
    private void FixedUpdate()
    {
        //如果前进键被按下
        if (forwardDirection) 
        {
            rb.AddForce(transform.up * forwardSpeed);
        }
        //如果有旋转输入
        if (turnDirection != 0f) 
        {
            //向刚体添加扭矩
           rb.AddTorque(turnDirection * rotationSpeed);
        }
    }
    private void Shoot()
    {
        // 实例化子弹,创建实例对象，因为Instantiate会返回一个GameObject
        MyBullet bullet = Instantiate(bulletprefab, transform.position, transform.rotation);
        // 传递方向向量
        bullet.Shoot(transform.up);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // 当玩家与小行星发生碰撞时，玩家死亡
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            //this.gameObject.SetActive(false);
            // 这里可以添加游戏结束逻辑,当游戏结束时，可以调用GameManager的相关方法来处理游戏状态，玩家存活，得分事件
            // 例如，调用GameManager的死亡方法
            //若是用下方的调用会发现无法正常工作,必须使用单例模式的Instance来访问GameManager或者委托什么的
            myGameManager.Death(this);
            //MyGameManager.Instance.Death(this);
            
        }
    }
    //这是无敌时间的实现
    private void TurnOffCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
    }

    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void OnEnable()
    {
        // Turn off collisions for a few seconds after spawning to ensure the
        // player has enough time to safely move away from asteroids
        TurnOffCollisions();
        Invoke(nameof(TurnOnCollisions), respawnInvulnerability);
    }
    
}
