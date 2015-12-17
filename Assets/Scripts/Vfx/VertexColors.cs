using UnityEngine;
using System.Collections;

public class VertexColors : MonoBehaviour 
{
    public Color startColor = Color.red;
    public Color endColor = Color.blue;

    protected Color cacheStartColor;
    protected Color cacheEndColor;

	void Awake () 
    {
        ApplyColors();
	}	

    void OnEnable()
    {
        ApplyColors();
    }

    public void ApplyColors()
    {
        cacheStartColor = startColor;
        cacheEndColor = endColor;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] colors = new Color[mesh.vertices.Length];
        colors[0] = startColor;
        colors[1] = endColor;
        colors[2] = startColor;
        colors[3] = endColor;
        mesh.colors = colors;
    }

    void Update()
    {
        if (cacheStartColor != startColor || cacheEndColor != endColor)
        {
            ApplyColors();
        }
    }
}
