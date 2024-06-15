using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

     [SerializeField] private Slider slider;

    [SerializeField] private Camera camera;

    [SerializeField] private Transform target;


    public void UpdateHealthBar(float currentHealth, float maxHealth) {
        slider.value = currentHealth / maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if(camera && target) {
            transform.rotation = camera.transform.rotation;
            transform.position = target.position;
        }
        
    }
}
