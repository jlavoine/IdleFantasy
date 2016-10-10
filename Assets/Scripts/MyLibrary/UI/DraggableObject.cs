using UnityEngine;

namespace MyLibrary {
    [RequireComponent( typeof( RectTransform ) )]
    public class DraggableObject : MonoBehaviour {
        private RectTransform m_rect;
        public RectTransform Rect {
            get {
                if ( m_rect == null ) {
                    m_rect = GetComponent<RectTransform>();
                }

                return m_rect;
            }
        }

        public void OnDrag( UnityEngine.EventSystems.BaseEventData eventData ) {
            var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
            if ( pointerData == null ) {
                return;
            }

            var currentPosition = Rect.position;
            currentPosition.x += pointerData.delta.x;
            currentPosition.y += pointerData.delta.y;
            Rect.position = currentPosition;
        }
    }
}
