using UnityEngine;

namespace Menu
{
    public class ButtonHoverLight : MonoBehaviour
    {
        public void ActiveHover(bool isActive)
        {
            MenuControl.Instance.hoverLight.gameObject.SetActive(isActive);

            MenuControl.Instance.hoverLight.transform.position = transform.position;
        }
    }
}
