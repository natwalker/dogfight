using UnityEngine;
using System;
using System.Collections;

public class EnemyPlaneScript : MonoBehaviour
{
    public float speed = 50.0f;
    public float heading = 0.0f;
    public float pitch = 0.0f;
    public float headingMultiplier = 4.0f;
    private GameObject _player = null;
    private PlayerPlaneScript _playerPlane = null;
    private GameObject _arrow = null;
    private float _arrowDistance = 30.0f;
    // Use this for initialization
    void Start()
    {
        _player = GameObject.Find("PlayerPlane");
        if (_player != null)
        {
            _playerPlane = _player.GetComponent < PlayerPlaneScript >();
            _playerPlane.addEnemy(this);
            _arrow = transform.FindChild("Arrow").gameObject;
            _arrow.transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        heading -= pitch * headingMultiplier * Time.deltaTime;
        if (heading < -180.0)
            heading += 360.0f;
        if (heading > 180.0)
            heading -= 360.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, heading, pitch));
        var newDirection = Quaternion.Euler(Mathf.Cos(heading * Mathf.PI/ 180.0f), 0.0f, Mathf.Sin(heading * Mathf.PI/ 180.0f));
        transform.position += transform.forward * Time.deltaTime * speed;

    }

    void LateUpdate()
    {
        if (_arrow != null)
        {
            float direction = Mathf.Atan2(transform.position.z - _player.transform.position.z,
                                         transform.position.x - _player.transform.position.x);
            Vector3 planeRotation = _player.transform.eulerAngles;

            _arrow.transform.position = _player.transform.position + new Vector3(Mathf.Cos(direction) * _arrowDistance, 0, Mathf.Sin(direction) * _arrowDistance);
            _arrow.transform.rotation = Quaternion.Euler(0.0f, direction * 180.0f / Mathf.PI, 0.0f);
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
}
    
