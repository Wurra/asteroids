using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
   
    private MyPlayer player; // ��Ҷ���
    public int lives = 3; // ���������
    public ParticleSystem explosionEffect;
    public int score = 0; // ��ҵ÷�
    //����Ҫ�Ǿ�̬�ģ��Ա��������ű��з���
    public static MyGameManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    public GameObject gameOverUI;
   
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }

    }
    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }
    public void NewGame()
    {
        MyAsteroid[] asteroids = FindObjectsOfType<MyAsteroid>();

        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }

        gameOverUI.SetActive(false);

        SetScore(0);
        //ֱ����lives=3 ���ַ�ʽֱ���޸ĳ�Ա������ֵ�ᵼ��UI�ı���������
        SetLives(3);
        //Ϊɶ��ֱ�ӵ���Respawn��������Ϊ����public��
        Respawn();
    }

    public void Death(MyPlayer player)
    {
        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();

        // ����player���õ���Ա�����������Ǵ�������ˣ���Ҫʹ�õ���һ�е�������䣡�����ǳ���Ҫ��ֱ�ӵ����˺�ߵĺ����Ƿ񱻵���
        this.player = player;
        
        // ������Ҷ����������Ϸ������
        player.gameObject.SetActive(false);

        // ���������ʱ������������
        SetLives(lives-1);
        if (lives <= 0)
        {
            // ���������С�ڵ���0����Ϸ����
            // ��������������Ϸ�����߼���������ʾ��Ϸ��������
           Gameover();
        }
        else
        {
            Invoke(nameof(Respawn), 2f); // �ӳ�2������Respawn����
        }
    }
    
    public void Respawn()
    {
         if (player != null)
        {
            // �����Ҷ�����ڣ�����λ�ò�����
            player.transform.position = Vector2.zero; // �������λ��
            player.gameObject.SetActive(true);
        }
    }
    public void OnAsteroidDestroyed(MyAsteroid asteroid)
    {
        explosionEffect.transform.position = asteroid.transform.position;
        explosionEffect.Play();
        if (asteroid.size < 0.7f)
        {
            SetScore(score + 100); // small asteroid
        }
        else if (asteroid.size < 1.4f)
        {
            SetScore(score + 50); // medium asteroid
        }
        else
        {
            SetScore(score + 25); // large asteroid
        }
    }
    private void SetScore(int score)
    {
        // ���µ÷�
        this.score = score;
        //ToString() ����������ת��Ϊ�ַ�����ʽ����ΪUI�ı����ֻ����ʾ�ַ���
        scoreText.text = score.ToString();
    }
    public void Gameover()
    {
        // ��ʾ��Ϸ����UI
        gameOverUI.SetActive(true);

        //������ã��������ҵ������еĶ���ֻҪ���ǵ�������Ψһ��
        //GameObject.Find�ľ�����:
        //ֻ�ܲ��Ҽ���״̬����Ϸ����
        //���ִ�Сд��������ȫƥ������
        //gameOverUI = GameObject.Find("GameOver");

       

    }
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }
}
