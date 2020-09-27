using System;
using System.Collections.Generic;
using System.Linq;
using Game.Units.Unit_Types;
using UnityEngine;

namespace GameUi
{
    public class ControlPanel : MonoBehaviour
    {
        public ControlPanelData[] pool;

        public SoldierParameters soldierPanel;

        private IEnumerable<GameObject> AllObjects => pool.Select(x => x.panel).ToArray();

        public void InActive()
        {
            AllObjects.ToList().ForEach(x => x.SetActive(false));
            
            soldierPanel.itemsPanel.SetActive(false);
        }
        
        public void OpenRightPanel(ControlType type)
        {
            InActive();
            
            if (type == ControlType.None)
                return;

            var founded = pool.First(x => x.type == type);

            founded?.panel.SetActive(true);
            
            soldierPanel.UpdateActivePanel(type);
        }

        [Serializable]
        public class ControlPanelData
        {
            public ControlType type;
            
            public GameObject panel;
        }
    }
}