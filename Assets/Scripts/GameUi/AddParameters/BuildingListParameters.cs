using System.Collections.Generic;
using System.Linq;
using Game.Units.Control;
using Game.Units.Unit_Types;
using UnityEngine;
using UnityEngine.UI;

namespace GameUi.AddParameters
{
    public class BuildingListParameters : MonoBehaviour
    {
        public GameObject activeItems;

        public Image activeImage;

        public Transform parentItems;

        public BuildItem itemPrefab;

        private static IEnumerable<ControlType> BuildingActive => new[]
        {
            ControlType.House,
            ControlType.Barracks,
            ControlType.Factory,
            ControlType.FlyPort,
            ControlType.Laboratory
        };

        public void InActive()
        {
            activeItems.SetActive(false);

            activeImage.enabled = false;
        }

        public void UpdateActivePanel(ControlType type)
        {
            var isActive = BuildingActive.Contains(type);

            activeItems.SetActive(isActive);

            activeImage.enabled = isActive;

            ClearAllItems();

            if (!isActive)
                return;

            var currentBuilds = UnitSelector.Instance.SelectedUnit.gameParameters.currentBuilds;

            if (currentBuilds.Count <= 0)
                return;

            foreach (var build in currentBuilds.ToList())
            {
                var newUnitItem = Instantiate(itemPrefab, parentItems);

                newUnitItem.Init(build, build.toBuildUnit.parameters.buildSprite);
            }
        }

        private void ClearAllItems()
        {
            if (parentItems.childCount <= 0)
                return;

            for (var i = 0; i < parentItems.childCount; i++)
                Destroy(parentItems.GetChild(i).gameObject);
        }
    }
}