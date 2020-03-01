using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{

	
	OscSimpl.Examples.EegReceiver eegReceiver;
	// Start is called before the first frame update
	void Start()
    {
		eegReceiver = GetComponent<OscSimpl.Examples.EegReceiver>();
	}

    // Update is called once per frame
    void Update()
    {
    }
}
