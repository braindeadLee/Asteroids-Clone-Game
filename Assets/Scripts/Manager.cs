using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Manager : MonoBehaviour
{
    //Singleton class implementation, n!gga
    public static Manager Instance { get; private set; }

    //instances of sub-manager scripts to be used
    public EnemyManager enemyManager;
    public AsteroidUIManager UIManager;

    public GameObject spaceship;

    private Vector3 _spaceshipPosition = Vector3.zero;
    private Quaternion _spaceshipRotation = Quaternion.Euler(0, 0, 0);

    public int CurrentLevelCount = 0;
    public int PlayerLives = 3;
    private bool _winFlag = false;

    private int _asteroidCount = 3;
    private float _asteroidAccelerationMultiplier = 1f;

    private int _score = 0;
    public int LifeUpScore = 10000;
    private int NextLifeUpCounter = 0;
    private int _highscore = 0;

    public BoxCollider2D _SafezoneCollider;

    private void OnEnable()
    {
        AsteroidUIManager.OnStartClicked += StartClicked;
        SpaceshipLife.OnDeath += ReviveSpaceship;

        //Small asteroids trigger function to check if all asteroids are cleared to proceed to next level
        EnemyManager.AsteroidDeath += NextLevel;

        EnemyManager.AsteroidScore += UpdateScore;
    }

    private void OnDisable()
    {
        AsteroidUIManager.OnStartClicked -= StartClicked;
        SpaceshipLife.OnDeath -= ReviveSpaceship;
        EnemyManager.AsteroidDeath -= NextLevel;
        EnemyManager.AsteroidScore -= UpdateScore;
    }


    private void Awake() => Instance = this;
    //Create singleton instance of AsteroidManager

    private void Start()
    {
        StartGame();
        SaveSystem.Load();
    }

    private void StartGame()
    {
        UIManager.StartingScreen();
        StartingAsteroids();
        CurrentLevelCount = 0;
        PlayerLives = 3;
        _score = 0;
        _winFlag = false;
    }
    //Asteroids that spawn only for the title screen
    private void StartingAsteroids()
    {
        enemyManager.DestroyAsteroids();
        enemyManager.SpawnAsteroids(6);
    }

    //Start Button clicked, triggering beginning of game loop
    private void StartClicked()
    {
        enemyManager.DestroyAsteroids();

        Invoke(nameof(SpawnSpaceship), 2f);
        Invoke(nameof(StartLevel), 2f);

        CurrentLevelCount = 0;
        PlayerLives = 3;
    }

    //Sets the parameters of the level based on LevelDatabase data that matches with the CurrentLevelCount, and prepares to move on to next level by incrementing CurrentLevelCount++
    private void StartLevel()
    {
        //If the current level is not beyond the amount of levels currently stored

        if (CurrentLevelCount < LevelDatabase.levels.Length)
        {
            LevelData currentLevel = LevelDatabase.levels[CurrentLevelCount];
            _asteroidCount = currentLevel.AsteroidCount;
            _asteroidAccelerationMultiplier = currentLevel.AccelerationSpeedMultiplier;
            UIManager.UpdateLivesText(PlayerLives);
            UIManager.UpdateScoreText(_score);
            UIManager.UpdateHighscoreText(_highscore);

            GameObject[] spaceship = GameObject.FindGameObjectsWithTag("Player");
            _SafezoneCollider.GetComponent<BoxCollider2D>().transform.position = spaceship[0].transform.position;

            enemyManager.SetMultiplier(_asteroidAccelerationMultiplier);
            enemyManager.SpawnAsteroids(_asteroidCount);
            CurrentLevelCount++;
        } else
        // else, no more levels so you Win
        {
            Win();
        }
    }

    // Triggered everytime a small asteroid is destroyed, if all asteroids are gone then move on to next level
    private void NextLevel()
    {
        if (enemyManager.AreThereAsteroids() == false)
        {
            Invoke(nameof(StartLevel), 1f);
        }
    }

    private void SpawnSpaceship() => Instantiate(spaceship, _spaceshipPosition, _spaceshipRotation);

    private void ReviveSpaceship()
    {
        if (PlayerLives > 0 && !(_winFlag))
        {
            Invoke(nameof(SpawnSpaceship), 2f);
            PlayerLives--;
            UIManager.UpdateLivesText(PlayerLives);
        }
        else
        {
            Lose();
        }
    }

    private void Win()
    {
        UIManager.UpdateAnnouncementText("You win!");
        PlayerLives = 0;

        _winFlag = true;
        GameObject[] spaceship = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ship in spaceship)
        {
            Destroy(ship);
        }
        
        SaveSystem.Save();

        Invoke(nameof(StartGame), 2f);

    }

    private void Lose()
    {
        UIManager.UpdateAnnouncementText("Game Over");
        enemyManager.DestroyAsteroids();
        
        SaveSystem.Save();

        Invoke(nameof(StartGame), 2f);
    }

    private void UpdateScore(int score)
    {
        _score += score;
        NextLifeUpCounter += score;
        //Example: _score = 1080, NextLifeUpCounter = 1080, LifeUpScore = 1000
        // 1. NextLifeUpCounter >= LifeUpScore -> True
        // 2. NextLifeUpCounter = 1080 - 1000 = 80
        // 3.Score remains 1080 and continues to increase, NextLifeUpCounter is now 80 and goes back to 1000
        // 4. In Summary: Player still retains max score and gets a new life every 1000 points

        if (NextLifeUpCounter >= LifeUpScore)
        {   
            NextLifeUpCounter -= LifeUpScore;
            UIManager.UpdateLivesText(++PlayerLives);
        }
        UIManager.UpdateScoreText(_score);

        if(_score >= _highscore)
        {
            _highscore = _score;
            UIManager.UpdateHighscoreText(_highscore);
        }
    }

    public void Save(ref HighScoreData data)
    {
        data.Highscore = _highscore;
    }
    public void Load(HighScoreData data)
    {
        _highscore = data.Highscore;
    }

    
}

[System.Serializable]
public struct HighScoreData
{
    public int Highscore;
}