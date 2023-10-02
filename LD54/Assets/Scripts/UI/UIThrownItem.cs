using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIThrownItem : MonoBehaviour
{
    [SerializeField]
    private Image imgIcon;
    private Vector2 targetPosition;
    private Vector2 startPosition;
    private Vector2 currentPosition;
    public void Initialize(Sprite sprite, Vector2 start, Vector2 target)
    {
        startPosition = start;
        currentPosition = start;
        targetPosition = target;
        transform.position = start;
        isAnimating = true;
        timer = 0f;
        imgIcon.sprite = sprite;
    }


    float timer = 0f;
    [SerializeField]
    float duration = 0.2f;

    bool isAnimating = false;

    void Update()
    {
        if (isAnimating)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, targetPosition, timer / duration);
            if (timer >= duration)
            {
                transform.position = targetPosition;
                Kill();
            }
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
