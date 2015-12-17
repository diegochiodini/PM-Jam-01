using System.Collections.Generic;

public class SocialConstants
{
    public const float REACHED = 100f;
	public const string GLOBAL_LEADERBOARD = "CgkI0Pm96N0REAIQBg";

    public static readonly KeyValuePair<int, string>[] LevelAchievements = 
    {
        new KeyValuePair<int, string>(50, "CgkI0Pm96N0REAIQEw"),
        new KeyValuePair<int, string>(100, "CgkI0Pm96N0REAIQEA"),
        new KeyValuePair<int, string>(200, "CgkI0Pm96N0REAIQEQ"),
        new KeyValuePair<int, string>(300, "CgkI0Pm96N0REAIQEg")
    };

    public static readonly KeyValuePair<int, string>[] ShotAchievements = 
    {
        new KeyValuePair<int, string>(10, "CgkI0Pm96N0REAIQAg"),
        new KeyValuePair<int, string>(100, "CgkI0Pm96N0REAIQBw"),
        new KeyValuePair<int, string>(1000, "CgkI0Pm96N0REAIQCA")
    };

    public static readonly KeyValuePair<int, string>[] DeathAchievements = 
    {
        new KeyValuePair<int, string>(10, "CgkI0Pm96N0REAIQAw"),
        new KeyValuePair<int, string>(100, "CgkI0Pm96N0REAIQDA"),
        new KeyValuePair<int, string>(1000, "CgkI0Pm96N0REAIQDQ")
    };
}
