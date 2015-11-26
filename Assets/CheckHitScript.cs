using UnityEngine;
using System.Collections;

public class CheckHitScript : MonoBehaviour {
    public BasicPlaneScript plane;
	// Use this for initialization
	void Start () {
        plane = transform.parent.GetComponent<BasicPlaneScript>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "CausesDamage")
        {
            plane.IsHit();
        }
    }
}
