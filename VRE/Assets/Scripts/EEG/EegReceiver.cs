using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OscSimpl.Examples
{
	public class EegReceiver : MonoBehaviour
	{
		[SerializeField] OscIn _oscIn;

		public Vector3 gyroscope;
		public string connectivity;
		public float deltaAbsolute;
		public float thetaAbsolute;
		public float alphaAbsolute;
		public float betaAbsolute;


		//const string address1 = "/test1";
		//const string address2 = "/test2";



		void Start()
		{
			gyroscope = new Vector3(0, 0, 0);
			connectivity = "";
			deltaAbsolute = 0;;
			thetaAbsolute = 0;;
			alphaAbsolute = 0; ;
			betaAbsolute = 0;
			// Ensure that we have a OscIn component and start receiving on port 7000.
			if (!_oscIn) _oscIn = gameObject.AddComponent<OscIn>();
			_oscIn.Open(7000);
		}


		void OnEnable()
		{
			// You can "map" messages to methods in two ways:

			// 1) For messages with a single argument, route the value using the type specific map methods.
			//_oscIn.MapFloat( address1, OnTest1 );

			// 2) For messages with multiple arguments, route the message using the Map method.
			//_oscIn.Map( address2, OnTest2 );
			_oscIn.Map("/muse/gyro", OnGyro);
			_oscIn.Map("/muse/elements/horseshoe", OnConnectivity);
			_oscIn.MapFloat("/muse/elements/delta_absolute", OnDelta);
			_oscIn.MapFloat("/muse/elements/theta_absolute", OnTheta);
			_oscIn.MapFloat("/muse/elements/alpha_absolute", OnAlpha);
			_oscIn.MapFloat("/muse/elements/beta_absolute", OnBeta);
		}


		void OnDisable()
		{
			// If you want to stop receiving messages you have to "unmap".
			//_oscIn.UnmapFloat( OnTest1 );
			//_oscIn.Unmap( OnTest2 );
			_oscIn.UnmapFloat( OnDelta );
			_oscIn.Unmap(OnGyro);
		}

		void OnGyro(OscMessage message)
		{
			// Get arguments from index 0, 1 and 2 safely.
			float x;
			float y;
			float z;
			if (
				message.TryGet(0, out x) &&
				message.TryGet(1, out y) &&
				message.TryGet(2, out z)
			)
			{
				//Debug.Log("Received Gyro\n" + x + " " + y + " " + z + "\n");
				gyroscope = new Vector3(x, y, z);
			}


			// Always recycle incoming messages when used.
			OscPool.Recycle(message);
		}

		void OnConnectivity(OscMessage message)
		{
			// Get arguments from index 0, 1 and 2 safely.
			float tp9;
			float af7;
			float af8;
			float tp10;
			if (
				message.TryGet(0, out tp9) &&
				message.TryGet(1, out af7) &&
				message.TryGet(2, out af8) &&
				message.TryGet(3, out tp10)
			)
			{
				//Debug.Log("Received Gyro\n" + x + " " + y + " " + z + "\n");
				connectivity = connectivityResult(tp9, af7, af8, tp10);
			}


			// Always recycle incoming messages when used.
			OscPool.Recycle(message);
		}

		void OnDelta(float value)
		{
			if(!connectivity.Equals("bad"))
				deltaAbsolute = value;
		}
		void OnTheta(float value)
		{ 
			if(!connectivity.Equals("bad"))
				thetaAbsolute = value;
		}
		void OnAlpha(float value)
		{
			if(!connectivity.Equals("bad"))
				alphaAbsolute = value;
		}
		void OnBeta(float value)
		{
			if(!connectivity.Equals("bad"))
				betaAbsolute = value;
		}

		string connectivityResult(float tp9, float af7, float af8, float tp10)
		{
			int goodCount = 0;
			if (tp9 > 2) return "bad";
			else if (tp9 == 1) goodCount++;
			if (af7 > 2) return "bad";
			else if (af7 == 1) goodCount++;
			if (af8 > 2) return "bad";
			else if (af8 == 1) goodCount++;
			if (tp10 > 2) return "bad";
			else if (tp10 == 1) goodCount++;

			if (goodCount == 4) return "good";
			else return "medium";

		}
	}
}