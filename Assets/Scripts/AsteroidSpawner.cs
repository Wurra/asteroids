using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;       // 小行星预制体
    public float spawnDistance = 12f;     // 生成距离
    public float spawnRate = 1f;          // 生成频率(秒)
    public int amountPerSpawn = 1;        // 每次生成的数量
    [Range(0f, 45f)]
    public float trajectoryVariance = 15f; // 轨迹变化角度范围

    private void Start()
    {
        //游戏开始时，使用InvokeRepeating方法设置按照spawnRate的时间间隔重复调用Spawn方法
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    public void Spawn()
    {
        for (int i = 0; i < amountPerSpawn; i++)
        {
            // Choose a random direction from the center of the spawner and
            // spawn the asteroid a distance away
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * spawnDistance);

            // Calculate a random variance in the asteroid's rotation which will
            // cause its trajectory to change
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            // Create the new asteroid by cloning the prefab and set a random
            // size within the range
            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            // Set the trajectory to move in the direction of the spawner
            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }

}
