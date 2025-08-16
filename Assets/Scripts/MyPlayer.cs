using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    //��Unity��2D����ϵͳ�У���������Ť��ʱ��
    //��Ť��(-1f)����˳ʱ����ת
    //��Ť��(1f)������ʱ����ת

    
    private bool forwardDirection;//����һ������ֵ����������Ƿ�����ǰ����
    private float turnDirection;

    public MyBullet bulletprefab; // �ӵ�Ԥ����
    Rigidbody2D rb;


    public float forwardSpeed = 1f; //ǰ���ٶ�
    public float rotationSpeed = 0.1f; //��ת�ٶ�

    public MyGameManager myGameManager; // ��Ϸ����������
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
            turnDirection = 1f; //��ʱ����ת
        } 
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
        {
            turnDirection = -1f; //˳ʱ����ת
        } 
        else 
        {
            turnDirection = 0f; //����ת
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0)) // ����ʹ�ÿո����������ӵ�
        {
           Shoot(); // �����������
            
        }
    }
    private void FixedUpdate()
    {
        //���ǰ����������
        if (forwardDirection) 
        {
            rb.AddForce(transform.up * forwardSpeed);
        }
        //�������ת����
        if (turnDirection != 0f) 
        {
            //��������Ť��
           rb.AddTorque(turnDirection * rotationSpeed);
        }
    }
    private void Shoot()
    {
        // ʵ�����ӵ�,����ʵ��������ΪInstantiate�᷵��һ��GameObject
        MyBullet bullet = Instantiate(bulletprefab, transform.position, transform.rotation);
        // ���ݷ�������
        bullet.Shoot(transform.up);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // �������С���Ƿ�����ײʱ���������
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            //this.gameObject.SetActive(false);
            // ������������Ϸ�����߼�,����Ϸ����ʱ�����Ե���GameManager����ط�����������Ϸ״̬����Ҵ��÷��¼�
            // ���磬����GameManager����������
            //�������·��ĵ��ûᷢ���޷���������,����ʹ�õ���ģʽ��Instance������GameManager����ί��ʲô��
            myGameManager.Death(this);
            //MyGameManager.Instance.Death(this);
            
        }
    }
    //�����޵�ʱ���ʵ��
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
