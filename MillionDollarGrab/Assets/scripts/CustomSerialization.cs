using UnityEngine;
using System.Collections;

public class CustomSerialization : MonoBehaviour 
{
	int health;
	private void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			health -= 10;
		
		if (Input.GetKey (KeyCode.UpArrow))
			transform.position += Vector3.up * Time.deltaTime * 5;
		
		if (Input.GetKey (KeyCode.DownArrow))
			transform.position -= Vector3.up * Time.deltaTime * 5;
	}
	
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting) 
		{
			Vector3 pos = transform.position;
			stream.Serialize (ref health);
			stream.Serialize(ref pos);
		} 
		else 
		{
			Vector3 pos = Vector3.zero;
			stream.Serialize (ref health);
			stream.Serialize(ref pos);
			transform.position = pos;
		}
	}
}