using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OscSimpl.Examples
{
	public class EegReceiver : MonoBehaviour
	{
		[SerializeField] OscIn _oscIn;
		
		public Vector3 gyroscope;

		public string currentState;
		const string STATE_IDLE = "idle";
		const string STATE_RELAX = "relax";
		const string STATE_FOG = "fog";
		const string STATE_BIRD = "bird";

		public string connectivity;
		const string CONNECTIVITY_IDLE = "idle";
		const string CONNECTIVITY_GOOD = "good";
		const string CONNECTIVITY_MIDIUM = "midium";
		const string CONNECTIVITY_BAD = "bad";

		bool writeAccess;
		public float[] deltaAbsolute;
		public float[] thetaAbsolute;
		public float[] alphaAbsolute;
		public float[] betaAbsolute;
		public float[] gammaAbsolute;

		public float currentConcentrationIndex; // current concentration index, update from osc
		public float[] variousConcentrationIndex; // shows different concentration index algorithm result
		public float concentrationIndexThreshold; // adopted threshold
		public float maxConcentrationIndexThreshold; // relaxing concentraion index * 1.3f
		public float minConcentrationIndexThreshold; // relaxing concentraion index

		const float MAX_DIFFICULTY = 100;
		public float difficulty; // for tuning proper threshold 
		int difficultCounter; //
		const int MAX_DIFFICULT_TIME = 10; // stuck in difficult after MAX_DIFFICULT_TIME will reduce difficulty

		float inSecondConcentrationIndexSum; // concentration index sum in this second
		public List<float> concentrationIndexList; // record of concentration index of each second

		const float THRESHOLD_SCALE = 1.3f;
		const float RELAXATION_WINDOW_LENGTH = 30; // length of relaxation window time
		public float timer; // count down for a second
		int frameCounter; // how many frames in relaxation window time, for averaging concentration index

		int concentratingCounter; // how many concentrating seconds
		SceneController sceneController;
		void Start()
		{
			// initial values
			gyroscope = new Vector3(0, 0, 0);
			currentState = STATE_IDLE;
			connectivity = CONNECTIVITY_IDLE;

			deltaAbsolute = new float[4];
			thetaAbsolute = new float[4];
			alphaAbsolute = new float[4];
			betaAbsolute = new float[4];
			gammaAbsolute = new float[4];

			currentConcentrationIndex = 0;
			variousConcentrationIndex = new float[4];
			concentrationIndexThreshold = 0;
			maxConcentrationIndexThreshold = 0;
			minConcentrationIndexThreshold = 0;
			difficulty = MAX_DIFFICULTY;
			inSecondConcentrationIndexSum = 0;

			timer = 0;
			frameCounter = 0;
			concentrationIndexList = new List<float>();
			writeAccess = true;

			concentratingCounter = (int)RELAXATION_WINDOW_LENGTH;
			sceneController = GetComponent<SceneController>();
			// Ensure that we have a OscIn component and start receiving on port 7000.
			if (!_oscIn) _oscIn = gameObject.AddComponent<OscIn>();
			_oscIn.Open(7000);
		}

		private void Update()
		{
			if(!connectivity.Equals(CONNECTIVITY_IDLE) &&  !connectivity.Equals(CONNECTIVITY_BAD))
			{

				if (currentState.Equals(STATE_IDLE))
				{
					NextState();
				}

				SetConcentrationIndexList();
			}
			if (concentratingCounter < concentrationIndexList.Count)
			{
				if(concentrationIndexList[concentratingCounter] >= concentrationIndexThreshold)
				{
					ControlScene();
					difficultCounter = 0;
				}
				else
				{
					difficultCounter++;
					if(difficultCounter == MAX_DIFFICULT_TIME)
					{
						difficultCounter = 0;
						ReduceDifficulty();
					}
				}
				concentratingCounter++;
			}
			
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
			_oscIn.Map("/muse/elements/delta_absolute", OnDelta);
			_oscIn.Map("/muse/elements/theta_absolute", OnTheta);
			_oscIn.Map("/muse/elements/alpha_absolute", OnAlpha);
			_oscIn.Map("/muse/elements/beta_absolute", OnBeta);
			_oscIn.Map("/muse/elements/gamma_absolute", OnGamma);
		}


		void OnDisable()
		{
			// If you want to stop receiving messages you have to "unmap".
			//_oscIn.UnmapFloat( OnTest1 );
			//_oscIn.Unmap( OnTest2 );
			_oscIn.Unmap(OnGyro);
			_oscIn.Unmap(OnConnectivity);
			_oscIn.Unmap(OnDelta);
			_oscIn.Unmap(OnTheta);
			_oscIn.Unmap(OnAlpha);
			_oscIn.Unmap(OnBeta);
			_oscIn.Unmap(OnGamma);
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
				connectivity = ConnectivityResult(tp9, af7, af8, tp10);
			}


			// Always recycle incoming messages when used.
			OscPool.Recycle(message);
		}
		void OnDelta(OscMessage message)
		{
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
				if (!connectivity.Equals(CONNECTIVITY_BAD))
				{
					deltaAbsolute[0] = tp9;
					deltaAbsolute[1] = af7;
					deltaAbsolute[2] = af8;
					deltaAbsolute[3] = tp10;
				}
			}
			// Always recycle incoming messages when used.
			OscPool.Recycle(message);

		}
		void OnTheta(OscMessage message)
		{
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
				if (!connectivity.Equals(CONNECTIVITY_BAD))
				{
					thetaAbsolute[0] = tp9;
					thetaAbsolute[1] = af7;
					thetaAbsolute[2] = af8;
					thetaAbsolute[3] = tp10;
					SetInSecondConcentrationIndex();
					
				}
			}
			// Always recycle incoming messages when used.
			OscPool.Recycle(message);

		}
		void OnAlpha(OscMessage message)
		{
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
				if (!connectivity.Equals(CONNECTIVITY_BAD))
				{
					alphaAbsolute[0] = tp9;
					alphaAbsolute[1] = af7;
					alphaAbsolute[2] = af8;
					alphaAbsolute[3] = tp10;
				}
			}
			// Always recycle incoming messages when used.
			OscPool.Recycle(message);

		}
		void OnBeta(OscMessage message)
		{
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
				if (!connectivity.Equals(CONNECTIVITY_BAD))
				{
					betaAbsolute[0] = tp9;
					betaAbsolute[1] = af7;
					betaAbsolute[2] = af8;
					betaAbsolute[3] = tp10;
				}
			}
			// Always recycle incoming messages when used.
			OscPool.Recycle(message);

		}
		void OnGamma(OscMessage message)
		{
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
				if (!connectivity.Equals(CONNECTIVITY_BAD))
				{
					gammaAbsolute[0] = tp9;
					gammaAbsolute[1] = af7;
					gammaAbsolute[2] = af8;
					gammaAbsolute[3] = tp10;
				}
			}
			// Always recycle incoming messages when used.
			OscPool.Recycle(message);

		}
		string ConnectivityResult(float tp9, float af7, float af8, float tp10)
		{
			int goodCount = 0;
			if (tp9 == 1) { goodCount++; }
			if (af7 > 2) { return CONNECTIVITY_BAD; }
			else if (af7 == 1) { goodCount++; }
			if (af8 > 2) { return CONNECTIVITY_BAD; }
			else if (af8 == 1) { goodCount++; }
			if (tp10 == 1) { goodCount++; }

			if (goodCount == 4) { return CONNECTIVITY_GOOD; }
			else { return CONNECTIVITY_MIDIUM; }
		}

		void SetInSecondConcentrationIndex()
		{
			//currentConcentrationIndex = (betaAbsolute[1] + betaAbsolute[2]) / (thetaAbsolute[1] + thetaAbsolute[2]);
			variousConcentrationIndex[0] = Mathf.Pow(10, betaAbsolute[1] + betaAbsolute[2]) / Mathf.Pow(10, thetaAbsolute[1] + thetaAbsolute[2]);
			variousConcentrationIndex[1] = (betaAbsolute[1] + betaAbsolute[2]) / (thetaAbsolute[1] + thetaAbsolute[2]);
			variousConcentrationIndex[2] = betaAbsolute[1] + betaAbsolute[2] - thetaAbsolute[1] + thetaAbsolute[2];
			variousConcentrationIndex[3] = thetaAbsolute[1] + thetaAbsolute[2];
			currentConcentrationIndex = variousConcentrationIndex[3];
			if (writeAccess == true)
			{
				inSecondConcentrationIndexSum += currentConcentrationIndex;
				frameCounter++;
				//float relativeIndex = Mathf.Pow(10, b) / Mathf.Pow(10, t);
			}
		}

		void SetConcentrationIndexThreshold()
		{
			float concentrationSum = 0;
			for(int i = 0; i < RELAXATION_WINDOW_LENGTH; i++)
			{
				concentrationSum += concentrationIndexList[i];
			}
			maxConcentrationIndexThreshold = concentrationSum / RELAXATION_WINDOW_LENGTH * THRESHOLD_SCALE;
			minConcentrationIndexThreshold = concentrationSum / RELAXATION_WINDOW_LENGTH;
			concentrationIndexThreshold = maxConcentrationIndexThreshold;
		}

		void SetConcentrationIndexList()
		{
			timer += Time.deltaTime;

			if ((int)timer >= concentrationIndexList.Count && frameCounter != 0)
			{
				writeAccess = false;
				concentrationIndexList.Add(inSecondConcentrationIndexSum / frameCounter);
				frameCounter = 0;
				inSecondConcentrationIndexSum = 0;


				if (concentrationIndexList.Count == RELAXATION_WINDOW_LENGTH)
				{
					Debug.Log("Get Threshold");
					SetConcentrationIndexThreshold();
					if(currentState.Equals(STATE_RELAX))
					{
						sceneController.StopIntroMusic();
						NextState();
					}
					else
					{
						Debug.LogError("State Error SetConcentrationIndexList");
					}
				}
				writeAccess = true;

			}
		}

		void NextState()
		{
			switch(currentState)
			{
				case STATE_IDLE:
					currentState = STATE_RELAX;
					break;
				case STATE_RELAX:
					currentState = STATE_FOG;
					break;
				case STATE_FOG:
					currentState = STATE_BIRD;
					break;
				case STATE_BIRD:
					currentState = STATE_BIRD;
					break;
			}
		}

		void ControlScene()
		{
			if(currentState.Equals(STATE_FOG))
			{
				if(sceneController.ClearFog() == true)
				{
					NextState();
				}
			}
			else if(currentState.Equals(STATE_BIRD))
			{
				if(sceneController.MuteBirdTweet() == true)
				{
					NextState();
				}
			}
			else
			{
				Debug.LogError("State Error ControlScene");
			}
		}

		void ReduceDifficulty()
		{
			difficulty *= .9f;
			concentrationIndexThreshold = difficulty * maxConcentrationIndexThreshold + (1 - difficulty) * minConcentrationIndexThreshold;
		}
	}
}