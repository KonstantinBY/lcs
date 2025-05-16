using UnityEngine;
using UnityEngine.UI;

namespace sl.controller
{
    public class ColorSwapper : MonoBehaviour
    {
        public Color enabledColor;
        public Color disabledColor;

        private bool isEnabled = true;

        private Image m_image;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        public void SwapColor()
        {
            setColor(!isEnabled);
        }
        
        internal void setColor(bool isEnabled)
        {
            if (isEnabled)
            {
                this.isEnabled = true;
                m_image.color = enabledColor;
                
            }
            else
            {
                this.isEnabled = false;
                m_image.color = disabledColor;
            }
        }
    }
}
