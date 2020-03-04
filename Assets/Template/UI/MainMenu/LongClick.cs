using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ChangeMode { Increase, Decrease}
    public SettingsParameter settingsParameter;
    bool _pressed = false;
    public float firstPressRate = 1;
    private float pressRate;
    private float nextPress;
    private int rateIncrease;
    public ChangeMode changeMode;
    void Start()
    {
        pressRate = firstPressRate;
        rateIncrease = 1;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
        pressRate = firstPressRate;
        rateIncrease = 1;
        nextPress = 0;
    }

    void Update()
    {
        if ((_pressed) && (Time.time > nextPress) )
        {
            nextPress = Time.time + pressRate;
            if (pressRate > 0.25)
                pressRate -= (0.25f /** rateIncrease++*/);

            Debug.Log(pressRate);
            switch (changeMode)
            {
                case ChangeMode.Increase:
                    if (settingsParameter.index < settingsParameter.values.Length - 1)
                        settingsParameter.increase();
                    break;

                case ChangeMode.Decrease:
                    if (settingsParameter.index > 0)
                        settingsParameter.decrease();
                    break;
                default:
                    break;
            }
        }
    }
}
