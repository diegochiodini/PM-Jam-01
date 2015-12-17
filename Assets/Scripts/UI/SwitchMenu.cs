using UnityEngine;
using System.Collections;

public class SwitchMenu : MonoBehaviour 
{
    private int _activeChild = 0;
    protected int activeChild
    {
        get
        {
            return _activeChild;
        }

        set
        {
            _previousActiveChild = _activeChild;
            _activeChild = value;
			RefreshChildren(_activeChild);
        }
    }

    private int _previousActiveChild = 0;
    protected int previousActiveChild
    {
        get
        {
            return _previousActiveChild;
        }

        private set{}
    }

    public void BackToPreviusMenu()
    {
        activeChild = previousActiveChild;
    }

    public GameObject GetActiveObject()
    {
        return transform.GetChild(activeChild).gameObject;
    }

    public T GetActiveObjectComponent<T>() where T : Component
    {
        GameObject menu = GetActiveObject();
        return menu.GetComponentInChildren<T>();
    }

	protected virtual void RefreshChildren(int index)
	{
		int i = 0;
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(i++ == index);
		}
	}

#if UNITY_EDITOR
    private int previousChild = -1;
    public int editorActiveChild = 0;

    public void EditorMenuForward()
    {
        editorActiveChild = (editorActiveChild + 1) % transform.childCount;
    }

    public void EditorMenuBackward()
    {
        int index = editorActiveChild - 1;
        if (index < 0)
        {
            index = transform.childCount - 1;
        }
        editorActiveChild = index % transform.childCount;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying && (previousChild != editorActiveChild))
		{
            RefreshChildren(editorActiveChild);
            previousChild = editorActiveChild;
		}
    }
#endif
}
