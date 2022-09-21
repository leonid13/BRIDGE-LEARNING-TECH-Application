using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainPlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _ground;

    float _minX, _maxX, _minZ, _maxZ;

    void Awake()
    {
        SetConstraints();
    }

    private void SetConstraints()
    {
        _minX = _ground.localScale.x / -2;
        _maxX = _ground.localScale.x / 2;
        _minZ = _ground.localScale.z / -2;
        _maxZ = _ground.localScale.z / 2;
    }

    private void FixedUpdate()
    {
        ConstrainPlayer();
    }

    private void ConstrainPlayer()
    {
        if(transform.position.x > _maxX)
        {
            transform.position = new Vector3(_maxX, transform.position.y, transform.position.z);
        }
        else if(transform.position.x < _minX)
        {
            transform.position = new Vector3(_minX, transform.position.y, transform.position.z);
        }
        else if (transform.position.z > _maxZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _maxZ);
        }
        else if (transform.position.z < _minZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _minZ);
        }
    }
}
