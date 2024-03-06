using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;

namespace _Class.Scripts
{
    public class TimeCounter : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counterText;
        [SerializeField] private GameObject _startButton;
        [SerializeField] private float _timerLength = 10f;
    
        private bool _isTimerRunning;
        private uint _counterStartTick;

        [SyncObject]
        private readonly SyncTimer _syncCounter = new SyncTimer();

        private void Start()
        {
            _syncCounter.OnChange += OnSyncCounterChange;
        }

        private void OnSyncCounterChange(SyncTimerOperation operation, float localTime, float syncedTime, bool asServer)
        {
            switch (operation)
            {
                case SyncTimerOperation.Start:
                    // syncedTime => Initial time , here it's _timerLength
                    _isTimerRunning = true;
                    break;
                case SyncTimerOperation.Pause: // Local, time is paused
                    break;
                case SyncTimerOperation.PauseUpdated: // Client: A pause is received and the server says exactly how much time is left 
                    // syncedTime => How much time is left
                    break;
                case SyncTimerOperation.Unpause:
                    break;
                case SyncTimerOperation.Stop: // Server: Stop was called
                    break;
                case SyncTimerOperation.StopUpdated: // Client: Stop was received 
                    // syncedTime => How much time was left when it is stopped
                    break;
                case SyncTimerOperation.Finished: // The time has reached 0
                    Debug.Log("SyncTimer counter has finished!");
                    break;
                case SyncTimerOperation.Complete: // It has finished processing all the events
                    break;
            }
        }
    
        public void StartCounter() // Called from the button
        {
            _syncCounter.StartTimer(_timerLength);
            // Other functions
            // _syncCounter.PauseTimer();
            // _syncCounter.UnpauseTimer();
            // _syncCounter.StopTimer();
            // _syncCounter.StartTimer(); // If a StartTimer is called when there is another one running, then a StopTimer is called and another one is started. 
        }

        private void Update()
        {
            if (_isTimerRunning == false)
            {
                return;
            }

            _syncCounter.Update(Time.deltaTime);
            float counter = _syncCounter.Remaining;

            _counterText.text = counter.ToString("F2"); // F2 => 2 decimals
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (IsServer == false)
            {
                _startButton.SetActive(false); // It will only be visible for the server
            }
        }
    
        // Initial Start
        // public void StartCounter() // Called from the button
        // {
        //     _counterStartTick = base.TimeManager.Tick;
        //     StartCounterRPC(_counterStartTick); // TimeManager.Tick => Server Time
        //     _isTimerRunning = true;
        // }
        // RPC is no longer needed
        // [ObserversRpc]
        // private void StartCounterRPC(uint serverTick)
        // {
        //     _counterStartTick = serverTick;
        //     _isTimerRunning = true;
        // }
    
        // Initial Version
        // private void Update()
        // {
        //     if (_isTimerRunning == false)
        //     {
        //         return;
        //     }
        //
        //     float passedTime = (float) base.TimeManager.TimePassed(_counterStartTick);
        //     float counter = _timerLength - passedTime;
        //     
        //     if (counter <= 0f)
        //     {
        //         counter = 0f;
        //         _isTimerRunning = false;
        //         Debug.Log("float counter has finished!");
        //     }
        //
        //     _counterText.text = counter.ToString("F2"); // F2 => 2 decimals
        // }
    }
}
