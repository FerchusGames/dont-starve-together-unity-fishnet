using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerEvents : MonoBehaviour
{
    private Queue<Vector3> _positionHistory = new Queue<Vector3>(10);
    
    private void Start()
    {
        // If the script is MonoBehaviour
        FishNet.InstanceFinder.TimeManager.OnTick += OnTick;

        // If we use NetworkBehaviour
        // base.TimeManager.
    }

    // Fishnet game cycle
    
    private void OnPreTick() { } // Before preparing the packages sending

    private void OnTick() // Right before sending the packages
    {
        _positionHistory.Enqueue(transform.position);

        if (_positionHistory.Count > 10)
        {
            _positionHistory.Dequeue();
        }
    } 
    
    private void OnPostTick() { } // The packages have been sent
    
    // Called when your new ping/lag is calculated
    // Can be used to disable certain functions or show a UI to tell the player they have a bad connection
    private void OnRoundTripTimeUpdated(float tripTime) { }
    
    // Used if you ask FishNet to control the physics update
    private void OnPostPhysicsSimulation(float deltaTime) { }
    private void OnPrePhysicsSimulation(float deltaTime) { }
    
    // Called at the same time as their Unity counterpart
    private void OnUpdate() { }
    private void OnFixedUpdate() { }
    private void OnLateUpdate() { }
}
