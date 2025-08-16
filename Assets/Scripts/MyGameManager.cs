using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
   
    private MyPlayer player; // 玩家对象
    public int lives = 3; // 玩家生命数
    public ParticleSystem explosionEffect;
    public int score = 0; // 玩家得分
    //必须要是静态的，以便在其他脚本中访问
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
        //直接用lives=3 这种方式直接修改成员变量的值会导致UI文本更新有误
        SetLives(3);
        //为啥能直接调用Respawn方法，因为它是public的
        Respawn();
    }

    public void Death(MyPlayer player)
    {
        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();

        // 保存player引用到成员变量，但凡是传入变量了，都要使用到下一行的类似语句！！！非常重要，直接导致了后边的函数是否被调用
        this.player = player;
        
        // 禁用玩家对象而不是游戏管理器
        player.gameObject.SetActive(false);

        // 当玩家死亡时，减少生命数
        SetLives(lives-1);
        if (lives <= 0)
        {
            // 如果生命数小于等于0，游戏结束
            // 在这里可以添加游戏结束逻辑，比如显示游戏结束界面
           Gameover();
        }
        else
        {
            Invoke(nameof(Respawn), 2f); // 延迟2秒后调用Respawn方法
        }
    }
    
    public void Respawn()
    {
         if (player != null)
        {
            // 如果玩家对象存在，重置位置并激活
            player.transform.position = Vector2.zero; // 重置玩家位置
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
        // 更新得分
        this.score = score;
        //ToString() 方法将整数转换为字符串形式，因为UI文本组件只能显示字符串
        scoreText.text = score.ToString();
    }
    public void Gameover()
    {
        // 显示游戏结束UI
        gameOverUI.SetActive(true);

        //这个好用！！可以找到场景中的对象，只要它们的名字是唯一的
        //GameObject.Find的局限性:
        //只能查找激活状态的游戏对象
        //区分大小写，必须完全匹配名称
        //gameOverUI = GameObject.Find("GameOver");

       

    }
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }
}
