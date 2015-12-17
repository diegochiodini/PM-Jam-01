using UnityEngine;
using UnityEngine.UI;

public class HudMenu : MonoBehaviour 
{
    public Text tapLabel;

    void OnEnable()
    {
        tapLabel.enabled = true;
    }

	void Update () 
    {
        bool mouseTouch = false;
#if UNITY_EDITOR || UNITY_STANDALONE
        mouseTouch = Input.GetMouseButtonUp(0);
#endif
	    if (tapLabel.isActiveAndEnabled && ((Input.touchCount > 0) || mouseTouch))
        {
            tapLabel.enabled = false;
        }
	}
}
