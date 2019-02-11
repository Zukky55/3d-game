using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReviewGames
{
    public class FieldmapController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        Rigidbody rb;

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("called OnBeginDrag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            var touch = Input.GetTouch(0);
            var vector = touch.deltaPosition;
            rb.AddTorque(vector, ForceMode.Force);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("called OnEndDrag");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("yobaretao");
        }
    }
}