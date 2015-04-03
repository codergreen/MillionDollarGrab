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
	public int health = 3;
	public int maxHealth = 10;
	public Texture heartTex;

	void Start(){
		invItemType = new int[80];
		invItemAmount = new int[80];
		invItemType [0] = 1;
		invItemAmount [0] = 0;
	}
	public void OnGUI() {
		if (networkView.isMine) {
			GUI.TextArea (new Rect (0, 450, 100, 25), "My coins: " + invItemAmount [0]);
			if (health > maxHealth)
				health = maxHealth;
			for (int i=0; i<health; ++i)
				GUI.DrawTexture (new Rect (300 + i * 60, 0, 60, 60), heartTex, ScaleMode.ScaleToFit, true, 1.0F);
		}
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
						if (body.isKinematic && l == null)
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

						Pickup p = hit.collider.gameObject.GetComponent<Pickup> ();
						ClientNetwork cn = Camera.main.gameObject.GetComponent<ClientNetwork> ();
						BlargID bid = hit.collider.gameObject.GetComponent<BlargID>();
						if (p != null && p.type != 0&&cn!=null&&bid!=null) {
							networkView.RPC ("requestDestroyPickup", RPCMode.Server, new object[]{bid.myid, cn._myNetworkPlayer});	
						}



						Vector3 pushDir = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
						body.velocity = pushDir * pushPower;
		}
	public void ApplyDamage(){
		--health;
	}
	[RPC]
	void requestDestroyPickup(int myid, NetworkPlayer n){
		if(Network.isServer)
			networkView.RPC ("requestDestroyPickup", RPCMode.OthersBuffered, new object[]{myid,n});
		BlargID[] os = FindObjectsOfType(typeof(BlargID)) as BlargID[];
		foreach(BlargID o in os){
			if(o.myid==myid){
				Destroy(o.gameObject);
				if(Network.isServer){
					networkView.RPC ("AddPickup", RPCMode.OthersBuffered, new object[]{o.gameObject.GetComponent<Pickup> ().type, n});
				}
				break;
			}
		}
	}
	[RPC]
	void AddPickup(int type ,NetworkPlayer n){
		if (n == Camera.main.gameObject.GetComponent<ClientNetwork>()._myNetworkPlayer) {
			bool newItem = true;
			//Add grab and add to inventory
			for (int invSpot=0; invSpot<80; ++invSpot) {
				if (invItemType [invSpot] == type) {
					++invItemAmount [invSpot];

					newItem = false;
					break;
				}
			}
			if (newItem == true) {
				for (int invSpot=0; invSpot<80; ++invSpot) {
					if (invItemType [invSpot] == 0) {
						invItemType [invSpot] = type;
						++invItemAmount [invSpot];
						newItem = false;
						break;
					}
				}
			}
		}
	}

}