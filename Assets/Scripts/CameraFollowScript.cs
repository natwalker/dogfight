using UnityEngine;
using System.Collections;
using System;

public class CameraFollowScript : MonoBehaviour {
    public float posBack = 30.0f;
    public float posUp = 5.0f;
    public float lookForwards = 5.0f;
    public float damping = 0.98f;
	// Use this for initialization
	void Start () {
        Vector3 moveCamTo = transform.position - transform.forward * posBack + transform.up * posUp;
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position + transform.forward * 5.0f);
    }

    // Update is called once per frame
    void Update() {

    }

    void LateUpdate () {
        Vector3 forEuler = transform.eulerAngles;
        Vector3 behindPos = transform.position + new Vector3(0.0f, posUp, 0.0f) - transform.forward * posBack;
        Camera.main.transform.position = Camera.main.transform.position * damping + behindPos * (1.0f - damping);
        Camera.main.transform.LookAt(transform.position + transform.forward * lookForwards);
	}
}
