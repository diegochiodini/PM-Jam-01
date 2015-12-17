using System;
using System.IO;
using System.Collections.Generic;

namespace Data
{
    public enum LevelRating
    {
        NONE = -1,
        ONE_STARS,
        TWO_STARS,
        THREE_STARS,
    }

    public enum LevelStates
    {
        UNLOCKED,
        COMPLETED,
        LOCKED,
    }

    public enum GameplayStates
    {
        STARTED,
        PLAYING,
        WIN,
        GAME_OVER,
        SUSPENDED, //game is no longer required so we can close it
        PAUSED, // game is frozen and resumable
    }

    public interface IGameplay
    {
        GameplayStates GetStatus();
        void SetStatus(GameplayStates status);
    }

    public interface LevelProviderInterface
    {
        int GetCurrentLevel();
        int GetTotalLevels();
        int GetLevelPerPage();
        int GetMaxLevelUnlocked();
        LevelRating GetLevelRating(int level);
        LevelStates GetLevelStatus(int level);
    }

    public interface SerializableInterface
    {
        object[] Serialize();
        void Deserialize(object[] objects);
    }
}