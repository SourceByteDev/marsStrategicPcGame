using System;
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

        [SerializeField] private bool changeColorInActive;

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

        public bool Interactable
        {
            get => interactable;

            set
            {
                interactable = value; 
                
                UpdateInteractable();
            }
        }

        private bool IsChangeByColor => disabledSprite == null && changeColorInActive;
        
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectedSprite == null || CurrentSprite != normalSprite)
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
                CurrentSprite = normalSprite;
                
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
        
        [ContextMenu("Update interactable")]
        private void UpdateInteractable()
        {
            image = GetComponent<Image>();

            if (IsChangeByColor)
            {
                image.color = Interactable ? Color.white : Color.gray;
                
                return;
            }
            
            var isNullDisable = disabledSprite == null;

            var isNullNormal = normalSprite == null;
            
            if (isNullDisable && !Interactable || isNullNormal && Interactable)
                return;

            image.sprite = Interactable ? normalSprite : disabledSprite;
        }
        
        private void Start()
        {
            image = GetComponent<Image>();

            normalSprite = image.sprite;
            
            UpdateInteractable();
        }
    }
}