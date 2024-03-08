using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class ObjectsSync : NetworkBehaviour
{
     // List, HashSet, Dictionary
    // List: Dynamic Array
    // HashSet: Lists that can't repeat values
    // Dictionary<string, int>: Like a yellow book

    [SyncObject]
    readonly SyncList<int> myArray = new SyncList<int>();
    // readonly SyncHashSet<int>;
    // readonly SyncDictionary<string, int>;

    private void Start()
    {
        myArray.OnChange += OnMyArrayChange;
    }

    private void Update()
    {
        if(base.IsServer == false)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            myArray.Add(Random.Range(1, 100));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            int index = Random.Range(0, myArray.Count);
            myArray[index] = Random.Range(1, 100);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            int index = Random.Range(0, myArray.Count);
            myArray.RemoveAt(index);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            myArray.Clear();
        }
    }

    void OnMyArrayChange(SyncListOperation operation, int index, int previous, int next, bool asServer)
    {
        Debug.Log($"Updated list: {operation} - index:{index} - previous:{previous} - next:{next}");
        
        // When we get here, "myArray" is already updated.
        switch (operation)
        {
            case SyncListOperation.Add: // myArray.Add(5); // next has the added value
                break;
            case SyncListOperation.Insert: // myArray.Insert(3, 5); // next has the added value
                break;
            case SyncListOperation.Set: // myArray[3] = 5; // previous has the previous value and next the next value
                break;
            case SyncListOperation.RemoveAt: // myArray.RemoveAt(3); // previous hast the value that was in that position
                break;
            case SyncListOperation.Clear: // myArray.Clear(); // index, previous, next have no value
                break;
            case SyncListOperation.Complete: // When it has finished processing all the events. // index, previous, next have no value
                break;
        }
    }
}