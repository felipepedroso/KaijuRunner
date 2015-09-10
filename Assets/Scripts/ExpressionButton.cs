using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ExpressionButton
{
    private int lastIntensity;
    private float lastDebounceTime;
    private float debounceDelay;
    private bool buttonState;
    public bool ButtonUp { get; private set; }
    public bool ButtonDown { get; private set; }
    public bool ButtonHold { get; private set; }
    private PXCMFaceData.ExpressionsData.FaceExpression Expression { get; set; }
    private int ExpressionThreshold { get; set; }

    public ExpressionButton(PXCMFaceData.ExpressionsData.FaceExpression expression, int expressionThreshold, float debounceDelay)
    {
        Expression = expression;
        ExpressionThreshold = expressionThreshold;
        lastIntensity = 0;
        this.debounceDelay = debounceDelay;
    }

    public void UpdateStatus(PXCMFaceData.ExpressionsData expressionsData)
    {
        ButtonUp = false;
        ButtonDown = false;
        ButtonHold = false;

        if (expressionsData != null)
        {
            PXCMFaceData.ExpressionsData.FaceExpressionResult expressionResult;

            if (expressionsData.QueryExpression(Expression, out expressionResult))
            {
                int currentIntensity = expressionResult.intensity;

                // This debounce algorithm was based on the Arduino Debounce Tutorial (https://www.arduino.cc/en/Tutorial/Debounce)
                //Debug.Log(string.Format("CI:{0}, LI:{1}",currentIntensity, lastIntensity));
                bool currentState = currentIntensity >= ExpressionThreshold;
                bool lastState = lastIntensity >= ExpressionThreshold;

                if (currentState != lastState)
                {
                    lastDebounceTime = UnityEngine.Time.time;
                }

                if ((Time.time - lastDebounceTime) > debounceDelay)
                {
                    if (currentState != buttonState)
                    {
                        buttonState = currentState;

                        ButtonUp = !buttonState;
                        ButtonDown = buttonState;
                    }
                    else
                    {
                        ButtonHold = buttonState;
                    }
                }
                lastIntensity = currentIntensity;
            }
        }
    }

    public override string ToString()
    {
        return string.Format("{0} (T:{1}) - BUp:{2}, BDown:{3}, BHold:{2}", Expression, ExpressionThreshold, ButtonUp, ButtonDown, ButtonHold);
    }
}