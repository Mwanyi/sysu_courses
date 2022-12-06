using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar2 : MonoBehaviour
{
    public Slider slider;

    public bool restart;

    void Start() {
        slider.maxValue = 5000.0F;
        slider.minValue = 0F;
        slider.value = slider.maxValue;
        restart = false;
    }

    void Update() {

        // 面向摄像机，此时血条会发生抖动
        // slider.direction = Slider.Direction.RightToLeft;
        // slider.transform.LookAt(Camera.main.transform.position);

        // 不是面向摄像机，而是面向游戏屏幕
        slider.direction = Slider.Direction.LeftToRight;
        slider.transform.rotation = Camera.main.transform.rotation;
        

        if(restart){
            slider.maxValue = 5000.0F;
            slider.value = slider.maxValue;
            restart = false;
        }

        if(slider.value <= 0.3 * slider.maxValue){
            slider.fillRect.transform.GetComponent<Image>().color = Color.red;
        }
        else if(slider.value <= 0.6 * slider.maxValue){
            slider.fillRect.transform.GetComponent<Image>().color = Color.yellow;
        }
        else{
            slider.fillRect.transform.GetComponent<Image>().color = Color.green;
        }
    }

    public void Hurt(float h) {
        slider.value -= h;
        if(slider.value <= 0){
            slider.value = 0;
        }
    }

    public void Heal(float h) {
        if(slider.value > 0){
            slider.value += h;
            if(slider.value >= slider.maxValue) {
                slider.value = slider.maxValue;
            }
        }
    }

}
