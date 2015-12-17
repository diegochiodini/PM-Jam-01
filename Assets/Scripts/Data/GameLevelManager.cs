using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using GameUtils;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Assertions;

public class GameLevelManager : LevelDatabase, LevelProviderInterface, SerializableInterface
{
    [Serializable]
    public struct LevelData
    {
        public bool isSkipped;
        public LevelRating rating;
        public LevelStates status;

        public void Init()
        {
           isSkipped = false;
           rating = LevelRating.NONE;
#if UNLOCK_LEVELS
            status = LevelStates.UNLOCKED;
#else
           status = LevelStates.LOCKED;
#endif
        }
    }

    private LevelData[] levelData;

    public bool isSkippedCurrentLevel
    {
        get
        {
            return levelData[currentLevel].isSkipped;
        }

        set
        {
            Assert.IsTrue(levelData[currentLevel+1].status == LevelStates.LOCKED, "You can skip only a unlocked, not completed level");

            bool oldValue = levelData[currentLevel].isSkipped;
            if (!oldValue && value)
            {
                levelData[currentLevel].isSkipped = value;
                UnlockNextLevel();
            }
            else
            {
                Debug.LogError("You can't set false a skipped level or skip it more than once. Only new level can be false.");
            }
        }
    }

    public GameLevelManager()
    {
    }

    public int GetScore()
    {
        int count = 0;
        foreach (LevelData level in levelData)
        {
            count += level.status == LevelStates.COMPLETED ? 1 : 0;
        }
        return count;
    }

    public override void PostLoading() 
    {
        if (levelData == null)
        {
            //first time we run the load, let's create the levels
            Assert.IsTrue(levelCount > 0, "You must have at least one level to load.");
            levelData = new LevelData[levelCount];

            for (int i = 0; i < levelCount; i++ )
            {
                levelData[i].Init();
            }
            levelData[0].status = LevelStates.UNLOCKED; //init first level
        }
        else if (levelCount != levelData.Length)
        {
            //adjust saved data to the new file
            LevelData[] clone = new LevelData[levelCount];
            int count = Mathf.Min(levelCount, levelData.Length);
            Array.Copy(levelData, clone, count);

            //skip if the new array is smaller than the saved one
            for (int i = count; i < levelCount; i++)
            {
                clone[i].Init();
            }

            levelData = clone;
        }

        Assert.IsTrue(levelData.Length == levelCount, "You are loading an old set of data. Delete profile.dat please.");
    }

    public override void UnlockNextLevel()
    {
        base.UnlockNextLevel();
        levelData[currentLevel].status = LevelStates.UNLOCKED;
        int score = GetScore();
        Social.Active.ReportScore(score, SocialConstants.GLOBAL_LEADERBOARD, GameUtils.Callbacks.ReceiveSuccess);

        foreach (KeyValuePair<int, string> pair in SocialConstants.LevelAchievements)
        {
            if (score == pair.Key)
            {
                Social.Active.ReportProgress(pair.Value, SocialConstants.REACHED, Callbacks.ReceiveSuccess);
            }
        }
    }

    #region Level Provider

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetTotalLevels()
    {
        return levelCount;
    }

    public int GetLevelPerPage()
    {
        return int.MaxValue;
    }

    public int GetMaxLevelUnlocked()
    {
        return maxLevelUnlocked;
    }

    private void SetCurrentLevel()
    {
        for (int i = 0; i < levelData.Length; i++)
        {
            if (levelData[i].status == LevelStates.UNLOCKED)
            {
                currentLevel = i;
                if (currentLevel > maxLevelUnlocked)
                {
                    maxLevelUnlocked = currentLevel;
                }
            }
        }
    }

    public LevelRating GetLevelRating(int level)
    {
        return levelData[level].rating;
    }

    public LevelStates GetLevelStatus(int level)
    {
        return levelData[level].status;
    }

    public void SetRating(LevelRating rating)
    {
        levelData[currentLevel].rating = rating;
        levelData[currentLevel].status = LevelStates.COMPLETED;
    }

    #endregion

    #region SerializableInterface

    public object[] Serialize()
    {        
        object[] objects = { levelData };
        return objects;
    }

    public void Deserialize(object[] objects)
    {
        levelData = (LevelData[])objects[0];
        SetCurrentLevel();
    }

    #endregion
}
