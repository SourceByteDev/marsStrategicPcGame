﻿using System;
using Addone;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FetchUi
{
    [RequireComponent(typeof(Image))]
    public class FetchButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private bool interactable = true;

        [SerializeField] private bool lockChangeSprite;

        [Space(15)] 
        
        public Sprite disabledSprite;

        [FormerlySerializedAs("highLightedSprite")] public Sprite selectedSprite;

        public Sprite pressedSprite;

        [Space(15)]

        public UnityEvent onClick;


        // Saved variable

        [HideInInspector] public Image image;

        private Sprite normalSprite;

        public bool LockChangeSprite
        {
            get => lockChangeSprite;
            set
            {
                lockChangeSprite = value;
                
                if (value)
                    return;

                CurrentSprite = MouseInsideUi.IsMouseInside(this) ? normalSprite : selectedSprite;
            }
        }

        private Sprite CurrentSprite
        {
            get => image.sprite;

            set
            {
                if (!Interactable || LockChangeSprite)
                    return;

                image.sprite = value;
            }
        }

        public bool Interactable
        {
            get => interactable;

            set
            {
                interactable = value; 
                
                UpdateInteractable();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectedSprite == null)
                return;
            
            CurrentSprite = selectedSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (pressedSprite != null && CurrentSprite == pressedSprite)
                return;
            
            CurrentSprite = normalSprite;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (pressedSprite == null)
                return;
            
            CurrentSprite = pressedSprite;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerEnter == gameObject)
            {
                OnPointerEnter(eventData);
                
                return;
            }

            CurrentSprite = normalSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable)
                return;
            
            onClick.Invoke();
        }
        
        private void UpdateInteractable()
        {
            var isNullDisable = disabledSprite == null;

            var rightDisabledSprite = disabledSprite == null ? normalSprite : disabledSprite;

            image.sprite = Interactable ? normalSprite : rightDisabledSprite;
        }
        
        private void Start()
        {
            image = GetComponent<Image>();

            normalSprite = image.sprite;
            
            
        }
    }
}