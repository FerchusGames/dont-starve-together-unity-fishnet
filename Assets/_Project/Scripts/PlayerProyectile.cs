using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Example.Scened;
using FishNet.Object;
using UnityEngine;

public class PlayerProyectile : NetworkBehaviour
{
    [SerializeField] private float _lifetime = 5;

    [SerializeField] private int _damageAmount = 1;
    
    // [Client] // Only executed if it is a client
    [Server] // Only executed if it is a server
    private void Start()
    {
        Invoke(nameof(DestroyNetwork), _lifetime);
    }

    private void DestroyNetwork()
    {
        // Despawn only works with 'NetworkObject'
        
        base.Despawn(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsServer)
        {
            other.GetComponent<CharacterController>().TakeDamage(_damageAmount);
            base.Despawn(gameObject);
        }
    }
}
