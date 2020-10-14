using Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameUi.AddParameters
{
    public class BuildItem : MonoBehaviour
    {
        public Image imageAvatar;

        public Image fillCurrent;
        
        public void Init(BuildUnitParameters data, Sprite avatar)
        {
            imageAvatar.sprite = avatar;

            var valueFill = (float) data.currentSeconds / data.needSeconds;

            fillCurrent.fillAmount = valueFill;
        }
    }
}