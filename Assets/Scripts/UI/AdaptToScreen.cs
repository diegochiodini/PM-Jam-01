using UnityEngine;
using System.Collections;
using GameUtils;

public class AdaptToScreen : MonoBehaviour 
{
    public float deltaPercentage = 1f;
    void Awake()
    {
        GraphicsUtils.ResizeRendererToScreen<MeshRenderer>(gameObject, deltaPercentage);
    }
}
