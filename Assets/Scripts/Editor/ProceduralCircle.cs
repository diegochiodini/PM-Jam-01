using UnityEngine;
using UnityEditor;

public class ProceduralCircle : EditorWindow 
{
    private string radiusString = "1.0";
    private string segmentsString = "3";

    [MenuItem("GameObject/Create Other/Circle")]
    public static void Init()
    {
        EditorWindow window = GetWindow<ProceduralCircle>();
        window.Show();
    }

    private void CreateCircle(float radius, int segments)
    {
        GameObject circleObj = new GameObject("Circle");
        MeshFilter circleMesh = (MeshFilter)circleObj.AddComponent<MeshFilter>();
        circleObj.AddComponent<MeshRenderer>();

        circleMesh.sharedMesh = ProceduralGeometry.CreateCircleMesh(radius, segments);

        int index = Random.Range(0, int.MaxValue);
        AssetDatabase.CreateAsset(circleMesh.sharedMesh, "Assets/circle" + index.ToString() + ".asset");
        circleMesh.GetComponent<Renderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Material/tex_test.mat");
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Radius");
        radiusString = EditorGUILayout.TextField(radiusString);
        float radius;
        if (!float.TryParse(radiusString, out radius))
        {
            Debug.LogError("You must provide a valid radius value");
        }

        GUILayout.Label("Segments");
        segmentsString = EditorGUILayout.TextField(segmentsString);
        int segments;
        if (!int.TryParse(segmentsString, out segments))
        {
            Debug.LogError("You must provide a valid segment number");
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create Circle"))
        {
            CreateCircle(radius, segments);
        }
    }
}
