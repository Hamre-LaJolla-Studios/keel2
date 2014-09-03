using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera/Tracking Behavior")]
public class TrackingBehavior : MonoBehaviour {

	public GameObject target;
	
	public float distance;
	public float height;

	void Update()
	{
		Vector3 new_position;
		Transform target_transform;
	
		if (target == null)
		{
			return;
		}
		
		target_transform = target.transform;
		
		new_position = target_transform.position;
		
		new_position.y += height;
		new_position -= target_transform.rotation * Vector3.forward * distance;
		
		transform.position = new_position;
		
		transform.LookAt(target.transform);
	}
}
