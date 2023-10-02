using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIGoldDisplay : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI txtValue;
    [SerializeField]
    private Transform textContainer;
    private Vector2 textOriginalSize;
    private Vector2 textStartSize;
    private Vector2 textTargetSize;
    private Vector2 textCurrentSize;
    private bool animationHalfwayPassed = false;
    private int currentValue;
    private int startValue;
    private int targetValue;

    private bool isAnimating = false;

    [SerializeField]
    private float textAnimFactor = 2f;

    [SerializeField]
    private float duration = 0.5f;

    private int previousGold = 0;
    private int soundEveryGold = 5;

    private float timer = 0f;

    void Start()
    {
        textOriginalSize = textContainer.transform.localScale;
        txtValue.text = $"{currentValue}";
    }

    public void UpdateValue(int change)
    {
        timer = 0f;
        textStartSize = textContainer.localScale;
        textCurrentSize = textStartSize;
        textTargetSize = textOriginalSize * textAnimFactor;
        startValue = currentValue;
        targetValue = currentValue + change;
        isAnimating = true;
        animationHalfwayPassed = false;
        previousGold = currentValue;
        SoundManager.main.PlaySound(GameSoundType.GainGold);
    }

    void Update()
    {
        if (isAnimating)
        {
            timer += Time.unscaledDeltaTime;
            if (Mathf.Abs(currentValue - previousGold) > soundEveryGold)
            {
                SoundManager.main.PlaySound(GameSoundType.GainGold);
                previousGold = currentValue;
            }
            currentValue = (int)Mathf.Lerp(startValue, targetValue, timer / duration);
            if (animationHalfwayPassed)
            {
                textCurrentSize = Vector2.Lerp(textStartSize, textTargetSize, timer / duration);
            }
            else
            {
                textCurrentSize = Vector2.Lerp(textStartSize, textTargetSize, timer / (duration / 2));
            }
            if (timer > duration / 2 && !animationHalfwayPassed)
            {
                animationHalfwayPassed = true;
                textStartSize = textCurrentSize;
                textTargetSize = textOriginalSize;
            }
            if (timer > duration)
            {
                animationHalfwayPassed = false;
                currentValue = targetValue;
                textCurrentSize = textTargetSize;
                isAnimating = false;
                timer = 0f;
            }
            textContainer.transform.localScale = textCurrentSize;
            txtValue.text = $"{currentValue}";
        }
    }
}
