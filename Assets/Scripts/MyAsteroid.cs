using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAsteroid : MonoBehaviour
{
   public Sprite[] sprites; // Ԥ��ľ������飬Sprite���͵����飬���ڴ洢��ͬ�ľ���
   private SpriteRenderer spriteRenderer; // ������Ⱦ�����
   private Rigidbody2D rb; // �������
    public float size = 1f; // С���ǵĴ�С
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
        // ���ѡ��һ�����鲢���õ�������Ⱦ��
        if (sprites.Length > 0)
        {
            // �Ӿ������������ѡ��һ�����鸳ֵ��������Ⱦ��
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
        else
        {
            // ���û�з��侫�飬���������Ϣ
            Debug.LogWarning("No sprites assigned to MyAsteroid.");
        }
        // ���ó�ʼ��ת�Ƕ�(ŷ����Ҳ��Vector3ֵ)
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        transform.localScale = Vector3.one*size; // ���ó�ʼ����Ϊ1
        rb.mass = size; // ���ó�ʼ����Ϊ1
    }

    public void SetTrajectory(Vector2 direction)
    {
        // ����С���ǵ��˶��켣
        rb.AddForce(direction*movementSpeed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ��С�������������巢����ײʱ������С����
        if(collision.gameObject.CompareTag("Bullet"))
        {
            if( this.size* 0.5f >= minSize )
            {
                Split();
                Split();
            }
            MyGameManager.Instance.OnAsteroidDestroyed(this);
            Destroy(gameObject); // ����С����
        }
    }
    private void Split()
    {
        
        Vector2 position = transform.position; // ��ȡС���ǵ�ǰ��λ��
        position+= Random.insideUnitCircle * 0.1f; // �ڵ�ǰλ�ø�����΢ƫ��һ��
        // ����һ���µ�С����ʵ������������λ�ú���ת
        MyAsteroid newAsteroid = Instantiate(this,position, transform.rotation);
        newAsteroid.size *= 0.5f; // ��С���Ǵ�С����

        newAsteroid.SetTrajectory(Random.insideUnitCircle.normalized);
    }
}
