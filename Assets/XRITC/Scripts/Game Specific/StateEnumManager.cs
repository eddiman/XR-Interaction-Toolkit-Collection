using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class StateEnumManager : MonoBehaviour
{
    public enum GameState
    {
        IdleScene,
        SceneStart,
        GazeStart,
    };

    [Serializable] public class StateEvent : UnityEvent <GameState> {}

    public GameState currentState;

    public bool showDebugForStateChange = false;

    public StateEvent StateChanged;

    public GameObject parentStateObjects;
    public Transform[] listOfStateObjects;



    private void Start()
    {
        //Parent is included in GetComponentsInChildren, so this rewrties the first element, the parent, with the first child, and then shortens the array
        listOfStateObjects = parentStateObjects.GetComponentsInChildren<Transform>();
        //listOfStateObjects[0] = listOfStateObjects[1];
        //Array.Resize(ref listOfStateObjects, listOfStateObjects.Length - 1);
        foreach (Transform child in parentStateObjects.transform)
        {
            //child.gameObject.SetActive(false);
        }

        SetState("IdleScene");
    }

    public void SetState(string stateString)
    {
        //Converts the string rewceived in to the num. This is case-sensitive.
        if (!Enum.TryParse(stateString, out GameState newState))
        {
            throw new Exception("State doesnt exist, see StateControlle object for list of valid states");
        }
        Debug.Log(newState);
        currentState = newState;
        if (showDebugForStateChange)
        {
            debugLogState(stateString);
        }
        StateChanged.Invoke(newState);
    }

    public void ExecuteStateEvent(GameState state)
    {
        string currentStateTranform;

        foreach (var stateTransform in listOfStateObjects)
        {
            if (stateTransform.name != state.ToString()) continue;
            stateTransform.gameObject.SetActive(true);
            stateTransform.GetComponent<OnEnableEvent>().ExecuteStateObjectEvent();
            return;
        }
        /*
        switch (currentState)
        {
            case GameState.SceneStart:
                break;
            case GameState.IdTokenRetrieved:
                break;

            default:
                throw new Exception("This state: " + currentState + " does not exist in state list. Please review the state names in StateEnumManager.cs.");
                break;
            // throw new Exception("This state: " + stateString + " does not exist in state list. Please review the state names in StateEnumManager.cs.");
        }
*/
    }
    private void debugLogState(string stateString)
    {
        Debug.Log("State is now " + stateString + ", and the event of this state has been fired.");
    }

    public GameState GetState()
    {
        return currentState;
    }

}
