using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class AnimationSync : NetworkBehaviour
{
    [SerializeField] private Animator _anim;

    private void Update()
    {
        if(!base.IsServer)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayWinRPC(base.TimeManager.Tick);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // NOTE: NetworkAnimator can't sync triggers, but it can do booleans, floats and integers
            _anim.SetBool("Jump", true);
        }
    }

    [ObserversRpc (RunLocally = true)] // Server will execute this immediately
    private void PlayWinRPC(uint serverTick)
    {
        if (base.IsHost) // I'm server and client at the same time
        {
            PlayWin();
            return;
        }
        
        float passedTime = (float)base.TimeManager.TimePassed(serverTick); // 0.1 seconds 
        const float winDuration = 3.967f;

        float passedPercent = passedTime / winDuration;
        Debug.Log($"passedTime: {passedTime}, passedPercent: {passedPercent}");
        PlayWin(passedPercent);
    }

    private void PlayWin(float passedPercent = 0f)
    {
        _anim.CrossFade("WIN00", 0.1f, 0, passedPercent);
    }
}

