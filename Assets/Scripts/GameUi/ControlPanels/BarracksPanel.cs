using System.Linq;
using Data;
using LogicHelper;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public class BarracksPanel : ControlPanel
    {
        [SerializeField] private UnitBuyButton[] buyButtons;
        
        public override void UpdateValues(UnitGameParameters parameters)
        {
            buyButtons.ToList().ForEach(x =>
            {
                x.Button.onClick.RemoveAllListeners();
                
                x.Button.onClick.AddListener(delegate
                {
                    UnitBuilder.AddUnitCurrentToBuild(x.Data);
                });
            });
        }
    }
}