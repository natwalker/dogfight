using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudFunctions : MonoBehaviour {
    int score = 0;
    int ammo = 100;
    int health = 100;
    public Text scoreText;
    public Text healthText;
    public Text ammoText;
    void setScore(int newScore)
    {
        score = newScore;
        scoreText.text = score.ToString("00000");
    }
    void setHealth(int newHealth)
    {
        health = newHealth;
        healthText.text = health.ToString();
    }

    void setAmmo(int newAmmo)
    {
        ammo = newAmmo;
        ammoText.text = ammo.ToString();
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
