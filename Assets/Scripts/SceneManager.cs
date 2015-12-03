using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public GameObject enemyPlaneInst;

	void Awake() {

		GameObject instance = Instantiate (enemyPlaneInst, new Vector3(400,30,400), Quaternion.identity) as GameObject;
		instance.transform.SetParent (transform.parent);
		instance.SetActive (true);

		instance = Instantiate (enemyPlaneInst, new Vector3(650, 30, 350), Quaternion.identity) as GameObject;
		instance.transform.SetParent (transform.parent);
		instance.SetActive (true);

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
