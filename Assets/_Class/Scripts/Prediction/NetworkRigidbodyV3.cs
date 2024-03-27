using UnityEngine;
using FishNet.Object;

public class NetworkRigidbodyV3 : NetworkBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _force;
    
    private Rigidbody2D _rigidbody2D;

    private Vector2 _startPosition;

    [SerializeField] private PredictionManager _predictionManager;
    
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

        float stepInterval = Time.fixedDeltaTime;
        int steps = (int)(passedTime / stepInterval); // How many physics frame to calculate

        (Vector2 finalPosition, Vector2 finalVelocity) =
            _predictionManager.Predict(gameObject, _force * _direction, steps);
        _rigidbody2D.position = finalPosition;
        _rigidbody2D.velocity = finalVelocity;
    }

    [ObserversRpc (RunLocally = true)]
    private void RestartRPC()
    {
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
    }
}

