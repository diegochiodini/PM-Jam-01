using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset = Vector3.zero;

    private Vector3 cameraTarget = Vector3.zero;
    private Vector3 defaultPosition;

    void Awake()
    {
        defaultPosition = Camera.main.transform.position;
    }

    public void Reset()
    {
        cameraTarget = Vector3.zero;
        Camera.main.transform.position = defaultPosition;
    }

	void Update () 
    {
	    if (target.position.y > cameraTarget.y)
        {
            cameraTarget.y = target.position.y;
        }

        Camera.main.transform.position = cameraTarget + offset;
	}
}
