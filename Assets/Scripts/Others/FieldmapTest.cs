using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReviewGames
{
    public class FieldmapTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,IPointerDownHandler
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
            Debug.Log("OnPointerClick yobaretao");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown yobaretao");
        }
    }
}