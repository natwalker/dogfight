using UnityEngine;
using System;
using System.Collections;

public class EnemyPlaneScript : BasicPlaneScript
{
    private GameObject _player = null;
    private PlayerPlaneScript _playerPlane = null;
    private GameObject _arrow = null;
    private float _arrowDistance = 30.0f;
    // Use this for initialization
    void Start()
    {
        base.Start();
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

