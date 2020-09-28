using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameUi
{
    public class CentralPanel : MonoBehaviour
    {
        public Image avatarImage;

        public Text nameText;

        public Image fillHealth;

        public Text countText;
        
        public GameObject activeObjects;

        public void InitItem(UnitGameParameters parameters)
        {
            activeObjects.SetActive(true);
            
            avatarImage.sprite = parameters.avatar;

            nameText.text = parameters.unitName;

            fillHealth.fillAmount = (float) parameters.currentHealth / parameters.startHealth;

            countText.text = $"{parameters.currentHealth}/{parameters.startHealth}";
        }
        
        public void InActive()
        {
            activeObjects.SetActive(false);
        }
        
    }
}