using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAsteroidSpawner : MonoBehaviour
{
    public MyAsteroid asteroidPrefab;       // 小行星预制体
    public float spawnDistance = 12f;     // 生成距离
    public float spawnRate = 1f;          // 生成频率(秒)
    public int amountPerSpawn = 1;        // 每次生成的数量
    [Range(0f, 45f)]
    public float trajectoryVariance = 15f; // 轨迹变化角度范围,控制小行星运动方向的随机偏移角度范围
    private void Start()
    {
        //游戏开始时，使用InvokeRepeating方法设置按照spawnRate的时间间隔重复调用Spawn方法
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    public void Spawn()
    {
        for (int i = 0; i < amountPerSpawn; i++)
        {
            // 生成小行星的逻辑
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            //Random.insideUnitCircle：返回单位圆内的随机点
            Vector3 spawnPoint = transform.position + (spawnDirection * spawnDistance);
            //•	计算在随机方向上距离生成器spawnDistance单位的生成点


            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            //•	在-trajectoryVariance到trajectoryVariance范围内随机选择一个角度值
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
            //•	使用Quaternion.AngleAxis方法创建一个绕Z轴旋转variance角度的四元数

            // 通过克隆预制件创建新小行星，并设置一个随机范围内的尺寸
            MyAsteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            // 设置轨迹以沿生成器的方向移动
            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }
}
