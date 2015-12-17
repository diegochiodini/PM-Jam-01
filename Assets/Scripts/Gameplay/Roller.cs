using UnityEngine;
using System.Collections;

public class Roller : Rotation 
{
    public void CreateGeometry(float radius, float angleStep)
    {
        ProceduralGeometry.AddToObjectCircleMeshAndCollider(gameObject, radius, 8);
        this.angleStep = angleStep;
    }
}
