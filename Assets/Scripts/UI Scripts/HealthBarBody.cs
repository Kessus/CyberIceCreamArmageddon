using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Contains all of the healthbar logic
//Handles displaying changes to character's health
public class HealthBarBody : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private bool isInitialized = false;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
        if (!isInitialized)
        {
            isInitialized = true;
        }
        else
        {
            Image[] imageComponents = gameObject.GetComponentsInChildren<Image>();
            foreach (Image image in imageComponents)
            {
                if (!image.enabled)
                    image.enabled = true;
            }
        }
    }
}
