using System;
using FishNet.Component.ColliderRollback;
using FishNet.Managing.Timing;
using FishNet.Object;
using UnityEngine;

public class PlayerRollback : NetworkBehaviour  
{
    private void Update()
    {
        if (!base.IsOwner)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            PreciseTick preciseTick = base.TimeManager.GetPreciseTick(base.TimeManager.Tick);
            FireServerRPC(ray.origin, ray.direction, preciseTick);
        }
    }

    [ServerRpc]
    private void FireServerRPC(Vector3 position, Vector3 direction, PreciseTick preciseTick)
    {
        // if (Player is dead)
        //      return;
        
        base.RollbackManager.Rollback(preciseTick, RollbackPhysicsType.Physics); // Moves back the colliders to where they were

        RaycastHit raycastHit;
        if (Physics.Raycast(position, direction, out raycastHit))
        {
            Debug.Log($"You hit {raycastHit.collider}");
            // Inflict damage, tell other players that the impact was successful
        }
        else
        {
            Debug.Log("You didn't hit anything");
        }

        base.RollbackManager.Return(); // Returns the colliders to the current positions
    }
}
