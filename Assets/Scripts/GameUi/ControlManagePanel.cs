using System;
using System.Collections.Generic;
using System.Linq;
using Game.Units.Control;
using Game.Units.Unit_Types;
using GameUi.AddParameters;
using GameUi.ControlPanels;
using UnityEngine;

namespace GameUi
{
    public class ControlManagePanel : MonoBehaviour
    {
        public ControlPanelData[] pool;

        public SoldierParameters soldierPanel;

        public BuildingListParameters buildingPanel;

        private IEnumerable<GameObject> AllObjects => pool.Select(x => x.panel.gameObject).ToArray();

        public void InActive()
        {
            AllObjects.ToList().ForEach(x => x.SetActive(false));

            soldierPanel.InActive();
            
            buildingPanel.InActive();
        }

        public void OpenRightPanel(ControlType type, bool isOpenOthers)
        {
            InActive();

            if (!isOpenOthers)
                return;
            
            var founded = pool.First(x => x.type == type);

            if (founded == null)
                return;
            
            founded.panel.gameObject.SetActive(true);

            founded.panel.UpdateValues(UnitSelector.Instance.SelectedUnit.gameParameters);
            
            soldierPanel.UpdateActivePanel(type);
            
            buildingPanel.UpdateActivePanel(type);
        }

        [Serializable]
        public class ControlPanelData
        {
            public ControlType type;

            public ControlPanel panel;
        }
    }
}