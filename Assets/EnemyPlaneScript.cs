using UnityEngine;
using System;
using System.Collections;

public class EnemyPlaneScript : MonoBehaviour {
    public float speed = 50.0f;
    public float heading = 0.0f;
    public float pitch = 0.0f;
    public float headingMultiplier = 4.0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        heading -= pitch * headingMultiplier;
        if (heading < -180.0)
            heading += 360.0f;
        if (heading > 180.0)
            heading -= 360.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, heading, pitch * 180));
        var newDirection = Quaternion.Euler((float)Math.Sin((double)heading * Math.PI), 0.0f, (float)Math.Cos(heading * Math.PI));
        transform.position += transform.forward * Time.deltaTime * speed;

    }
}
