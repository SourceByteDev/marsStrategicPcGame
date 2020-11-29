using System;
using System.Collections;
using System.Linq;
using Common.Extensions;
using TMPro;
using UnityEngine;

namespace GameUi.Message
{
    public class MessageShow : Singleton<MessageShow>
    {
        [SerializeField] private TypeMessageData[] typedMessages;

        [SerializeField] private VisualShowParameters visuals;

        private Coroutine _currentToCloseMessage;

        public void ShowMessage(TypedMessage message)
        {
            if (typedMessages.All(x => x.Type != message))
                return;
            
            var founded = typedMessages.ToList().Find(x => x.Type == message);
            
            ShowMessage(founded.Message);
        }
        
        public void ShowMessage(string message)
        {
            visuals.ActivePanel(message);
            
            if (_currentToCloseMessage != null)
                StopCoroutine(_currentToCloseMessage);
            
            _currentToCloseMessage = StartCoroutine(WaitingToCloseMessage(visuals.WaitTimeShowing));
        }

        [ContextMenu("Test1")]
        private void Test1()
        {
            ShowMessage("1");
        }

        [ContextMenu("Test2")]
        private void Test2()
        {
            ShowMessage("2");
        }

        private void Start()
        {
            visuals.DeActivePanel();
        }

        private IEnumerator WaitingToCloseMessage(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            visuals.DeActivePanel();
        }

        [Serializable]
        private struct VisualShowParameters
        {
            [SerializeField] private GameObject activePanel;

            [SerializeField] private TMP_Text messageText;

            [SerializeField] private float waitTimeShowing;

            public float WaitTimeShowing => waitTimeShowing;

            public void ActivePanel(string message)
            {
                activePanel.SetActive(true);
                
                messageText.text = message;
            }

            public void DeActivePanel()
            {
                activePanel.SetActive(false);
            }
        }
        
        [Serializable]
        private struct TypeMessageData
        {
            [SerializeField] private TypedMessage type;

            [SerializeField] private string message;

            public TypedMessage Type => type;

            public string Message => message;
        }
        
        public enum TypedMessage
        {
            NotEnoughSupply,
            NotEnoughCrystals
        }
    }
}