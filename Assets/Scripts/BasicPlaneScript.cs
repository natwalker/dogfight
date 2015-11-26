using UnityEngine;
using System.Collections;

public abstract class BasicPlaneScript : MonoBehaviour {
    public float pitch = 0.0f;
    public float heading = 0.0f;
    public float speed = 50.0f;
    public float timeForCircle = 10.0f;
    public int health = 100;
    protected int bulletsLeft = 100;
    ParticleSystem smoke;
    // Use this for initialization
    public Transform bulletInst;
    protected void Start () {
        Debug.Log("In base Start");
        GameObject smokeObject = transform.Find("Smoke").gameObject;
        if (smokeObject != null)
        {
            Debug.Log("Stop smoke");
            smoke = smokeObject.GetComponent<ParticleSystem>();
            smoke.Stop();
        }
        else
        {
            Debug.Log("No smoke object found");
        }

	}


    protected abstract void UpdateDirection();

    void DoMovement() {
        heading -= pitch * 180 * Time.deltaTime * 8.0f / Mathf.PI / timeForCircle;
        if (heading < -180.0f)
            heading += 360.0f;
        if (heading > 180.0f)
            heading -= 360.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, heading, pitch * 180.0f));
        var newDirection = Quaternion.Euler(Mathf.Cos(heading * Mathf.PI / 180.0f), 0.0f, Mathf.Sin(heading * Mathf.PI / 180.0f));
        transform.position += transform.forward * Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position) + 30.0f, transform.position.z);

    }

    protected virtual bool IsFiring() {
        return false;
    }

    protected virtual void DoFiring()
    {
        if (bulletsLeft > 0)
            bulletsLeft--;
        Transform bullet;
        bullet = (Transform)Instantiate(bulletInst, transform.position + transform.forward * 10.0f, transform.rotation);
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
        Debug.Log("IsHit");
        health--;
        // TODO: add smoke.
        if (smoke != null)
        {
            if (smoke.isStopped)
                smoke.Play();
            smoke.startSize = (110.0f - health) * 0.1f;
        }
    }
}
