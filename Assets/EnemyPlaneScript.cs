using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyPlaneScript : BasicPlaneScript
{
    private GameObject _player = null;
    private PlayerPlaneScript _playerPlane = null;
    private GameObject _arrow = null;
    private float _arrowDistance = 30.0f;
    private float timeSinceFiring = 100.0f;

    private List<Vector2> _wayPoints;
    private int _nextWayPoint = 0;

    void addWayPoint(float x, float y)
    {
        float gridSize = 50.0f;
        Vector3 pos = transform.position;
        _wayPoints.Add(new Vector2(pos.z + x * gridSize, pos.x + y * gridSize));
    }

    // Use this for initialization
    protected new void Start()
    {
        base.Start();
        //TODO: Find front of colliders
        frontOfPlane = 9.76f;
        //TODO: Read the waypoints and start position from a file or the scene.
        _wayPoints = new List<Vector2>();
        addWayPoint(0.0f, 4.0f);
        addWayPoint(-1.0f, 5.0f);
        addWayPoint(-4.0f, 5.0f);
        addWayPoint(-5.0f, 4.0f);
        addWayPoint(-5.0f, 1.0f);
        addWayPoint(-4.0f, 0.0f);
        addWayPoint(2.0f, 0.0f);
        addWayPoint(3.0f, -1.0f);
        addWayPoint(3.0f, -2.0f);
        addWayPoint(2.0f, -3.0f);
        addWayPoint(1.0f, -3.0f);
        addWayPoint(0.0f, -2.0f);

        _player = GameObject.Find("PlayerPlane");
        if (_player != null)
        {
            _playerPlane = _player.GetComponent<PlayerPlaneScript>();
            _playerPlane.addEnemy(this);
            _arrow = transform.FindChild("Arrow").gameObject;
            _arrow.transform.SetParent(null);
        }
    }



    // Update is called once per frame
    override protected void UpdateDirection()
    {
        // Todo: work out direction based on AI.
        float currentAngle = Mathf.Deg2Rad * heading;
        float requiredAngle = Mathf.Atan2(_wayPoints[_nextWayPoint].x - transform.position.x, _wayPoints[_nextWayPoint].y - transform.position.z);
        float diffAngle = requiredAngle - currentAngle;
        if (diffAngle > Mathf.PI)
            diffAngle -= Mathf.PI * 2.0f;
        if (diffAngle < - Mathf.PI)
            diffAngle += Mathf.PI * 2.0f;
        //Debug.Log("Curr " + currentAngle.ToString() + " req " + requiredAngle.ToString());
        float reqPitch = Mathf.Clamp(2.0f * -diffAngle / Mathf.PI, -1.0f, 1.0f);
        //Debug.Log("Pitch " + pitch.ToString() + " Req " + reqPitch.ToString());
        
        if (reqPitch < (pitch - 5.0f * Time.deltaTime))
            pitch -= Time.deltaTime;
        if (reqPitch > (pitch + 5.0f * Time.deltaTime))
            pitch += Time.deltaTime;
        
        float minDistance = 15.0f;
        float theDist = Mathf.Sqrt(((_wayPoints[_nextWayPoint].y - transform.position.z) * (_wayPoints[_nextWayPoint].y - transform.position.z)) +
                                   ((_wayPoints[_nextWayPoint].x - transform.position.x) * (_wayPoints[_nextWayPoint].x - transform.position.x)));
        //Debug.Log("Distance " + theDist.ToString());
            
        if ( theDist < minDistance)
        {
            //Debug.Log("Updating waypoint");
            _nextWayPoint++;
            if (_nextWayPoint == _wayPoints.Count)
                _nextWayPoint = 0;
        }
    }

    protected override bool IsFiring()
    {
        bool result = false;
        timeSinceFiring += Time.deltaTime;
        if (timeSinceFiring > 0.2)
        {
            float angle = Mathf.Atan2(_player.transform.position.x - transform.position.x, _player.transform.position.z - transform.position.z);
            float diffAngle = angle - heading * Mathf.Deg2Rad;
            if (diffAngle < -Mathf.PI)
                diffAngle += Mathf.PI * 2.0f;
            if (diffAngle > Mathf.PI)
                diffAngle -= Mathf.PI * 2.0f;
            float distance = Vector3.Magnitude(_player.transform.position - transform.position);
            float maxDistance = 300.0f;
            if (distance < maxDistance && Mathf.Abs(diffAngle) < 25.0f * Mathf.Deg2Rad)
            {
                result = true;
                timeSinceFiring = 0.0f;
            }
        }
        return result;
    }

    void LateUpdate()
    {
        if (_arrow != null)
        {
            float direction = Mathf.Atan2(transform.position.z - _player.transform.position.z,
                                         transform.position.x - _player.transform.position.x);
            Vector3 planeRotation = _player.transform.eulerAngles;

            _arrow.transform.position = _player.transform.position + new Vector3(Mathf.Cos(direction) * _arrowDistance, 0, Mathf.Sin(direction) * _arrowDistance);
            Vector3 currDirection = _arrow.transform.eulerAngles;
            _arrow.transform.LookAt(transform.position);

            float distance = Mathf.Sqrt((transform.position.z - _player.transform.position.z) * (transform.position.z - _player.transform.position.z) +
                (transform.position.x - _player.transform.position.x) * (transform.position.x - _player.transform.position.x));
            if (distance < _arrowDistance * 1.5f)
            {
                _arrow.SetActive(false);
            }
            else
            {
                _arrow.SetActive(true);
            }
        }
    }

    public void UnregisterPlayer(PlayerPlaneScript thePlayer)
    {
        _playerPlane = null;
    }

    public void OnDestroy()
    {
        if (_playerPlane != null)
        {
            _playerPlane.removeEnemy(this);
            Destroy(_arrow);
        }
    }

    void OnTriggerEnter(Collider col)
    {

        Debug.Log("OMG: I've been hit");
    }
}

