using UnityEngine;
using FishNet.Object;

public class NetworkRigidbodyV2 : NetworkBehaviour
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
            ShootRPC(base.TimeManager.Tick);
            _rigidbody2D.velocity = _force * _direction;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartRPC();
        }
    }

    [ObserversRpc]
    private void ShootRPC(uint serverTick)
    {
        float passedTime = (float)base.TimeManager.TimePassed(serverTick);
        // This works if there is no gravity
        //Vector2 positionDifference = _direction * _force * passedTime;
        //_rigidbody2D.position += positionDifference;

        float stepInterval = 0.02f;
        int steps = (int)(passedTime / stepInterval); // How many physics ticks we're going to calculate
        float velocity = _force * Time.fixedDeltaTime;

        // Vector2 predictedPosition = _rigidbody2D.position;
        // float appliedGravity = 0f;
        //
        // for (int i = 0; i < steps; i++)
        // {
        //     // Origin + Direction * Velocity * Frame Duration
        //     predictedPosition = _rigidbody2D.position + _direction * velocity * i * stepInterval;
        //     appliedGravity = Physics2D.gravity.y / 2f * Mathf.Pow(i * stepInterval, 2); // Parabolic movement formula
        //     predictedPosition.y += appliedGravity;
        // }
        //
        // _rigidbody2D.position = predictedPosition;
        // Vector2 newVelocity = _direction * _force;
        // newVelocity.y -= appliedGravity;
        // _rigidbody2D.velocity = newVelocity;
    }

    [ObserversRpc (RunLocally = true)]
    private void RestartRPC()
    {
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
    }
}
