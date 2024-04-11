using UnityEngine;

namespace Bounder.Framework
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ShowableObjectAlphaCanvas : ShowableObject
    {
        // ********************************************************************
        #region Private Variables 
        // ********************************************************************
        CanvasGroup canvasGroup = null;
        float originalAlpha = 1.0f;
        bool shouldBlockRaycast = false;
        bool shouldBeInteractable = false;
        float transStartAlpha = 1.0f;
        // ********************************************************************
        #endregion
        // ********************************************************************


        // ********************************************************************
        #region MonoBehaviour Functions 
        // ********************************************************************
        protected override void Awake()
        {
            // Get components
            canvasGroup = GetComponent<CanvasGroup>();
            originalAlpha = canvasGroup.alpha;
            shouldBeInteractable = canvasGroup.interactable;
            shouldBlockRaycast = canvasGroup.blocksRaycasts;

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
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

            base.HIDDEN_Enter();
        }
        // ********************************************************************
        protected override void SHOWING_Enter()
        {
            canvasGroup.blocksRaycasts = shouldBlockRaycast;
            transStartAlpha = canvasGroup.alpha;

            base.SHOWING_Enter();
        }
        // ********************************************************************
        protected override void SHOWING_Update()
        {
            if (timeInState < m_transDuration)
            {
                // lerp alpha
                canvasGroup.alpha = Easing.QuadEaseOut(timeInState, transStartAlpha, originalAlpha - transStartAlpha, m_transDuration);
            }

            base.SHOWING_Update();
        }
        // ********************************************************************
        protected override void SHOWN_Enter()
        {
            // Fully visible
            canvasGroup.alpha = originalAlpha;
            canvasGroup.blocksRaycasts = shouldBlockRaycast;
            canvasGroup.interactable = shouldBeInteractable;

            base.SHOWN_Enter();
        }
        // ********************************************************************
        protected override void HIDING_Enter()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            transStartAlpha = canvasGroup.alpha;

            base.HIDING_Enter();
        }
        // ********************************************************************
        protected override void HIDING_Update()
        {
            if (timeInState < m_transDuration)
            {
                // lerp alpha
                canvasGroup.alpha = Easing.QuadEaseIn(timeInState, transStartAlpha, -transStartAlpha, m_transDuration);
            }

            base.HIDING_Update();
        }
        // ********************************************************************
        #endregion
        // ********************************************************************

    }
}