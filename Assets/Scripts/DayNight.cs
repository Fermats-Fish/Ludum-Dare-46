using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    SpriteRenderer dayNightOverlay;
    Color Night;
    public float transitionTime = 10;

    private void Start()
    {
        dayNightOverlay = GetComponent<SpriteRenderer>();
        Night = dayNightOverlay.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * Camera.main.orthographicSize*100;
        
        if (GameController.instance.timeOfDay > 0.2f) {
            if (GameController.instance.timeOfDay < 0.75f)
            {
                dayNightOverlay.color = new Color(Night.r, Night.g, Night.b, Mathf.Max(dayNightOverlay.color.a - Time.deltaTime/transitionTime,0));
            }
            else {
                dayNightOverlay.color = new Color(Night.r, Night.g, Night.b, Mathf.Min(dayNightOverlay.color.a + Time.deltaTime/transitionTime, Night.a));
            }
        }
       
    }
}
