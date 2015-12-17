using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CommonConstants
{
    public const string GAME_FILE_VERSION = "0.0.1";

    public const string SCENE_MAIN = "Main";

    //input
    public const string DEV_LOCAL_LEVEL_DATA =  "ballyball_data - Development.csv";
    public const string LOCAL_LEVEL_DATA = "level_data.csv";
    public const string LOCAL_SAVE_DATA = "playerProfile.dat";
    public const string LOCAL_SOCIAL_DATA = "ballyball_data - Social.csv";

    //output
    public const string LOCAL_SOCIAL_CONST = "./Assets/Data/SocialConstants.h";
    public const string LOCAL_RANDOM_LEVEL_DATA = "./build/randomLevels.csv";

    public const string REMOTE_LEVEL_DATA = "https://www.dropbox.com/s/ngxevg4z3oixnym/ballyball_data%20-%20Production.csv?dl=1";
    public const string LABEL_SCORE = "high score: ";

#if UNITY_ANDROID
    public const string AD_UNIT_ID = "ca-app-pub-2942273842190883/7220643853";
#elif UNITY_IPHONE
    public const string AD_UNIT_ID = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
#else
    public const string AD_UNIT_ID = "unexpected_platform";
#endif

    public const string TIME_FORMAT_SAFE = "yyyy-MM-dd_HH-mm-ss";
}
