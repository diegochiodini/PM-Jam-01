using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour 
{
	public float angleStep = 1f;

    private bool run = true;

    public void Start()
    {
        run = true;
    }

    public void Stop()
    {
        run = false;
    }

	void Update () 
	{
        if (run)
        {
            transform.Rotate(0f, 0f, angleStep * Time.deltaTime);
        }
	}
}
