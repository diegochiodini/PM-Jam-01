using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Extensions;
using GameUtils;

public class InputForwarder : MonoBehaviour, IPointerClickHandler
{
    public Transform target;

    public void OnPointerClick(PointerEventData eventData)
    {
        Log.Print("POINTER CLICK CHILD");

        if (target == null)
        {
            target = transform.parent;
        }

        while (target != null)
        {
            IPointerClickHandler clickHandler = target.gameObject.GetInterface<IPointerClickHandler>();
            if (clickHandler != null)
            {
                clickHandler.OnPointerClick(eventData);
                break;
            }
            else
            {
                target = target.transform.parent;
            }
        }
    }
}
