using UnityEngine;
using System.Collections;

public class BulletMoveScript : MonoBehaviour {

    public float lifetime = 3.0f;
    public float speed = 100.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f)
        {
            Destroy(gameObject);
        }
	}
}
