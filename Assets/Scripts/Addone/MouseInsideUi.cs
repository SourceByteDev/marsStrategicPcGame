using FetchUi;
using UnityEngine;
using UnityEngine.UI;

namespace Addone
{
    public static class MouseInsideUi
    {
        private static Vector2 MousePosition => Input.mousePosition;

        public static bool IsMouseInside(Rect rect)
        {
            return rect.Contains(MousePosition);
        }
        
        public static bool IsMouseInside(RectTransform rectTransform)
        {
            return IsMouseInside(rectTransform.rect);
        }
        
        public static bool IsMouseInside(Image image)
        {
            return IsMouseInside(image.rectTransform);
        }

        public static bool IsMouseInside(FetchButton button)
        {
            return IsMouseInside(button.image);
        }
    }
}