using UnityEngine;
using System.Collections.Generic;
using Data;
using System.IO;

public abstract class LevelDatabase
{
    public virtual void PreLoading() { }
    public virtual void PostLoading() { }

    public TextAsset gameData;

    private List<LevelDescription> levelDescriptions;
    public int maxLevelUnlocked { get; protected set; }

    protected int currentLevel;
    
    public int levelCount
    {
        get
        {
            return levelDescriptions.Count;
        }
    }

    public void LoadFromString(string text)
    {
        CSVReader reader = new CSVReader();
        reader.LoadFile(text);
        PreLoading();
        levelDescriptions = reader.ParseObjects<LevelDescription>();
        PostLoading();
    }

    public void LoadFromFile(string path)
    {
        StreamReader fileReader = new StreamReader(path);
        string text = fileReader.ReadToEnd();
        LoadFromString(text);
    }

    public virtual void AdvanceLevel()
    {
        currentLevel++;
    }

    public virtual void UnlockNextLevel()
    {
        maxLevelUnlocked = Mathf.Clamp(++maxLevelUnlocked, 0, levelDescriptions.Count - 1);
        currentLevel = maxLevelUnlocked;
    }

    public LevelDescription GetCurrentLevelDescription()
    {
        return levelDescriptions != null && levelDescriptions.Count > 0 ? levelDescriptions[currentLevel] : null;
    }

    public LevelDescription GetLevelDescription(int index)
    {
        currentLevel = index;
        return GetCurrentLevelDescription();
    }
}
