using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    [Header("Enemies")]
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private MotherShip MotherShipPrefab;

    [Header("Spawning")]
    [SerializeField] private float[] spawnDelayRange = new float[2] { 1, 5 };
    [SerializeField] private float spawnDistance = 500f;
    [SerializeField] private float firstShootDistance = 400f;
    [SerializeField] private float secondShootDistance = 200f;
    [SerializeField] private float enemyMaxLeft = -60;
    [SerializeField] private float enemyMaxRight = 60;
    [SerializeField] private float enemyMaxTop = 40;
    [SerializeField] private float enemyMaxBottom = -5;
    [SerializeField] private float bossSpawnSecond = 30;
    [SerializeField] private float spawnDelayAfterBoss = 20;
    [SerializeField] private int maxActiveAsteroids = 7;
    [SerializeField] private int maxActiveShips = 3;

    [Header("Game")]
    [SerializeField] private GameObject player;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private float gameDuration = 90f;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text gameTimerLabel;
    [SerializeField] private Canvas ui;

    private int asteroidsCount = 0;
    private int shipsCount = 0;
    private int asteroidsMissed = 0;
    private int shipsMissed = 0;
    private int gameTime;
    private bool shipDamaged = false;

    private int activeAsteroids;
    private int activeShips;

    public GameObject Player { get { return player; } }

    public Camera GameCamera { get { return gameCamera; } }
    public Canvas UI { get { return ui; } }

    public int AsteroidsCount
    {
        get { return asteroidsCount; }
        set { asteroidsCount = value; }
    }

    public int ShipsCount
    {
        get { return shipsCount; }
        set { shipsCount = value; }
    }

    public float CountDown { get; set; }

    public int Score
    {
        get { return DTO.CurrentScore; }
        set
        {
            DTO.CurrentScore = value;
            scoreLabel.text = string.Format("Score: {0}", DTO.CurrentScore);
        }
    }

    public int MaxEnemies { get { return maxActiveAsteroids + maxActiveShips + 1; } }

    public float SpawnDistance { get { return spawnDistance; } }
    public float FirstShootDistance { get { return firstShootDistance; } }
    public float SecondShootDistance { get { return secondShootDistance; } }
    public float EnemyMaxLeft { get { return enemyMaxLeft; } }
    public float EnemyMaxRight { get { return enemyMaxRight; } }
    public float EnemyMaxTop { get { return enemyMaxTop; } }
    public float EnemyMaxBottom { get { return enemyMaxBottom; } }

    public bool playerIsDead { get; set; }

    private bool mothershipSpawned;

    // Use this for initialization
    void Start()
    {
        //InvokeRepeating("GameCountDown", 0, 1);
        CountDown = gameDuration;
        activeAsteroids = 0;
        activeShips = 0;
        SpawnAsteroids();
        SoundManager.Instance.PlayBackground();

    }

    private void SpawnAsteroids()
    {
        activeAsteroids = maxActiveAsteroids;
        for (int i = 0; i<maxActiveAsteroids; i++)
        {
            Vector3 newPosition = Random.insideUnitSphere * Random.Range(50, 2000);
            Instantiate(enemies[0], newPosition, Quaternion.identity);
        }
    }

    //private void GameCountDown()
    //{
    //    gameTime++;
    //    if (--CountDown == 0)
    //    {
    //        CancelInvoke("GameCountDown");
    //        SoundManager.Instance.StopMusic();
    //        SceneController.Instance.FinalScore = Score;
    //        SceneController.Instance.FadeAndLoadScene("Score");
    //    };
    //    gameTimerLabel.text = NiceTime(CountDown);
    //}

    public string NiceTime(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void MissionEnd(bool meetAllCondition)
    {
        if (meetAllCondition)
        {
            // Win
        }
        else
        {
            // lose
        }
    }
}
