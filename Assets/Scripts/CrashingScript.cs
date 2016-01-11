using UnityEngine;
using System.Collections;

public class CrashingScript : MonoBehaviour {
	public Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		Debug.Log ("RB velocity = " + rb.velocity.ToString ());
		Transform thePlane = transform.Find ("sopwith_camel");
		thePlane.position = Vector3.zero;
		Destroy(thePlane.GetComponent<Rigidbody>());
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (transform.Find ("sopwith_camel").position.ToString ());
	}

	void UpdateFixed() {
		rb.AddForce (new Vector3( 0.0f, 9.6f, 0.0f));
	}
	
}
