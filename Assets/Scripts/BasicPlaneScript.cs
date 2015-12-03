using UnityEngine;
using System.Collections;

public abstract class BasicPlaneScript : MonoBehaviour {
    public float pitch = 0.0f;
    public float heading = 0.0f;
    public float speed = 50.0f;
    public float timeForCircle = 10.0f;
    public int health = 30;
    public int bulletsLeft = 200;
    protected ParticleSystem smoke;
    // Use this for initialization
    public Transform bulletInst;
    protected float frontOfPlane = 4.0f;
	public GameObject explodeInst;

    protected virtual void Start () {
        GameObject smokeObject = transform.Find("Smoke").gameObject;
        smoke = smokeObject.GetComponent<ParticleSystem>();
	}


    protected abstract void UpdateDirection();

    void DoMovement() {
        heading -= pitch * 180 * Time.deltaTime * 8.0f / Mathf.PI / timeForCircle;
        if (heading < -180.0f)
            heading += 360.0f;
        if (heading > 180.0f)
            heading -= 360.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, heading, pitch * 180.0f));
        transform.position += transform.forward * Time.deltaTime * speed;
        float newY = Mathf.Lerp(transform.position.y, 
                                Terrain.activeTerrain.SampleHeight(transform.position) + 30.0f, 
                                Time.deltaTime * 5.0f);
        if (newY < 30.0f)
            newY = 30.0f;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

    }

    protected virtual bool IsFiring() {
        return false;
    }

    protected virtual void DoFiring()
    {
        if (bulletsLeft > 0)
            bulletsLeft--;
        Transform bullet;
        bullet = (Transform)Instantiate(bulletInst, transform.position + transform.forward * frontOfPlane * 1.5f, 
                                        transform.rotation);
        bullet.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        UpdateDirection();
        DoMovement();
        if (IsFiring())
        {
            DoFiring();
        }
    }

    public virtual void IsHit()
    {
        health--;
        // TODO: add smoke.
        if (smoke != null)
        {
            if (! smoke.isPlaying)
                smoke.Play();
            smoke.startSize = (35.0f - health) * 0.2f;
        }

    }
}
