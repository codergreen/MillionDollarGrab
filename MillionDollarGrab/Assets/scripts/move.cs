using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class move : MonoBehaviour {
	public float speed = 3.0F;
	public float ladderSpeed = 1.0F;
	public float rotateSpeed = 3.0F;
	public bool toggle = true;
	public float pushPower = 2.0F;
	public GameObject cameraHolder;
	float waitasec;
	int[] invItemType;
	int[] invItemAmount;
	//bool touchLadder = false;

	void Start(){
		invItemType = new int[80];
		invItemAmount = new int[80];
	}
	public void OnGUI() {
		GUI.TextArea(new Rect(0, 450, 100, 25), "My coins: "+invItemAmount[0]);
	}

	void Update() {
		if (networkView.isMine) {
			if (Input.GetAxis ("Camera") != 0 && Time.realtimeSinceStartup - waitasec > 1.0f) {
				toggle = !toggle;
				waitasec = Time.realtimeSinceStartup;
			}
			CharacterController controller = GetComponent<CharacterController> ();
			transform.Rotate (0, Input.GetAxis ("Mouse X") * rotateSpeed, 0);
			cameraHolder.transform.Rotate (-1 * Input.GetAxis ("Mouse Y") * rotateSpeed, 0, 0);
			if (cameraHolder.transform.eulerAngles.x > 60.0f && cameraHolder.transform.eulerAngles.x < 300.0f)
				cameraHolder.transform.Rotate (-Input.GetAxis ("Mouse Y") * rotateSpeed, 0, 0);
			Vector3 forward = transform.TransformDirection (Vector3.forward);
			Vector3 right = transform.TransformDirection (Vector3.right);
			float curSpeed = speed * Input.GetAxis ("Vertical");
			float curSpeed2 = speed * Input.GetAxis ("Horizontal");
			controller.SimpleMove (forward * curSpeed + right * curSpeed2);
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
						Rigidbody body = hit.collider.attachedRigidbody;
						if (body == null)
								return;
		
						Lock l = hit.collider.gameObject.GetComponent<Lock> ();
						Ladder l2 = hit.collider.gameObject.GetComponent<Ladder> ();
						if (body.isKinematic && l == null && l2 == null)
								return;

						//if (hit.moveDirection.y < -0.3F)
								//return;

						if (l != null && body.isKinematic) {
								//Add grab and add to inventory
								for (int invSpot=0; invSpot<80; ++invSpot) {
										if (invItemType [invSpot] == 2) {
												--invItemAmount [invSpot];
												body.isKinematic = false;
												//Destroy(hit.collider.gameObject);
												break;
										}
								}
								return;
						}
						if (l2 != null) {
								//touchLadder = true;
								return;
						}

						Pickup p = hit.collider.gameObject.GetComponent<Pickup> ();
						if (p != null && p.type != 0) {
								bool newItem = true;
								//Add grab and add to inventory
								for (int invSpot=0; invSpot<80; ++invSpot) {
										if (invItemType [invSpot] == p.type) {
												++invItemAmount [invSpot];
												networkView.RPC ("requestDestroy", RPCMode.All, new object[]{hit.collider.gameObject.GetComponent<BlargID>().myid});
												newItem = false;
												break;
										}
								}
								if (newItem == true) {
										for (int invSpot=0; invSpot<80; ++invSpot) {
												if (invItemType [invSpot] == 0) {
														invItemType [invSpot] = p.type;
														++invItemAmount [invSpot];
														networkView.RPC ("requestDestroy", RPCMode.All, new object[]{hit.collider.gameObject.GetComponent<BlargID>().myid});
														newItem = false;
														break;
												}
										}
								}
						}



						Vector3 pushDir = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
						body.velocity = pushDir * pushPower;
		}
	[RPC]
	void requestDestroy(int myid){
		BlargID[] os = FindObjectsOfType(typeof(BlargID)) as BlargID[];
		foreach(BlargID o in os){
			if(o.myid==myid){
				Destroy(o.gameObject);
				break;
			}
		}
	}

}