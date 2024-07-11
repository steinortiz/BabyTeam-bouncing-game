using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Babyteam.SO.UI
{
    [Serializable]
    public class OnSelectedEvent: UnityEvent {}
    [Serializable]
    public class OnDeselectedEvent: UnityEvent {}
    [Serializable]
    public class OnSubmitEvent: UnityEvent {}

    [Serializable]
    public class ExtendedButton : Selectable, ISubmitHandler, IPointerClickHandler
    {
    
        [SerializeField] private bool is3dObject;
        [SerializeField] public OnSelectedEvent onSelected;
        [SerializeField] public OnDeselectedEvent onDeselected;
        [SerializeField] public OnSubmitEvent onSubmit;
        
        public override void OnSelect(BaseEventData eventData)
        {
            onSelected?.Invoke();
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            onDeselected?.Invoke();
            base.OnDeselect(eventData);
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            onSelected?.Invoke();
            base.OnPointerEnter(eventData);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            onDeselected?.Invoke();
            base.OnPointerExit(eventData);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            onSubmit?.Invoke();
            base.Select();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            onSubmit?.Invoke();
            base.OnPointerDown(eventData);
        }
        
        void OnMouseDown()
        {
            if (is3dObject && interactable)
            {
                onSubmit?.Invoke();
            }	
        }

        private void OnMouseOver()
        {
            if(is3dObject && interactable) Select();
        }

        private void OnMouseExit()
        {
            if(is3dObject&& interactable) onDeselected?.Invoke();
        }
    }

    [Serializable]
    public enum SelectionState
    {
        NORMAL,
        HIGHLIGHTED,
        PRESSED,
        SELECTED,
        DISABLED 
    }
}

