using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    
    public float speed = 10f;
    public float maxLifetime = 5f; // 子弹最大存活时间
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
       
    }
    //传参这个做法真的太聪明了
    public void Shoot(Vector2 direction)
    { 
        rb.velocity = direction * speed;
        Destroy(gameObject, maxLifetime);
    }
    
    
}
