using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTimer : MonoBehaviour
{

    // Values
    
    private string _timerName;

    [SerializeField] private float _duration = 5f; // timer duration
    [SerializeField] private int _level = 1; // timer level - timer level in timers stack
    private float _timer = 0; // time since creation

    private const float levelGap = 0.7f; // do in settings maybe

    private float maskWidth;
    private float imageWidth;

    // Public members

    public string TimerName
    {
        get => _timerName;
        set => _timerName = value;
    }
    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }
    public int Level
    {
        get => _level;
        set => _level = value;
    }

    // Objects

    [SerializeField] private UnityEngine.UI.Image mask;
    [SerializeField] private RectTransform maskRectTransform;
    [SerializeField] private RectTransform imageRectTransform;
    
    private Transform body;

    void Start()
    {
        body = transform.parent.parent; // player body
        setSize();
    }

    void FixedUpdate()
    {
        if(_duration - _timer <= 0)
        {
            SendMessageUpwards("DestroyTimer", this);
        }

        mask.fillAmount = (_duration-_timer) / _duration;
        if(!GameManager.Instance.isFrozen()) _timer += Time.fixedDeltaTime;
    }

    // Set timer size
    public void setSize()
    {
        float sizeCorrection = (1 / body.localScale.x);
        float width = Settings.Instance.initialTimerWidth * sizeCorrection;
        float offset = Settings.Instance.initialTimerOffset * sizeCorrection;
        float gap = (levelGap * (_level - 1)) * sizeCorrection;

        imageWidth = 1 + (width * _level) + offset + gap;
        maskWidth = imageWidth + 1 + (width * (_level - 1)) + gap;

        imageRectTransform.sizeDelta = new Vector2(imageWidth, imageWidth);
        maskRectTransform.sizeDelta = new Vector2(maskWidth, maskWidth);
    }

    public void resetTimer()
    {
        _timer = 0;
    }

    public void lowerLevel()
    {
        _level -= 1;
        setSize();
    }

    public void deleteTimer()
    {
        Destroy(gameObject);
    }
}
