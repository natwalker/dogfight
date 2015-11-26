using UnityEngine;
using System.Collections;
using System;

public class CameraFollowScript : MonoBehaviour {
    public float posBack = 10.0f;
    public float posUp = 30.0f;
    public float lookForwards = 10.0f;
    public float damping = 0.5f;
    private Vector3 cameraVelocity = Vector3.zero;
	// Use this for initialization
	void Start () {
        Vector3 moveCamTo = transform.position - transform.forward * posBack + transform.up * posUp;
        Camera.main.transform.position = moveCamTo;
        Camera.main.transform.LookAt(transform.position + transform.forward * lookForwards);
    }

    // Update is called once per frame
    void Update() {

    }

    void LateUpdate () {
        Vector3 forEuler = transform.eulerAngles;
        Vector3 behindPos = transform.position + new Vector3(0.0f, posUp, 0.0f) - transform.forward * posBack;

        float actualDamping = damping * 30.0f / (1 / Time.deltaTime);
        if (actualDamping > 1.0f)
            actualDamping = 1.0f;
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, behindPos, ref cameraVelocity, 0.5f);
        Camera.main.transform.LookAt(transform.position + transform.forward * lookForwards + new Vector3(0.0f, -posUp, 0.0f));
	}
}
