using Cinemachine;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class CharacterController : NetworkBehaviour
{
    [SerializeField] private float speed;
    
    [Header("Life")] [SerializeField] private int _maxLife;

    [SyncVar(OnChange = nameof(UpdateLife))] [SerializeField]
    private int _life;

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    
    void Awake()
    {
        _life = _maxLife;
        _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (base.Owner.IsLocalClient)
        {
            name += " - (Local Player)";
            _cinemachineVirtualCamera.Follow = this.transform;
        }
    }
    
    void Update()
    {
        if (!base.IsOwner) return;

        Vector3 inputDirection = Vector3.zero;
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.z = Input.GetAxis("Vertical");

        transform.Translate(inputDirection * speed * Time.deltaTime);
    }

    private void UpdateLife(int oldLife, int newLife, bool asServer)
    {
        Debug.Log($"{name} suffered damage. New life: {_life}");
    }

    [Server]
    public void TakeDamage(int damageAmount)
    {
        _life -= damageAmount;
    }
}
