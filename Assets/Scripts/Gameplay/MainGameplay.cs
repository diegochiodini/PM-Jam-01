using UnityEngine;
using System.Collections;
using GameUtils;
using Data;

public class MainGameplay : MonoBehaviour, IGameplay
{
    public GameObject rollersContainer;
    public GameObject rollerTemplate;

    public Player player;
    public FollowTarget followTarget;

    private Camera gameCamera;
    private Vector3 viewportPosition;
    private GameplayStates _status;

    public GameplayStates GetStatus()
    {
        return _status;
    }

    public void SetStatus(GameplayStates status)
    {
        if (_status != status)
        {
            _status = status;
            switch (_status)
            {
                case GameplayStates.STARTED:
                    {
                        gameObject.SetActive(true);
                        Reset();
                        break;
                    }
                case GameplayStates.PLAYING:
                    {
                        gameObject.SetActive(true);
                        break;
                    }
                default:
                    {
                        gameObject.SetActive(false);
                        break;
                    }
            }
        }
    }

    void Awake()
    {

    }

    void Start()
    {
        SetStatus(GameplayStates.STARTED);
    }

    void Update()
    {
        if (GetStatus() != GameplayStates.PLAYING)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            Reset();
        }


        //check exit condition
        //SetStatus(GameplayStates.GAME_OVER);        
    }

    private void Reset()
    {
        //player.Reset();
        SetStatus(GameplayStates.PLAYING);
    }
}
