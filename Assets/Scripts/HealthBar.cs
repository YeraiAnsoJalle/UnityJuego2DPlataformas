using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    public void SetHealth(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
