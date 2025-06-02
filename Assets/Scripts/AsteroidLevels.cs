using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelData
{
    public int AsteroidCount;
    public float AccelerationSpeedMultiplier;
    public string LevelName;

    //Constructor
    public LevelData(string levelName, int asteroidCount, float accelerationSpeedMultiplier)
    {
        this.LevelName = levelName;
        this.AsteroidCount = asteroidCount;
        this.AccelerationSpeedMultiplier = accelerationSpeedMultiplier;
    }

}
public static class LevelDatabase
{
    public static LevelData[] levels = new LevelData[]
        {
        new LevelData("Level 1", 6, 1f),
        new LevelData("Level 2", 10, 1.5f),
        new LevelData("Level 3", 14, 2.0f),
        //new LevelData("Level 4", 8, 1.5f),
        //new LevelData("Level 5", 8, 1.7f),
        //new LevelData("Level 6", 9, 1.7f),
        //new LevelData("Level 7", 10, 2f),
        };
}