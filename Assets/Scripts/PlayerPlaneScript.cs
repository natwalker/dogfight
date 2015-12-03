using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class PlayerPlaneScript : BasicPlaneScript
{
    public float headingMultiplier = 4.0f;
    public float inputMultiplier = 0.02f;
    private float timeSinceFiring = 100.0f;
	private bool isMobile = false;
	private Vector3 zeroAc;
	private Vector3 curAc;
	private float sensH = 1;
	private float GetAxisH = 0;
	private int _score = 0;
	private int _numPlanes = 0;
	public Text scoreText;
    private List<EnemyPlaneScript> _enemyPlanes = new List<EnemyPlaneScript>();
    public Slider _healthBar;
    public Slider bulletsScroller;
	public Image _healthFill;
	public Text gameOver;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        _healthBar.value = health;
		_healthFill = _healthBar.GetComponentsInChildren<UnityEngine.UI.Image>()
			.FirstOrDefault(t => t.name == "Fill");
		frontOfPlane = 9.01f;
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            isMobile = true;
            ResetAxes();
        }
		_numPlanes = _enemyPlanes.Count;
		scoreText.text = _score.ToString () + " / " + _numPlanes.ToString ();
    }

    //accelerometer
    void ResetAxes()
    {
        zeroAc = Vector3.zero;
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
            curAc = Vector3.Lerp(curAc, Input.acceleration - zeroAc, Time.deltaTime / 0.25f);
            GetAxisH = Mathf.Clamp(-curAc.x * sensH, -0.5f, 0.5f);
            pitch = GetAxisH;
            Debug.Log("Mobile");
        }
        else
        {
            // Desktop
            pitch -= Input.GetAxis("Horizontal") * Time.deltaTime;
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

    public void removeEnemy(EnemyPlaneScript anEnemy, bool hasDied)
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
		if (hasDied) {
			_score++;
			scoreText.text = _score.ToString () + " / " + _numPlanes.ToString ();
			if (_score == _numPlanes)
			{
				gameOver.text = "Game Over\nYou Win!";
				gameOver.gameObject.SetActive(true);
			}
		}
    }

    public void OnDestroy()
    {
        Debug.Log("Destroying plane with enemies: " + _enemyPlanes.Count.ToString());
		if (_enemyPlanes.Count () > 0) {
			GetComponent<CameraFollowScript>().enabled = false;
			_enemyPlanes [0].transform.GetComponent<CameraFollowScript> ().enabled = true;
			gameOver.text = "Game Over\nYou Lose!";
			gameOver.gameObject.SetActive(true);

		}
        foreach (EnemyPlaneScript plane in _enemyPlanes)
        {
            plane.UnregisterPlayer(this);
        }
    }

    public override void IsHit()
    {
        base.IsHit();
        _healthBar.value = health;
		if (_healthBar.value < 2.0f * _healthBar.maxValue / 3.0f) {
			if (_healthBar.value < _healthBar.maxValue / 3.0f) {
				_healthFill.color = new Color(1.0f, 0.0f, 0.0f);
			}
			else {
				_healthFill.color = new Color(1.0f, 1.0f, 0.0f);
			}
		}
		if (health == 0) {
			Destroy (gameObject);
			GameObject theExplosion = Instantiate(explodeInst, transform.position, transform.rotation) as GameObject;
			smoke.transform.SetParent(theExplosion.transform);
			smoke.loop = false;
			theExplosion.SetActive(true);
		}
	}

}
