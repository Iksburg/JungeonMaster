using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class HealthBar : MonoBehaviour
    {
        [Header("Slider")]
        public Slider slider;
        
        public void SetMaxHealth(float health)
        {
            slider.maxValue = health;
            slider.value = health;
        }
        
        public void SetHealth(float health)
        {
            slider.value = health;
        }
    }
}
