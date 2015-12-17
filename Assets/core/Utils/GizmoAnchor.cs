using UnityEngine;
using System.Collections;

public class GizmoAnchor : MonoBehaviour 
{
	public float radius = 1f;
	public Color color = Color.red;

	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, radius);
	}
}
