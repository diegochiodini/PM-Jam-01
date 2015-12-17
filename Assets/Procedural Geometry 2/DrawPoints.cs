using UnityEngine;
using System.Collections;

public class DrawPoints : MonoBehaviour {
	
	public Transform pointPrefab;
	public bool useBadQuation = false;
	float radius = 1.0f;
	
	// Use this for initialization
	void Start () {
		
		if (useBadQuation) {
			for (float x = -1.0f; x <= 1.0001f; x+=0.1f) {
				// Don't allow the value to be negative or Sart returns NaN
				var inner = Mathf.Max(radius * radius - x * x, 0.0f);
				float y = Mathf.Sqrt(inner);
				
				Transform newPoint = (Transform)Instantiate(pointPrefab, new Vector3(x, y, 0.0f), pointPrefab.rotation);
				newPoint.parent = transform;
			}
		} else {
			float angle = 180.0f / (float)(20 - 1);
		
			for (int i = 0; i < 20; ++i) {
				Vector3 pos = Quaternion.AngleAxis(angle * (float)(i), Vector3.back) * -Vector3.right;
				Transform newPoint = (Transform)Instantiate(pointPrefab, pos, pointPrefab.rotation);
				newPoint.parent = transform;				
			}
		}
		
	}
}
