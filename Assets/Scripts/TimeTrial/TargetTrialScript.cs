using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTrialScript : MonoBehaviour
{
	private bool lineEnter;
	public int targetKill;
	public Text TargetCounterText1;
	public Text TargetCounterText2;

	void Start()
	{
		lineEnter = false;
		targetKill = 0;
		TargetCounterText1.text = "";
		TargetCounterText2.text = "";
	}

	// target counter hud
	private void Update()
	{
		if (targetKill == 20)
			FindObjectOfType<TimerScript>().trialStart = false;

		if (lineEnter)
			TargetCounterText2.text = targetKill.ToString() + " / 20";
	}

	// start time trial (target practice)
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && targetKill < 20)
		{
			if (!lineEnter)
			{
				FindObjectOfType<TimerScript>().trialStart = true;
				lineEnter = true;

				TargetCounterText1.text = "TARGET COUNTER";
			}
		}
	}
}
