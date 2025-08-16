using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAsteroid : MonoBehaviour
{
   public Sprite[] sprites; // 预设的精灵数组，Sprite类型的数组，用于存储不同的精灵
   private SpriteRenderer spriteRenderer; // 精灵渲染器组件
   private Rigidbody2D rb; // 刚体组件
    public float size = 1f; // 小行星的大小
    public float minSize = 0.35f;
    public float maxSize = 1.65f;
    public float movementSpeed = 5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 随机选择一个精灵并设置到精灵渲染器
        if (sprites.Length > 0)
        {
            // 从精灵数组中随机选择一个精灵赋值给精灵渲染器
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
        else
        {
            // 如果没有分配精灵，输出警告信息
            Debug.LogWarning("No sprites assigned to MyAsteroid.");
        }
        // 设置初始旋转角度(欧拉角也是Vector3值)
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        transform.localScale = Vector3.one*size; // 设置初始缩放为1
        rb.mass = size; // 设置初始质量为1
    }

    public void SetTrajectory(Vector2 direction)
    {
        // 设置小行星的运动轨迹
        rb.AddForce(direction*movementSpeed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 当小行星与其他物体发生碰撞时，销毁小行星
        if(collision.gameObject.CompareTag("Bullet"))
        {
            if( this.size* 0.5f >= minSize )
            {
                Split();
                Split();
            }
            MyGameManager.Instance.OnAsteroidDestroyed(this);
            Destroy(gameObject); // 销毁小行星
        }
    }
    private void Split()
    {
        
        Vector2 position = transform.position; // 获取小行星当前的位置
        position+= Random.insideUnitCircle * 0.1f; // 在当前位置附近稍微偏移一点
        // 创建一个新的小行星实例，并设置其位置和旋转
        MyAsteroid newAsteroid = Instantiate(this,position, transform.rotation);
        newAsteroid.size *= 0.5f; // 将小行星大小减半

        newAsteroid.SetTrajectory(Random.insideUnitCircle.normalized);
    }
}
