using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrialScript : MonoBehaviour
{
	private bool lineEnter;

    void Start()
    {
		lineEnter = false;
	}

	// start timer when trigger enter
	// reset timer when retrigger a second time
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			if (!lineEnter)
			{
				FindObjectOfType<TimerScript>().trialStart = true;
				lineEnter = true;
			}
			else if (lineEnter)
			{
				FindObjectOfType<TimerScript>().trialStart = false;
				FindObjectOfType<TimerScript>().timerText.text = "";
				FindObjectOfType<TimerScript>().timer = 0f;
				lineEnter = false;
			}
		}
	}
}
