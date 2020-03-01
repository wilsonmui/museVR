using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrationController : MonoBehaviour
{
	OscSimpl.Examples.EegReceiver eegReceiver;

	public string currentState;
	const string STATE_IDLE = "idle";
	const string STATE_RELAX = "relax";
	const string STATE_CLEAN_FOG = "clean fog";
	const string STATE_MUTE_BIRD = "mute bird";
    // Start is called before the first frame update
    void Start()
    {
		eegReceiver = GetComponent<OscSimpl.Examples.EegReceiver>();

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void NextState()
	{
		if (currentState.Equals(STATE_RELAX))
		{
			currentState = STATE_CLEAN_FOG;
		}
		else if (currentState.Equals(STATE_CLEAN_FOG))
		{
			currentState = STATE_MUTE_BIRD;
		}
	}

}
