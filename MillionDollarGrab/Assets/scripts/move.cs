using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class move : MonoBehaviour {
	public float speed = 3.0F;
	public float rotateSpeed = 3.0F;
	public bool toggle;
	public GameObject cameraObject;
	public float pushPower = 2.0F;
	public GameObject cameraHolder;
	float waitasec;
	int[] invItemType;
	int[] invItemAmount;

	void Start(){
		invItemType = new int[80];
		invItemAmount = new int[80];
	}

	void Update() {
		if (Input.GetAxis ("Camera")!= 0 && Time.realtimeSinceStartup - waitasec > 1.0f) {
		if (toggle) {
			cameraObject.transform.localPosition = new Vector3(0.0f,0.0f,1.0f);
		} else {
			cameraObject.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
		}
		toggle=!toggle;
		waitasec=Time.realtimeSinceStartup;
		}
		CharacterController controller = GetComponent<CharacterController>();
		transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);
		cameraHolder.transform.Rotate(Input.GetAxis ("Mouse Y") * rotateSpeed, 0, 0);
		if(cameraHolder.transform.eulerAngles.x > 60.0f && cameraHolder.transform.eulerAngles.x < 300.0f)
			cameraHolder.transform.Rotate( -Input.GetAxis ("Mouse Y") * rotateSpeed, 0, 0);
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);
		float curSpeed = speed * Input.GetAxis("Vertical");
		float curSpeed2 = speed * Input.GetAxis("Horizontal");
		controller.SimpleMove(forward * curSpeed + right * curSpeed2);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null)
			return;
		
		Lock l = hit.collider.gameObject.GetComponent<Lock> ();
		if(body.isKinematic && l == null)
			return;

		if (hit.moveDirection.y < -0.3F)
			return;

		if(l != null && body.isKinematic){
			//Add grab and add to inventory
			for(int invSpot=0;invSpot<80;++invSpot){
				if(invItemType[invSpot]==2){
					--invItemAmount[invSpot];
					body.isKinematic=false;
					//Destroy(hit.collider.gameObject);
					break;
				}
			}
			return;
		}

		Pickup p = hit.collider.gameObject.GetComponent<Pickup> ();
		if(p != null && p.type!=0){
			bool newItem=true;
			//Add grab and add to inventory
			for(int invSpot=0;invSpot<80;++invSpot){
				if(invItemType[invSpot]==p.type){
					++invItemAmount[invSpot];
					Destroy(hit.collider.gameObject);
					newItem=false;
					break;
				}
			}
			if(newItem==true){
				for(int invSpot=0;invSpot<80;++invSpot){
					if(invItemType[invSpot]==0){
						invItemType[invSpot]=p.type;
						++invItemAmount[invSpot];
						Destroy(hit.collider.gameObject);
						newItem=false;
						break;
					}
				}
			}
		}



		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = pushDir * pushPower;
	}
}