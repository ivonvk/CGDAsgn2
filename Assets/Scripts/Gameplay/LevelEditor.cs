using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LevelData
{
    public List<string> Enemy = new List<string>();
    public List<int> EveryWavesEnemyIndexGenerate = new List<int>();
    public List<int> WavesTotalEnemy = new List<int>();

}
public class LevelEditor : SingleMgr<LevelEditor>
{
    public List<LevelData> Levels = new List<LevelData>();
    public LevelEditor()
    {
        Setup();
    }
    public void Setup()
    {

        Level_1_Editing();
        Level_2_Editing();
        Level_3_Editing();
        Level_4_Editing();
    }
    public List<LevelData> GetAllLevels()
    {
        return Levels;
    }
    void Level_1_Editing()
    {
        LevelData level = new LevelData();
        level.Enemy = new List<string> { "Cookies_1", "Cookies_1", "Cookies_1" };
        level.EveryWavesEnemyIndexGenerate = new List<int> { 0, 0, 0 };
        level.WavesTotalEnemy = new List<int> { 40,60, 80 };

        Levels.Add(level);
    }
    void Level_2_Editing()
    {
        LevelData level = new LevelData();
        level.Enemy = new List<string> { "Ghost_2", "Cookies_1", "Ghost_2" };
        level.EveryWavesEnemyIndexGenerate = new List<int> { 0, 0, 0 };
        level.WavesTotalEnemy = new List<int> { 20, 100, 60 };

        Levels.Add(level);
    }
    void Level_3_Editing()
    {
        LevelData level = new LevelData();
        level.Enemy = new List<string> { "Cookies_1", "Ghost_2", "Ghoul_3" };
        level.EveryWavesEnemyIndexGenerate = new List<int> { 0, 0, 0 };
        level.WavesTotalEnemy = new List<int> { 140, 60, 60 };

        Levels.Add(level);
    }
    void Level_4_Editing()
    {
        LevelData level = new LevelData();
        level.Enemy = new List<string> { "Alien_4", "Ghoul_3", "Alien_4" };
        level.EveryWavesEnemyIndexGenerate = new List<int> { 0, 0, 0 };
        level.WavesTotalEnemy = new List<int> { 30, 120, 80 };

        Levels.Add(level);
    }
}
