// ************************************************************************ 
// File Name:   StateMachine.cs 
// Purpose:    	A state machine that can be used for AI, effects, etc.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2023 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


namespace Bounder.Framework
{
    // ********************************************************************
    #region Class: StateMachine
    // ********************************************************************

    public class StateMachine : MonoBehaviour
    {
        // ****************************************************************
        #region State Information
        // ****************************************************************
        public delegate void StateFunction();
        public delegate IEnumerator StateCoroutine();
        [System.Serializable]
        public class State
        {
            public int stateIndex = -1;

            public StateFunction Enter = null;
            public StateFunction Update = null;
            public StateFunction Exit = null;

            public StateCoroutine UpdateCR = null;

            public State()
            {
            }

            public State(int _stateIndex, StateFunction _enter, StateFunction _update, StateFunction _exit)
            {
                stateIndex = _stateIndex;
                Enter = _enter;
                Update = _update;
                Exit = _exit;
            }

            public State(int _stateIndex, StateFunction _enter, StateCoroutine _update, StateFunction _exit)
            {
                stateIndex = _stateIndex;
                Enter = _enter;
                UpdateCR = _update;
                Exit = _exit;
            }

        }
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Properties
        // ****************************************************************
        public float timeInState { get; private set; }  = 0.0f;
        public State currentState { get; private set; }  = null;
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Private Variables
        // ****************************************************************

        private Dictionary<int, State> stateMachine = new Dictionary<int, State>();
        private Coroutine activeCoroutine = null;
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region MonoBehaviour Methods
        // ****************************************************************
        public virtual void Update()
        {
            // Call update on the current state
            timeInState += Time.deltaTime;
            if (currentState != null && currentState.Update != null)
                currentState.Update();
        }
        // ********************************************************************
        protected virtual void OnDisable()
        {
            ChangeState(0);
        }
        // ****************************************************************
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Public Methods
        // ****************************************************************
        public int GetCurrentStateIndex()
        {
            return currentState != null ? currentState.stateIndex : -1;
        }
        // ****************************************************************
        public State GetState(int _index)
        {
            // First make sure we have this state registered
            if (!stateMachine.ContainsKey(_index))
            {
                Debug.LogError("State machine attempt to move to unregistered state " + _index);
                return null;
            }
            else return stateMachine[_index];
        }
        // ****************************************************************
        public void ChangeState(int _index)
        {
            // First make sure we have this state registered
            State newState = GetState(_index);
            if (newState == null)
                return; // Error message already given

            // are the two states different?
            if (currentState == null || _index != currentState.stateIndex)
            {
                // exit the old state
                if (currentState != null)
                {
                    //Debug.Log("Now exiting state: " + currentState.stateIndex);
                    if (activeCoroutine != null)
                    {
                        StopCoroutine(activeCoroutine);
                        activeCoroutine = null;
                    }
                    if (currentState.Exit != null)
                    {
                        currentState.Exit();
                    }
                }

                // Update the current state
                currentState = newState;

                // Call enter on the new state
                timeInState = 0.0f;
                if (currentState != null && currentState.Enter != null)
                {
                    //Debug.Log("Now entering state: " + currentState.stateIndex);
                    currentState.Enter();
                }
                //Debug.Log("State now active: " + currentState.stateIndex);
                if (currentState != null && currentState.UpdateCR != null)
                {
                    //Debug.Log("Now entering state: " + currentState.stateIndex);
                    activeCoroutine = StartCoroutine(currentState.UpdateCR());
                }
            }
        }
        // ****************************************************************
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Protected Methods
        // ****************************************************************
        protected void RegisterState(State _newState)
        {
            //Debug.Log("Registering state: " + _newState.stateIndex);
            stateMachine[_newState.stateIndex] = _newState;
        }
        // ****************************************************************
        #endregion
        // ****************************************************************
    }
    #endregion
    // ********************************************************************

}
