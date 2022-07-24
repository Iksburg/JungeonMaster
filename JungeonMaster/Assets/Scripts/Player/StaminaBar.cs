using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class StaminaBar : MonoBehaviour
    {
        [Header("Slider")]
        public Slider slider;
        
        public void SetMaxStamina(float stamina)
        {
            slider.maxValue = stamina;
            slider.value = stamina;
        }
        
        public void SetStamina(float stamina)
        {
            slider.value = stamina;
        }
    }
}
