using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Data
{
    public interface LevelButtonInterface
    {
        void SetState(LevelStates status);
        void SetLevel(int level);
        void SetRating(LevelRating stars);
    }
}