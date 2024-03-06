using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.Serialization;

public class CharacterController : NetworkBehaviour
{
    [SerializeField]
    private float speed;

    private Renderer _renderer;

    [SyncVar(OnChange = nameof(OnCurrentColorChange))]
    private Color currentColor; // To sync, you need to do it on the server
    
    [Header("Proyectile")]
    [SerializeField] private GameObject _proyectilePrefab;

    [SerializeField] private Transform _proyectileSpawnPoint;
    
    [SerializeField] private float _proyectileVelocity;

    [Header("Life")] [SerializeField] private int _maxLife;
    
    [SyncVar(OnChange = nameof(UpdateLife))]
    [SerializeField] private int _life;

    [SerializeField] private Transform _lifeBarPivot;
    
    // Start is called before the first frame update
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _life = _maxLife;
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (base.Owner.IsLocalClient)
        {
            _renderer.material.color = Color.green;
            name += " - (Local Player)";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!base.IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            Color newColor = Random.ColorHSV();
            ServerChangeColorRPC(newColor);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootServerRpc();
        }
        
        Vector3 inputDirection = Vector3.zero;
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.z = Input.GetAxis("Vertical");

        transform.Translate(inputDirection * speed * Time.deltaTime);
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject newProyectile = Instantiate(_proyectilePrefab, _proyectileSpawnPoint.position, _proyectileSpawnPoint.rotation);
        newProyectile.GetComponent<Rigidbody>().velocity = _proyectileSpawnPoint.forward * _proyectileVelocity;
            
        // Update any SyncVar variable before calling 'Spawn'
            
        // Syncs this object's spawning
        InstanceFinder.ServerManager.Spawn(newProyectile); // Only works if the game object has a NetworkObject
    }

    // [ServerRpc(RequireOwnership = false)] // To execute a function even if the client doesn't not own it
    
    [ServerRpc] // The function is executed on the server side
    void ServerChangeColorRPC(Color newColor)
    {
        currentColor = newColor;
        //ChangeColorRPC(color);
    }
    
    void OnCurrentColorChange(Color oldColor, Color newColor, bool asServer)
    {
        _renderer.material.color = newColor;
    }
    
    //RunLocally: Executes the code on the server also.

    [ObserversRpc(RunLocally = true)] // The function is executed in all clients
    void ChangeColorRPC(Color color)
    {
        _renderer.material.color = color;
    }
    
    // C#
    
    // Two types of variables
    
    // Reference variables
    // Every class
    
    // Data/value variables
    // bool, int, float, double, string 
    // Vector2, Vector3, Quaternion (Struct)
    // The variable has all the value
    
    // NOTE: NetworkObject can be sent 

    private void UpdateLife(int oldLife, int newLife, bool asServer)
    {
        Debug.Log($"{name} suffered damage. New life: {_life}");
        _lifeBarPivot.localScale = new Vector3(GetLifePercentage(), 1f, 1f);
    }

    private float GetLifePercentage()
    {
        return _life / (float)_maxLife;
    }

    [Server]
    public void TakeDamage(int damageAmount)
    {
        _life -= damageAmount;
    }
}
