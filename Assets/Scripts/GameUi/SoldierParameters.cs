using Game.Units.Unit_Types;
using UnityEngine;

namespace GameUi
{
    public class SoldierParameters : MonoBehaviour
    {
        public GameObject itemsPanel;

        public void UpdateActivePanel(ControlType type)
        {
            var isActive = type == ControlType.Soldier;

            itemsPanel.SetActive(isActive);
            
            if (!isActive)
                return;
        }
    }
}