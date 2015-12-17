using UnityEngine;
using System;

namespace Utils
{
	public class CameraUtils
	{
		public static Vector3 FrustrumSizeAtHeight(Camera camera, float height)
		{
			Vector3 viewPort = new Vector3(0,0,height);
			Vector3 bottomLeft = camera.ViewportToWorldPoint(viewPort);
			viewPort.Set(1,1,height);
			Vector3 topRight = camera.ViewportToWorldPoint(viewPort);
			return topRight - bottomLeft;
		}
	}
}

