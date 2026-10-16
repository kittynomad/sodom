/*****************************************************************************
// File Name : ScrollSelectDetector.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Detects when this object is selected so that it can notify it's parent ScrollWithSelected.
*****************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;

namespace AriUtils
{
    public class ScrollSelectDetector : MonoBehaviour, ISelectHandler
    {
        [SerializeField, Tooltip("Whether this element should search for a ScrollWithSelected component in it's parent heirarchy if it does " +
            "not have one assigned by default.")]
        private bool m_searchForScrollController;
        public ScrollWithSelected ScrollController { get; set; }

        public void OnSelect(BaseEventData eventData)
        {
            if (ScrollController == null)
            {
                if (m_searchForScrollController)
                {
                    ScrollController = FindScrollControllerInParents(gameObject.transform.parent.gameObject);
                    if (ScrollController == null)
                    {
                        Debug.LogWarning("Scroll Select Detector component on " + gameObject.name + " could not find a ScrollWithSelected " +
                            "component in its parents.");
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning("Scroll Controller not found.  If this behaviour is unavoidable, try turning on Search For Scroll Controller");
                    return;
                }
            }
            ScrollController.CheckSelected();
        }

        /// <summary>
        /// Searches the parents of this GameObject for a ScrollWithSelected component that can be stored to this component.
        /// </summary>
        /// <param name="gameObject"> The parent GameObject to start with. </param>
        /// <returns> The found ScrollWithSelected component, if any.</returns>
        private ScrollWithSelected FindScrollControllerInParents(GameObject gameObject)
        {
            ScrollWithSelected scrollCont = null;
            /// Iterates through each parent that the game object has and looks for a ScrollWithSelected component to store a link to.
            while (scrollCont == null)
            {
                if (gameObject == null)
                {
                    Debug.Log("No component found in parents.");
                    return null;
                }

                scrollCont = gameObject.GetComponent<ScrollWithSelected>();

                if (gameObject.transform.parent == null)
                {
                    Debug.Log("No component found in parents.");
                    return null;
                }
                gameObject = gameObject.transform.parent.gameObject;
            }
            return scrollCont;
        }
    }

}