using UnityEngine;


namespace Bounder.Framework
{
    public class ShowableObjectSlide : ShowableObject
    {
        // ********************************************************************
        #region Editor Variables 
        // ********************************************************************
        [SerializeField]
        protected Vector3 hiddenOffset = Vector3.zero;
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Private Variables 
        // ********************************************************************
        Vector3 hiddenPos = Vector3.zero;
        Vector3 shownPos = Vector3.zero;
        Vector3 transStartPos = Vector3.zero;
        RectTransform rect = null;
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region Properties 
        // ********************************************************************
        private Vector3 Pos
        {
            get
            {
                if (rect)
                    return rect.anchoredPosition3D;
                else
                    return transform.localPosition;
            }
            set
            {
                if (rect)
                    rect.anchoredPosition3D = value;
                else
                    transform.localPosition = value;
            }
        }
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region MonoBehaviour Functions 
        // ********************************************************************
        protected override void Awake()
        {
            if (transform is RectTransform)
                rect = transform as RectTransform;

            shownPos = Pos;

            hiddenPos = shownPos + hiddenOffset;

            base.Awake();
        }
        // ********************************************************************
        #endregion
        // ********************************************************************



        // ********************************************************************
        #region State Functions 
        // ********************************************************************
        protected override void HIDDEN_Enter()
        {
            // Ensure canvas group is hidden and non-interactable
            Pos = hiddenPos;

            base.HIDDEN_Enter();
        }
        // ********************************************************************
        protected override void SHOWING_Enter()
        {
            transStartPos = Pos;

            base.SHOWING_Enter();
        }
        // ********************************************************************
        protected override void SHOWING_Update()
        {
            if (timeInState < m_transDuration)
            {
                // lerp alpha
                Vector3 movementVector = shownPos - transStartPos;
                Pos = transStartPos + movementVector * Easing.BackEaseOut(timeInState, 0, 1.0f, m_transDuration);
            }

            base.SHOWING_Update();
        }
        // ********************************************************************
        protected override void SHOWN_Enter()
        {
            // Fully visible
            Pos = shownPos;

            base.SHOWN_Enter();
        }
        // ********************************************************************
        protected override void HIDING_Enter()
        {
            transStartPos = Pos;

            base.HIDING_Enter();
        }
        // ********************************************************************
        protected override void HIDING_Update()
        {
            if (timeInState < m_transDuration)
            {
                // lerp alpha
                Vector3 movementVector = hiddenPos - transStartPos;
                Pos = transStartPos + movementVector * Easing.BackEaseIn(timeInState, 0, 1.0f, m_transDuration);
            }

            base.HIDING_Update();
        }
        // ********************************************************************
        #endregion
        // ********************************************************************

    }
}
