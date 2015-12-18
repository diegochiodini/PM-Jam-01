using Data;
using Extensions;
using GameUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class MainController : SingletonBehaviour<MainController>, IPointerClickHandler, SerializableInterface
{
    private const int AD_TRIGGER_COUNT = 5;
    private const int LOCAL_FILE_INDEX = 0;
    private const int REMOTE_FILE_INDEX = 1;
    private const float GAME_OVER_WAIT_TIME = 0f;

    public GameObject gameplayObject;
    public SimpleMenu menu;
    public GameObject testCanvas;
    public PaletteBehaviour palette;

    private LevelDescription currentLevelDescription;
    private GameLevelManager levelManager { get; set; }
    private IGameplay gameplay;

    private int gameOverCount = 0;
    private int lastLevelPlayed = -1;
    private bool interstitialShowRequest = false;
    private bool pauseInput = false;
    private int shotCount = 0;
    private int deathCount = 0;

    public bool isPaused
    {
        get
        {
            return gameplay.GetStatus() == GameplayStates.PAUSED;
        }

        private set { }
    }

    private void Awake()
    {
        Log.TimeStamp("STARTING GAME...");

        gameplay = gameplayObject.GetInterface<IGameplay>();

        Input.simulateMouseWithTouches = true;
        levelManager = new GameLevelManager();

        //menu.Loading();
        //StartCoroutine(LoadLevelData()); //TODO: create new level data to load

        Application.targetFrameRate = 30;
        testCanvas.SetActive(false);

        InitADS();

        //Util.Noise.Init();
    }

    private void Start()
    {
        LoadHome();
    }

    private void Update()
    {
#if DEBUG
        if (currentLevelDescription != null && !pauseInput)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //FireBullet();
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                StartCoroutine(GameOver());
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                StartCoroutine(Win());
            }
        }
#endif

        if (gameplay.GetStatus() == GameplayStates.GAME_OVER)
        {
            StartCoroutine(GameOver());
        }
    }

    private void OnDestroy()
    {
        DeinitADS();
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        Log.Print("Focus changes: " + focusStatus.ToString());
        if (!focusStatus)
        {
            OnLevelEnds(false);
        }
        else if (!Social.Active.localUser.authenticated)
        {
            InitSocial();
        }
    }

    #region Social & Ads

    private void InitADS()
    {
    }

    private void DeinitADS()
    {
    }

    private void InitSocial()
    {
        Log.Print("Init social...");
        Social.Active.localUser.Authenticate(GameUtils.Callbacks.ReceiveSuccess);
    }

    private void RequestInterstitial()
    {
        //Todo: Load your AD here.
    }

    private void OnInterstisialsLoaded()
    {
        if (interstitialShowRequest)
        {
            //Todo: Show your AD here.
        }
    }

    private void OnInterstisialsOpened()
    {
        Log.Print("OPEN AD");
        interstitialShowRequest = false;
    }

    private void OnInterstisialsClosed()
    {
        Log.Print("CLOSE AD");
        RequestInterstitial();
        PrepareToPlay();
    }

    #endregion Social & Ads

    private IEnumerator LoadLevelData()
    {
        string backupPath = Application.persistentDataPath + "/" + CommonConstants.LOCAL_LEVEL_DATA;
        string localPath = Application.streamingAssetsPath + "/" + CommonConstants.LOCAL_LEVEL_DATA;

        Log.Print("Loading " + (Debug.isDebugBuild ? "local" : "remote") + " data.");
        string remotePath = Debug.isDebugBuild ? Application.streamingAssetsPath + "/" + CommonConstants.DEV_LOCAL_LEVEL_DATA : CommonConstants.REMOTE_LEVEL_DATA;

        //TODO: this could be avoided removing the reference of mainController inside mainMenu
        yield return new WaitForEndOfFrame(); //wait for a frame to give time MainMenu to be initialized

        if (Validation.IsInternetConnectionAvailable())
        {
            Log.Print("Internet available");
            GameUtils.IO.ReadAllTextUbiquitus(this, new string[] { backupPath, remotePath }, AllFilesLoaded);
        }
        else if (System.IO.File.Exists(backupPath))
        {
            Log.Print("Internet not available, backup file exist");
            GameUtils.IO.ReadAllTextUbiquitus(this, backupPath, FinishLoadingGameData);
        }
        else
        {
            Log.Print("Internet not available, backup file NOT exist, loading default file.");
            GameUtils.IO.ReadAllTextUbiquitus(this, localPath, FinishLoadingGameData);
        }
    }

    private void AllFilesLoaded(string[] contents)
    {
        Log.Print("All file finished loading.");
        string localFile = contents[LOCAL_FILE_INDEX];
        string remoteFile = contents[REMOTE_FILE_INDEX];

        //not used. everything is done in PostLoading
        //if (localFile != null && !GameUtils.Validation.IsStringHashIdentical(localFile, remoteFile))
        //{
        //    //fix saved data
        //    Debug.LogWarning("You are loading a new set of data. Saved game must be fixed.");
        //}

        string localPath = Application.persistentDataPath + "/" + CommonConstants.LOCAL_LEVEL_DATA;
        Log.Print("Writing local level data to: " + localPath);
        System.IO.File.WriteAllText(localPath, remoteFile);
        FinishLoadingGameData(remoteFile);
    }

    private void FinishLoadingGameData(string text)
    {
        LoadGame();
        levelManager.LoadFromString(text);
        menu.Home();
    }

    private void LoadLevelDescription()
    {
        Log.Print("Loading level: " + currentLevelDescription.id);
        palette.SetPaletteForLevel(currentLevelDescription);
    }

    private void LoadLevel(int level)
    {
        LevelDescription description = levelManager.GetLevelDescription(level);
        if (description != null)
        {
            LoadLevel(description);
        }
        else
        {
            PrepareToPlay();
        }
    }

    private void LoadLevel(LevelDescription level)
    {
        GameObjectUtils.DestroyAllObjectsWithComponent<Bullet>(false);
        currentLevelDescription = level;
        lastLevelPlayed = level.id;
        LoadLevelDescription();

        PrepareToPlay();
    }

    private void PrepareToPlay()
    {
        RefreshUI();

        pauseInput = false;
        gameplay.SetStatus(GameplayStates.STARTED);
        menu.PlayGame();
    }

    public void Pause()
    {
        pauseInput = true;
        gameplay.SetStatus(GameplayStates.PAUSED);
    }

    public void Play()
    {
        pauseInput = false;
        gameplay.SetStatus(GameplayStates.PLAYING);
    }

    public void ResetGame()
    {
        LoadLevel(0);
    }

    public void RestartGame()
    {
        LoadLevel(levelManager.GetCurrentLevel());
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(levelManager.GetCurrentLevel());
    }

    public int GetCurrentLevel()
    {
        return levelManager.GetCurrentLevel();
    }

    public int GetScore()
    {
        return levelManager.GetScore();
    }

    private void SkipLevel()
    {
        Log.Print("Level skipped");
        levelManager.isSkippedCurrentLevel = true;
        menu.Loading();

#if (UNITY_ANDROID || UNITY_IOS) && !(UNITY_EDITOR)
        GoogleMobileAd.ShowInterstitialAd ();
        interstitialShowRequest = true;
#else
        OnInterstisialsClosed();
#endif
    }

    private void CancelSkipLevel()
    {
        menu.BackToPreviusMenu();
    }

    public LevelProviderInterface GetLevelProvider()
    {
        return levelManager;
    }

    protected void LoadHome()
    {
        if (lastLevelPlayed >= 0)
        {
            OnLevelEnds(false);
        }
        Pause();
        //menu.Home();
        OnPlayGameClick();
    }

    protected IEnumerator Win()
    {
        Log.Print("YOU WIN!");
        OnLevelEnds(true);
        pauseInput = true;
        //mainArea.PlayExplosionAnimation();
        yield return new WaitForSeconds(GAME_OVER_WAIT_TIME);

        levelManager.SetRating(LevelRating.NONE);
        if (levelManager.GetCurrentLevel() == levelManager.maxLevelUnlocked)
        {
            levelManager.UnlockNextLevel();
        }
        else
        {
            levelManager.AdvanceLevel();
        }
        menu.Win(GetCurrentLevel());
        //mainArea.Reset(false);
    }

    protected IEnumerator GameOver()
    {
        Log.Print("GAME OVER!");
        OnLevelEnds(false);
        Pause();

        deathCount++;
        foreach (KeyValuePair<int, string> pair in SocialConstants.DeathAchievements)
        {
            if (deathCount == pair.Key)
            {
                Social.Active.ReportProgress(pair.Value, SocialConstants.REACHED, Callbacks.ReceiveSuccess);
            }
        }

        yield return new WaitForSeconds(GAME_OVER_WAIT_TIME);

        menu.GameOver(GetCurrentLevel());

        if (++gameOverCount >= AD_TRIGGER_COUNT)
        {
            //Action[] actions = { new Action(SkipLevel), new Action(CancelSkipLevel) };
            //menu.MessageBox("Would you like to skip this level?", actions);
            gameOverCount = 0;
            TriggerMaxGameOver();
        }
    }

    protected virtual void TriggerMaxGameOver()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !(UNITY_EDITOR)
        interstitialShowRequest = true;
#else
        OnInterstisialsClosed();
#endif
    }

    private void OnHit(Bullet bullet, Collision2D collision)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (pauseInput)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            TouchInputModule tim = EventSystem.current.currentInputModule as TouchInputModule;

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    //FireBullet();
                    break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //FireBullet();
        }
    }

    private void OnLevelEnds(bool isComplete)
    {
        gameplay.SetStatus(GameplayStates.SUSPENDED);
        if (lastLevelPlayed >= 0)
        {
            SaveGame();
            lastLevelPlayed = -1;
        }
    }

    #region UI

    private void RefreshUI()
    {
    }

    public void OnCharacterSelection()
    {
        menu.CharacterSelection();
    }

    public void OnPlayGameClick()
    {
        LoadCurrentLevel();
        menu.PlayGame();
    }

    public void OnClickLeaderboards()
    {
        Log.Print("OpenLeaderboards");
        Social.Active.ShowLeaderboardUI();
    }

    public void OnClickAchievements()
    {
        Log.Print("Achievements");
        Social.Active.ShowAchievementsUI();
    }

    public void OnClickRateApp()
    {
        Log.Print("Star");
    }

    public void OnClickHome()
    {
        LoadHome();
    }

    public void OnClickRestart()
    {
        RestartGame();
        menu.Restart();
    }

    public void OnClickPause()
    {
        if (!isPaused)
        {
            Pause();
            menu.Pause();
        }
    }

    public void OnClickUnpause()
    {
        if (isPaused)
        {
            Play();
            menu.Unpause();
        }
    }

    public void OnClickLoadLevel(int level)
    {
        LoadLevel(level);
    }

    public void OnClickShareOnFB()
    {
    }

    public void OnClickShareOnTwitter()
    {
    }

    public void OnCLickScoreLabel()
    {
        menu.SetScore(GetScore());
    }

    #endregion UI

    private void LoadGame()
    {
        Log.Print("Loading game...");
        SerializableInterface[] serializableObjects = { levelManager, this };
        GameUtils.IO.DeserializeObjects(CommonConstants.LOCAL_SAVE_DATA, serializableObjects);
    }

    private void SaveGame()
    {
        Log.Print("Saving game...");
        SerializableInterface[] serializableObjects = { levelManager, this };
        GameUtils.IO.SerializeObjects(CommonConstants.LOCAL_SAVE_DATA, serializableObjects);
    }

    #region SerializableInterface

    public object[] Serialize()
    {
        object[] objects = { shotCount, deathCount };
        return objects;
    }

    public void Deserialize(object[] objects)
    {
        shotCount = (int)objects[0];
        deathCount = (int)objects[1];
    }

    #endregion SerializableInterface
}