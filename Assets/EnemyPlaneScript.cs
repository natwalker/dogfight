using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyPlaneScript : BasicPlaneScript
{
    private GameObject _player = null;
    private PlayerPlaneScript _playerPlane = null;
    private GameObject _arrow = null;
    private float _arrowDistance = 30.0f;
    private float timeSinceFiring = 100.0f;

    private List<Vector2> _wayPoints;
    private int _nextWayPoint = 0;
    public GameObject guiCanvas;
    private Slider _healthBar;
    public enum EnemyPlaneState { STATE_PATROL, STATE_EVADE, STATE_ATTACK };
    public EnemyPlaneState _state;
    private float timeSinceSeeingPlayer;

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
        _state = EnemyPlaneState.STATE_PATROL;
        _healthBar = transform.Find("Health").GetComponent<Slider>();
        _healthBar.transform.SetParent(guiCanvas.transform);
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

    private void UpdateDirection_Patrol()
    {
        // Todo: work out direction based on AI.
        float currentAngle = Mathf.Deg2Rad * heading;
        float requiredAngle = Mathf.Atan2(_wayPoints[_nextWayPoint].x - transform.position.x, _wayPoints[_nextWayPoint].y - transform.position.z);
        float diffAngle = requiredAngle - currentAngle;
        if (diffAngle > Mathf.PI)
            diffAngle -= Mathf.PI * 2.0f;
        if (diffAngle < -Mathf.PI)
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

        if (theDist < minDistance)
        {
            //Debug.Log("Updating waypoint");
            _nextWayPoint++;
            if (_nextWayPoint == _wayPoints.Count)
                _nextWayPoint = 0;
        }

    }

    private void UpdateDirection_Evade()
    {

    }

    private void UpdateDirection_Attack()
    {
        SetPitchByReqPosition(new Vector2(_player.transform.position.x, _player.transform.position.z));
    }

    private void SetPitchByReqPosition(Vector2 reqPos)
    {
        float currentAngle = Mathf.Deg2Rad * heading;
        float reqHeading = Mathf.Atan2(reqPos.x - transform.position.x, reqPos.y - transform.position.z);
        float diffAngle = reqHeading - currentAngle;
        if (diffAngle > Mathf.PI)
            diffAngle -= Mathf.PI * 2.0f;
        if (diffAngle < -Mathf.PI)
            diffAngle += Mathf.PI * 2.0f;
        //Debug.Log("Curr " + currentAngle.ToString() + " req " + requiredAngle.ToString());
        float reqPitch = Mathf.Clamp(2.0f * -diffAngle / Mathf.PI, -1.0f, 1.0f);
        //Debug.Log("Pitch " + pitch.ToString() + " Req " + reqPitch.ToString());

        if (reqPitch < (pitch - 5.0f * Time.deltaTime))
            pitch -= Time.deltaTime * 5.0f;
        if (reqPitch > (pitch + 5.0f * Time.deltaTime))
            pitch += Time.deltaTime * 5.0f;
    }

    private bool _CanSeePlayer()
    {
        bool result = false;

        float angle = Mathf.Atan2(_player.transform.position.x - transform.position.x, _player.transform.position.z - transform.position.z);
        float diffAngle = angle - heading * Mathf.Deg2Rad;
        if (diffAngle < -Mathf.PI)
            diffAngle += Mathf.PI * 2.0f;
        if (diffAngle > Mathf.PI)
            diffAngle -= Mathf.PI * 2.0f;
        float distance = Vector3.Magnitude(_player.transform.position - transform.position);
        float maxDistance = 300.0f;
        if (distance < maxDistance && Mathf.Abs(diffAngle) < 35.0f * Mathf.Deg2Rad)
        {
            result = true;
        }
        return result;
    }

    private void _CheckNextWayPoint()
    {
        //TODO: work out which waypoint is nearest to current position and direction.
    }

    private void _CheckChangeInState()
    {
        if (_CanSeePlayer())
        {
            timeSinceSeeingPlayer = 0.0f;
            if (_state != EnemyPlaneState.STATE_ATTACK)
            {
                Debug.Log("Enemy is now ATTACKING");
                _state = EnemyPlaneState.STATE_ATTACK;
            }
        }
        else
        {
            timeSinceSeeingPlayer += Time.deltaTime;
            if (timeSinceSeeingPlayer > 0.25f && _state != EnemyPlaneState.STATE_PATROL)
            {
                Debug.Log("Enemy is now PATROLLING");
                _CheckNextWayPoint();
                _state = EnemyPlaneState.STATE_PATROL;
            }
        }

    }

    // Update is called once per frame
    override protected void UpdateDirection()
    {
        _CheckChangeInState();
        switch (_state)
        {
            case EnemyPlaneState.STATE_PATROL:
                UpdateDirection_Patrol();
                break;
            case EnemyPlaneState.STATE_EVADE:
                UpdateDirection_Evade();
                break;
            case EnemyPlaneState.STATE_ATTACK:
                UpdateDirection_Attack();
                break;
        }
    }

    protected override bool IsFiring()
    {
        bool result = false;
        timeSinceFiring += Time.deltaTime;
        if (timeSinceFiring > 0.2f)
        {
            if (_CanSeePlayer())
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

            _arrow.transform.position = _player.transform.position + new Vector3(Mathf.Cos(direction) * _arrowDistance, 0, Mathf.Sin(direction) * _arrowDistance);
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
        Vector3 healthPos = new Vector3(transform.position.x, transform.position.y + 10.0f, transform.position.z);
        _healthBar.transform.position = Camera.main.WorldToScreenPoint(healthPos);
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

    override public void IsHit()
    {
        base.IsHit();
        _healthBar.value = health;
    }
}

