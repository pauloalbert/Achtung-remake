using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTimer : MonoBehaviour
{

    private float duration = 5f;
    private float timer = 0f;

    private float timerOffset = 1f; // offset from player body
    private float timerWidth = 0.5f; // between 0 and 1

    public UnityEngine.UI.Image mask;
    public RectTransform maskRectTransform;
    public RectTransform imageRectTransform;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(duration - timer <= 0)
        {
            Destroy(gameObject);
        }

        maskRectTransform.localScale = new Vector3(1 + timerOffset, 1 + timerOffset, 0);
        imageRectTransform.localScale = new Vector3(timerWidth/2 + 0.5f, timerWidth/2 + 0.5f, 0);

        mask.fillAmount = (duration-timer) / duration;
        timer += Time.fixedDeltaTime;
    }
}
