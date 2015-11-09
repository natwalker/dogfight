using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PlayerPlaneScript : MonoBehaviour {
    public float speed = 50.0f;
    public float heading = 0.0f;
    public float pitch = 0.0f;
    public float headingMultiplier = 4.0f;
    public float inputMultiplier = 0.02f;
    private bool isFiring = false;
    private float timeSinceFiring = 100.0f;
    private int bulletsLeft = 100;
	private bool isMobile = false;
	private Vector3 zeroAc;
	private Vector3 curAc;
	private float sensH = 1;
	private float sensV = 10;
	private float smooth = 0.75f;
	private float GetAxisH = 0;
	private float GetAxisV = 0;

    public Text bulletsText;
    public Transform bulletInst;
	// Use this for initialization
	void Start () {
        Debug.Log("Script has started:" + gameObject.name);
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			isMobile = true;
			ResetAxes();
		}
	}

	//accelerometer
	void ResetAxes(){
		zeroAc = Input.acceleration;
		curAc = Vector3.zero;
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Debug.Log("Escape was pressed");
            Application.Quit();
        }
		if (isMobile) {
			curAc = Vector3.Lerp(curAc, Input.acceleration-zeroAc, Time.deltaTime/smooth);
			GetAxisV = Mathf.Clamp(curAc.y * sensV, -1, 1);
			GetAxisH = Mathf.Clamp(-curAc.x * sensH, -0.5f, 0.5f);
			pitch =  GetAxisH;
		} else {
			// Desktop
			pitch -= Input.GetAxis ("Horizontal") * inputMultiplier;
			if (pitch < -0.5f)
				pitch = -0.5f;
			if (pitch > 0.5f)
				pitch = 0.5f;
		}
        heading -= pitch * headingMultiplier;
        if (heading < -180.0)
            heading += 360.0f;
        if (heading > 180.0)
            heading -= 360.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, heading, pitch * 180));
        var newDirection = Quaternion.Euler((float)Math.Sin((double)heading * Math.PI), 0.0f, (float)Math.Cos(heading * Math.PI));
        transform.position += transform.forward * Time.deltaTime * speed;
        checkIfFiring();

        if (isFiring && bulletsLeft > 0)
        {
            Transform bullet; 
            bullet = (Transform)Instantiate(bulletInst, transform.position + transform.forward * 10.0f, transform.rotation);
            bullet.gameObject.SetActive(true);
            bulletsLeft--;
            bulletsText.text = bulletsLeft.ToString();
            Debug.Log("Fired !!!");
            Debug.Log(bullet.position);
        }

	}

    void checkIfFiring()
    {
        timeSinceFiring += Time.deltaTime;
        const float firingRate = 0.2f;
        if (Input.GetMouseButton(0) == true && timeSinceFiring >= firingRate)
        {
            timeSinceFiring = 0.0f;
            Debug.Log("Firing");
            isFiring = true;
        }
        else
        {
            Debug.Log("Not Firing");
            isFiring = false;
        }
    }
}
