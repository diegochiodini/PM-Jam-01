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
        gameCamera = followTarget.gameObject.GetComponent<Camera>();
        Vector3 position = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            position.y = 2f * i;
            CreateRoller(position, 0.3f, 90f);
        }
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

        viewportPosition = gameCamera.WorldToViewportPoint(player.transform.position);
        if (!Validation.VectorComponentsInRange(viewportPosition, 0f, 1f))
        {
            SetStatus(GameplayStates.GAME_OVER);
        }
    }

    private void Reset()
    {
        player.Reset();
        followTarget.Reset();
        SetStatus(GameplayStates.PLAYING);
    }

    private Roller CreateRoller(Vector3 position, float radius, float angularVelocity)
    {
        GameObject rollerObject = GameObject.Instantiate(rollerTemplate);
        Roller roller = rollerObject.GetComponent<Roller>();
        roller.CreateGeometry(radius, angularVelocity);
        rollerObject.transform.parent = rollersContainer.transform;
        rollerObject.transform.position = position;
        return roller;
    }
}
