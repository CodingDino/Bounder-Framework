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
using System.Collections.Generic;
using UnityEngine;
#endregion
// ************************************************************************


namespace Bounder.Framework
{
    // ********************************************************************
    #region Class: StateMachine
    // ********************************************************************

    public class StateMachine 
    {
        // ****************************************************************
        #region State Information
        // ****************************************************************
        public delegate void StateFunction();
        [System.Serializable]
        public class State
        {
            public int stateIndex;
            public StateFunction Enter;
            public StateFunction Update;
            public StateFunction Exit;

            public State()
            {
                this.stateIndex = -1;
                Enter = null;
                Update = null;
                Exit = null;
            }

            public State(int stateIndex, StateFunction enter, StateFunction update, StateFunction exit)
            {
                this.stateIndex = stateIndex;
                Enter = enter;
                Update = update;
                Exit = exit;
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
        #endregion
        // ****************************************************************


        // ****************************************************************
        #region Public Methods
        // ****************************************************************
        public virtual void Update()
        {
            // Call update on the current state
            timeInState += Time.deltaTime;
            if (currentState.Update != null)
                currentState.Update();
        }
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
        public void RegisterState(State _newState)
        {
            //Debug.Log("Registering state: " + _newState.stateIndex);
            stateMachine[_newState.stateIndex] = _newState;
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
                if (currentState != null && currentState.Exit != null)
                {
                    //Debug.Log("Now exiting state: " + currentState.stateIndex);
                    currentState.Exit();
                }

                // Update the current state
                currentState = newState;

                // Call enter on the new state
                timeInState = 0.0f;
                if (currentState.Enter != null)
                {
                    //Debug.Log("Now entering state: " + currentState.stateIndex);
                    currentState.Enter();
                }
                //Debug.Log("State now active: " + currentState.stateIndex);
            }
        }
        // ****************************************************************
        #endregion
        // ****************************************************************
    }
    #endregion
    // ********************************************************************

}
