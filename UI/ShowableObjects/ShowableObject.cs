using UnityEngine;
using Bounder.Framework;

namespace Bounder.Framework { 
    public class ShowableObject : StateMachine
    {
        // ********************************************************************
        #region ShowableObjectState Enum 
        // ********************************************************************
        public enum ShowableObjectState
        {
            HIDDEN,
            SHOWING,
            SHOWN,
            HIDING,
            MAX
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Editor Variables 
        // ********************************************************************
        [SerializeField]
        protected float m_transDuration = 0.2f;
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Properties 
        // ********************************************************************
        public bool Shown
        {
            get {
                return currentState != null && currentState.stateIndex == (int)ShowableObjectState.SHOWN;
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Public Functions 
        // ********************************************************************
        public void Show()
        {
            if (currentState.stateIndex == (int)ShowableObjectState.HIDDEN || currentState.stateIndex == (int)ShowableObjectState.HIDING)
            {
                ChangeState((int)ShowableObjectState.SHOWING);
            }
        }
        // ********************************************************************
        public void Hide()
        {
            if (currentState.stateIndex == (int)ShowableObjectState.SHOWN || currentState.stateIndex == (int)ShowableObjectState.SHOWING)
            {
                ChangeState((int)ShowableObjectState.HIDING);
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region MonoBehaviour Functions 
        // ********************************************************************
        protected virtual void Awake()
        {
            // Setup state machine
            RegisterState(new State((int)ShowableObjectState.HIDDEN, HIDDEN_Enter, null, null));
            RegisterState(new State((int)ShowableObjectState.SHOWING, SHOWING_Enter, SHOWING_Update, null));
            RegisterState(new State((int)ShowableObjectState.SHOWN, SHOWN_Enter, null, null));
            RegisterState(new State((int)ShowableObjectState.HIDING, HIDING_Enter, HIDING_Update, null));

            ChangeState((int)ShowableObjectState.HIDDEN);
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region State Functions 
        // ********************************************************************
        protected virtual void HIDDEN_Enter()
        {
            // Implement in child
        }
        // ********************************************************************
        protected virtual void SHOWING_Enter()
        {
            // Implement in child
        }
        // ********************************************************************
        protected virtual void SHOWING_Update()
        {
            if (timeInState >= m_transDuration)
            {
                ChangeState((int)ShowableObjectState.SHOWN);
            }
        }
        // ********************************************************************
        protected virtual void SHOWN_Enter()
        {
            // Implement in child
        }
        // ********************************************************************
        protected virtual void HIDING_Enter()
        {
            // Implement in child
        }
        // ********************************************************************
        protected virtual void HIDING_Update()
        {
            if (timeInState >= m_transDuration)
            {
                ChangeState((int)ShowableObjectState.HIDDEN);
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************
    }
}
