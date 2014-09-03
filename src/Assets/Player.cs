using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	private float lastSyncTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPos = Vector3.zero;
	private Vector3 syncEndPos = Vector3.zero;

	public float speed = 10f;
	
	void Update()
	{
		if (networkView.isMine)
		{
			InputMovement();
		}
		else
		{
			SyncedMovement();
		}
	}

	void InputMovement()
	{
		if (Input.GetKey (KeyCode.W))
		{
			rigidbody.MovePosition(rigidbody.position + Vector3.forward * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S))
		{
			rigidbody.MovePosition(rigidbody.position + Vector3.back * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.A))
		{
			rigidbody.MovePosition(rigidbody.position + Vector3.left * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D))
		{
			rigidbody.MovePosition(rigidbody.position + Vector3.right * speed * Time.deltaTime);
		}
	}

	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rigidbody.position = Vector3.Lerp(syncStartPos, syncEndPos, syncTime / syncDelay);
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPos = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		
		if (stream.isWriting)
		{
			syncPos = rigidbody.position;
			stream.Serialize(ref syncPos);
			
			syncVelocity = rigidbody.velocity;
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPos);
			stream.Serialize(ref syncVelocity);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSyncTime;
			lastSyncTime = Time.time;			
		
			syncStartPos = rigidbody.position;
			syncEndPos = syncPos + syncVelocity * syncDelay;
		}
	}

}
