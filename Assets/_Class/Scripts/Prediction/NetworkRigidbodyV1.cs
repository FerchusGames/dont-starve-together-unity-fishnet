using System;
using FishNet.Object;
using UnityEngine;

public class NetworkRigidbodyV1 : NetworkBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _force;
    
    private Rigidbody2D _rigidbody2D;

    private Vector2 _startPosition;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (!base.IsServer)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootRPC();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartRPC();
        }
    }

    [ObserversRpc (RunLocally = true)]
    private void ShootRPC()
    {
        _rigidbody2D.velocity = _direction * _force;
    }

    [ObserversRpc (RunLocally = true)]
    private void RestartRPC()
    {
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
    }
}
