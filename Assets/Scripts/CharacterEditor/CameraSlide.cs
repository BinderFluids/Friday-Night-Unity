using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CameraSlide : MonoBehaviour
{
    [SerializeField] Slider leftRight;
    [SerializeField] Slider upDown;
    [SerializeField] private float scrollFactor;

    private void Awake()
    {
        leftRight.onValueChanged.AddListener(delegate { SliderChanged(); });
        upDown.onValueChanged.AddListener(delegate { SliderChanged(); });
    }

    public void ReturnToZero()
    {
        leftRight.value = 0;
        upDown.value = 0;
    }

    private void SliderChanged()
    {
        transform.position = new Vector3(leftRight.value * scrollFactor, upDown.value * scrollFactor, transform.position.z);
    }
}
