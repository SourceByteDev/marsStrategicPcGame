using Game.Units.Unit_Types;
using UnityEngine;
using UnityEngine.UI;

namespace GameUi.AddParameters
{
    public class BuildingListParameters : MonoBehaviour
    {
        public GameObject activeItems;

        public Image activeImage;

        public void InActive()
        {
            activeItems.SetActive(false);

            activeImage.enabled = false;
        }
        
        public void UpdateActivePanel(ControlType type)
        {
            var isActive = type == ControlType.House;

            activeItems.SetActive(isActive);

            activeImage.enabled = isActive;
            
            if (!isActive)
                return;
        }
    }
}