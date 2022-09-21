using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private Vector3 _initialPos;

    private void Awake()
    {
        _initialPos = transform.position;
    }

    private void Update()
    {
        Move();
    }
    public void ResetPosition()
    {
        transform.position = _initialPos;
    }

    private void Move()
    {
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * _moveSpeed);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * _moveSpeed);
    }

}
