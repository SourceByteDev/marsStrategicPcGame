using Data;
using UnityEngine;

namespace GameUi.ControlPanels
{
    public abstract class ControlPanel : MonoBehaviour
    {
        public abstract void UpdateValues(UnitGameParameters parameters);
    }
}