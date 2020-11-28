using System;
using Common.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.GameHoverHelper
{
    public class HoverPanelControl : Singleton<HoverPanelControl>
    {
        [SerializeField] private ObjectParameters objects;
        
        public UnityAction<HoverElement> OnMouseEnterElement { get; set; }
        
        public UnityAction<HoverElement> OnMouseExitElement { get; set; }

        private HoverElement _lastElement;

        public void UpdateLastElement()
        {
            if (!objects.Panel.gameObject.activeSelf)
                return;
            
            if (_lastElement == null)
                return;
            
            ShowElement(_lastElement);
        }
        
        private void ShowElement(HoverElement element)
        {
            objects.SetActivePanel(true);
            
            objects.InitHoverPanel(element);

            _lastElement = element;
        }

        private void HideCurrentElement()
        {
            objects.SetActivePanel(false);
        }
        
        private void InitCallbacks()
        {
            OnMouseEnterElement += ShowElement;

            OnMouseExitElement += delegate
            {
                HideCurrentElement();
            };
        }
        
        private void Start()
        {
            InitCallbacks();
            
            HideCurrentElement();
        }

        [Serializable]
        private struct ObjectParameters
        {
            [SerializeField] private HoverPanel panel;

            public HoverPanel Panel => panel;

            public void InitHoverPanel(HoverElement element)
            {
                Panel.Init(element.Parameters);
            }
            
            public void SetActivePanel(bool isActive)
            {
                panel.gameObject.SetActive(isActive);
            }
        }
    }
}