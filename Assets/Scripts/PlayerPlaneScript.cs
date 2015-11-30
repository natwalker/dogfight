using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerPlaneScript : BasicPlaneScript
{
    public float headingMultiplier = 4.0f;
    public float inputMultiplier = 0.02f;
    private float timeSinceFiring = 100.0f;
	private bool isMobile = false;
	private Vector3 zeroAc;
	private Vector3 curAc;
	private float sensH = 1;
	private float smooth = 0.75f;
	private float GetAxisH = 0;

    private List<EnemyPlaneScript> _enemyPlanes = new List<EnemyPlaneScript>();
    public Slider healthText;
    public Slider bulletsScroller;
  
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        healthText.value = health;
        frontOfPlane = 9.01f;
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            isMobile = true;
            ResetAxes();
        }
    }

    //accelerometer
    void ResetAxes()
    {
        zeroAc = Input.acceleration;
        curAc = Vector3.zero;
    }

    // Update is called once per frame
    override protected void UpdateDirection()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
        if (isMobile)
        {
            curAc = Vector3.Lerp(curAc, Input.acceleration - zeroAc, Time.deltaTime / smooth);
            GetAxisH = Mathf.Clamp(-curAc.x * sensH, -0.5f, 0.5f);
            pitch = GetAxisH;
            Debug.Log("Mobile");
        }
        else
        {
            // Desktop
            pitch -= Input.GetAxis("Horizontal") * inputMultiplier;
            if (pitch < -0.5f)
                pitch = -0.5f;
            if (pitch > 0.5f)
                pitch = 0.5f;
        }
    }

    protected override void DoFiring()
    {
        base.DoFiring();
        bulletsScroller.value = bulletsLeft;
    }


    protected override bool IsFiring()
    {
        bool isFiring;
        timeSinceFiring += Time.deltaTime;
        const float firingRate = 0.2f;
        if (Input.GetMouseButton(0) == true && timeSinceFiring >= firingRate && bulletsLeft > 0)
        {
            timeSinceFiring = 0.0f;
            isFiring = true;
        }
        else
        {
            isFiring = false;
        }
        return isFiring;
    }

    public void addEnemy(EnemyPlaneScript anEnemy)
    {
        if (_enemyPlanes.Contains(anEnemy))
        {
            Debug.Log("Attempting to re-add enemy");
        }
        else
        {
            _enemyPlanes.Add(anEnemy);
            Debug.Log("Added enemy plane");
        }
    }

    public void removeEnemy(EnemyPlaneScript anEnemy)
    {
        if (_enemyPlanes.Contains(anEnemy))
        {
            _enemyPlanes.Remove(anEnemy);
            Debug.Log("Removed enemy plane");
        }
        else
        {
            Debug.Log("Attempting to remove non-existing enemy");
        }
    }

    public void OnDestroy()
    {
        Debug.Log("Destroying plane with enemies: " + _enemyPlanes.Count.ToString());
        foreach (EnemyPlaneScript plane in _enemyPlanes)
        {
            plane.UnregisterPlayer(this);
        }
    }

    public override void IsHit()
    {
        base.IsHit();
        healthText.value = health;
    }

}
