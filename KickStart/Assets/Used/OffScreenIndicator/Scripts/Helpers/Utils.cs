using UnityEngine;
using System.Collections;

namespace Greyman{
	static public class Utils 
	{
		/// <summary>
		/// Draws the plane.
		/// Draws a debug plane.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="normal">Normal.</param>
		static public void DrawPlane(Vector3 normal, Vector3 position, float radius){
			Vector3 v3;
			
			if (normal.normalized != Vector3.forward)
				v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude * radius;
			else
				v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude * radius;
			
			Vector3 corner0 = position + v3;
			Vector3 corner2 = position - v3;
			Quaternion q = Quaternion.AngleAxis(90.0f, normal);
			v3 = q * v3;
			Vector3 corner1 = position + v3;
			Vector3 corner3 = position - v3;

			Debug.DrawLine(corner0, corner2, Color.green);
			Debug.DrawLine(corner1, corner3, Color.green);
			Debug.DrawLine(corner0, corner1, Color.green);
			Debug.DrawLine(corner1, corner2, Color.green);
			Debug.DrawLine(corner2, corner3, Color.green);
			Debug.DrawLine(corner3, corner0, Color.green);
			Debug.DrawRay(position, normal, Color.blue);
		}
	}
}