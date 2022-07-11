using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{

    public GameObject timerPrefab;
    private PlayerController playerController;

    private List<CircleTimer> activeTimers = new List<CircleTimer>();

    void FixedUpdate()
    {
        updateSizes(); // Todo : update sizes only when body size changes
    }

    public void addTimer(float duration)
    {
        GameObject timer = Instantiate(timerPrefab, transform) as GameObject; // create timer
        CircleTimer circleTimer = timer.GetComponent<CircleTimer>();

        circleTimer.Level = transform.childCount + 1; // set timer level
        circleTimer.Duration = duration; // set timer duration

        activeTimers.Add(circleTimer); // add to active timers list
    }

    public void DestroyTimer(CircleTimer destroyedTimer) // gets called when a timer ends
    {
        activeTimers.Remove(destroyedTimer);

        // lower level of each timer in a higher level
        foreach (Transform child in transform)
        {
            CircleTimer timer = child.GetComponent<CircleTimer>();
            if(timer.Level > destroyedTimer.Level)
            {
                timer.lowerLevel();
            }
        }

        destroyedTimer.deleteTimer();
    }

    // Deletes all active timers
    public void deleteTimers()
    {
        foreach (CircleTimer timer in activeTimers)
        {
            timer.deleteTimer();
        }
        activeTimers.Clear();
    }

    private void updateSizes()
    {
        foreach (CircleTimer timer in activeTimers)
        {
            timer.setSize();
        }
    }
}
