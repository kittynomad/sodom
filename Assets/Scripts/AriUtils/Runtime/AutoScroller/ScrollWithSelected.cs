/*****************************************************************************
// File Name : ScrollWithSelected.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Automatically scrolls a ScrollView to show the selected button.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AriUtils
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollWithSelected : MonoBehaviour
    {
        #region Vars
        [SerializeField, Tooltip("The rect transform that holds the content of the scroll rect.")]
        private RectTransform m_contentTransform;
        [SerializeField, Tooltip("The rect transform that holds the viewport.")]
        private RectTransform m_viewportTransform;
        [SerializeField, Tooltip("Whether the scrolling movement of the scroll rect should be affected by Time.timeScale.")]
        private bool m_useScaledTime;
        [SerializeField, Tooltip("The extra margin of space that should come between the selected object and the edge of the viewport.")]
        private float m_scrollMargin;
        [SerializeField, Tooltip("How quickly the scroll rect should LERP to a position where the selceted object is in view.")]
        private float m_scrollSpeed;

        private ScrollRect m_scrollRect;

        private Coroutine m_scrollingCoroutine;
        #endregion

        private void Awake()
        {
            m_scrollRect = GetComponent<ScrollRect>();

            SetupSelectionDetectors(m_scrollRect.content.GetComponentsInChildren<ScrollSelectDetector>());
        }

        /// <summary>
        /// Assigns a reference to this component to all ScrollSelectDetectors that should update this ScrollRect.
        /// </summary>
        /// <param name="detectors"> The ScrollSelectDetectors to update. </param>
        private void SetupSelectionDetectors(ScrollSelectDetector[] detectors)
        {
            foreach (ScrollSelectDetector detector in detectors)
            {
                detector.ScrollController = this;
            }
        }

        /// <summary>
        /// Checks for the selected selected button being out of the range of the ScrollRect.
        /// </summary>
        public void CheckSelected()
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;

            /// Prevents scrolling if no object is selected.
            if (selected == null) { return; }
            /// Prevents scrolling if the selected object is outside of the Content of this scroll view.
            if (selected.transform.parent != m_contentTransform) { return; }

            RectTransform selectedTrans = (RectTransform)selected.transform;

            /// Find the local position of the selected button relative to the viewport.
            float selectedPos = selectedTrans.localPosition.y + m_contentTransform.localPosition.y;

            /// Defines the top and bottom y positions of the viewport.
            float top = -(selectedTrans.rect.height / 2);
            float bottom = (selectedTrans.rect.height / 2) - m_viewportTransform.rect.height;
            //Debug.Log(m_viewportTransform.localPosition.y);

            /// Checks if the local position of the selected button is above or below the defined top and bottom points of the viewport.
            if (selectedPos > top)
            {
                float targetPositionNormalized = GetTargetPositionNormalized(selectedPos, top, m_scrollMargin);
                //Debug.Log(targetPositionNormalized);
                if (m_scrollingCoroutine != null)
                {
                    StopCoroutine(m_scrollingCoroutine);
                }
                m_scrollingCoroutine = StartCoroutine(ScrollToNormalizedPosition(targetPositionNormalized));
            }
            else if (selectedPos < bottom)
            {
                float targetPositionNormalized = GetTargetPositionNormalized(selectedPos, bottom, -m_scrollMargin);
                //Debug.Log(targetPositionNormalized);
                /// Prevents two scrolling coroutines from existing at once.
                if (m_scrollingCoroutine != null)
                {
                    StopCoroutine(m_scrollingCoroutine);
                }
                m_scrollingCoroutine = StartCoroutine(ScrollToNormalizedPosition(targetPositionNormalized));
            }
            else
            {
                //Do nothing.
                //Debug.Log("Within");
            }
        }

        /// <summary>
        /// Gets the normalized position of the ScrollRect that would put the selected button within the bounds.
        /// </summary>
        /// <param name="selectedPos"> The position of the selected button to find the normalized position of. </param>
        /// <param name="exceededBound"> The upper or lower bound of the viewport that the selected button has passed. </param>
        /// <returns> a float between 0-1 that is the normalized position of the ScrollRect that puts the button back within the bounds. </returns>
        private float GetTargetPositionNormalized(float selectedPos, float exceededBound, float margin)
        {
            /// Find the absolute amount of units that are contained within the 0-1 of the normalizedPosition.
            float contentHeightDifference = m_contentTransform.rect.height - m_viewportTransform.rect.height;
            /// Calcualte the amount that the selected button is offset from the bounds of the viewport defined by top and bottom.
            float deltaPos = selectedPos - exceededBound + margin;
            /// Convert the delta position into a delta normalized position for the scroll rect by dividing it by the number of unity units it covers with
            /// it's normalized position
            float deltaNomralized = deltaPos / contentHeightDifference;
            /// Calcualtes the target position that the scroll rect should snap to to have the button in frame.
            return deltaNomralized + m_scrollRect.normalizedPosition.y;
        }

        /// <summary>
        /// Lerps the scroll rect to a passed in normalized target positon.
        /// </summary>
        /// <param name="targetPosition"> The normalized position that the scroll rect should move to. </param>
        private IEnumerator ScrollToNormalizedPosition(float targetPosition)
        {
            Vector2 normalPos = m_scrollRect.normalizedPosition;
            while (!ApproximatelyWithin(m_scrollRect.normalizedPosition.y, targetPosition, 0.01f))
            {
                normalPos = m_scrollRect.normalizedPosition;
                //Double check this lerp.
                float timeValue = m_useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
                float blend = 1 - Mathf.Pow(0.5f, m_scrollSpeed * timeValue);
                normalPos.y = Mathf.Lerp(normalPos.y, targetPosition, blend);
                m_scrollRect.normalizedPosition = normalPos;
                yield return null;
            }
            /// Snaps the scroll view to the target position once the two are close enough.
            normalPos.y = targetPosition;
            m_scrollRect.normalizedPosition = normalPos;

            Debug.Log("Done Scrolling");
            m_scrollingCoroutine = null;
        }

        /// <summary>
        /// Checks to see if two numbers are within a certain range of each other
        /// </summary>
        /// <param name="number1"> A number to compare. </param>
        /// <param name="number2"> A second number to compare. </param>
        /// <param name="range"> The numerical range that the two must be within. (Inclusive) </param>
        /// <returns> Whether the two numbers are within range of each other. </returns>
        public static bool ApproximatelyWithin(float number1, float number2, float range = 1E-6f)
        {
            float delta = number1 - number2;
            return Mathf.Abs(delta) <= range;
        }
    }

}