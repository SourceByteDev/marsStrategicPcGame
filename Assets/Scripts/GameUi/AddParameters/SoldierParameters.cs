using Game.Units.Unit_Types;
using UnityEngine;
using UnityEngine.UI;

namespace GameUi.AddParameters
{
    public class SoldierParameters : MonoBehaviour
    {
        public GameObject itemsPanel;
        
        public Image activeImage;
        
        public void InActive()
        {
            itemsPanel.SetActive(false);

            activeImage.enabled = false;
        }

        public void UpdateActivePanel(ControlType type)
        {
            var isActive = type == ControlType.Soldier;

            itemsPanel.SetActive(isActive);
            
            activeImage.enabled = isActive;
            
            if (!isActive)
                return;
        }
    }
}