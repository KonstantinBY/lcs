// Copyright (C) 2015-2023 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.UI;

namespace sl.controller
{
    // Utility class for swapping the sprite of a UI Image between two predefined values.
    public class SpriteSwapper : MonoBehaviour
    {
        public Sprite enabledSprite;
        public Sprite disabledSprite;

        internal bool isEnabled = true;

        private Image m_image;

        public void Awake()
        {
            m_image = GetComponent<Image>();
        }

        public void swapSprite()
        {
            if (isEnabled)
            {
                isEnabled = false;
                m_image.sprite = disabledSprite;
            }
            else
            {
                isEnabled = true;
                m_image.sprite = enabledSprite;
            }
        }

        public void setSprite(bool isEnable)
        {
            if (isEnable)
            {
                isEnabled = true;
                m_image.sprite = enabledSprite;
            }
            else
            {
                isEnabled = false;
                m_image.sprite = disabledSprite;
            }
        }
    }
}
