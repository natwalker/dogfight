using UnityEngine;
using System.Collections;

public class CrashingScript : MonoBehaviour {
	public Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
        Debug.Log ("RB velocity = " + rb.velocity.ToString () + " " + transform.forward.ToString());
		Transform thePlane = transform.Find ("Mesh");
		Destroy(thePlane.GetComponent<Rigidbody>());
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
        //rb.AddForce (new Vector3( 0.0f, 9.6f * Time.deltaTime, 0.0f) + transform.forward * Time.deltaTime );
        Debug.Log(transform.position.ToString());
        if (transform.position.y <= 0.0f || Terrain.activeTerrain.SampleHeight(transform.position) >= transform.position.y)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
	}
	
}
