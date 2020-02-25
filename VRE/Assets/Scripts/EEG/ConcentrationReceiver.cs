using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrationReceiver : MonoBehaviour
{
	OscSimpl.Examples.EegReceiver eegReceiver;
	public float concentrationIndex;
	public string state;
	// Start is called before the first frame update
	void Start()
    {
		state = "idle";
		eegReceiver = GetComponent<OscSimpl.Examples.EegReceiver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
