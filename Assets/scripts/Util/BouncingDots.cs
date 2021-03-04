using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingDots : MonoBehaviour
{
  
	//the total time of the animation
	public float RepeatTime = 1;

	//the time for a dot to bounce up and come back down
	public float BounceTime = 0.25f;

	//how far does each dot move
	public float BounceHeight = 10;

	public List<GameObject> Dots;

	private float delay;
	private Coroutine animationRoutine = null;
	private Vector3[] startLocalPositions;
	
	void Awake()
	{
		startLocalPositions = new Vector3[Dots.Count];
		for (int i = 0; i < Dots.Count; i++)
		{
			startLocalPositions[i] = Dots[i].transform.localPosition;
		}
	}
	
	void OnEnable(){
		if (RepeatTime < Dots.Count * BounceTime){
			RepeatTime = Dots.Count * BounceTime;
		}
		delay = RepeatTime - Dots.Count * BounceTime;
		animationRoutine = StartCoroutine(Animate());
		// InvokeRepeating("Animate", 0, repeatTime);
	}

	void OnDisable()
	{
		if(animationRoutine != null)
			StopCoroutine(animationRoutine);
	}

	private IEnumerator Animate()
	{
		Vector3[] startPositions = new Vector3[Dots.Count];
		Vector3[] endPositions = new Vector3[Dots.Count];
		bool[] onComplete = new bool[Dots.Count];
		float[] elapsedTime = new float[Dots.Count];
		float totalTime = 0;
		bool[] AnimationDone = new bool[Dots.Count];
		bool WaitForRepeatTime = false;
		Reset();
		
		void Reset(){
			WaitForRepeatTime = false;
			for (int i = 0; i < Dots.Count; i++)
			{
				Dots[i].transform.localPosition = startLocalPositions[i];
				totalTime = 0;
				elapsedTime[i] = 0;//- (BounceTime)*2 * i ;
				startPositions[i] = Dots[i].transform.localPosition;
				endPositions[i] = startPositions[i] + new Vector3(0,BounceHeight,0);
				onComplete[i] = AnimationDone[i] = false;
			}
		}
		
		while (true)
		{
			totalTime += Time.deltaTime;
			for (int i = 0; i < Dots.Count; i++)
			{
				if(AnimationDone[i] || totalTime < (BounceTime) * i) 
				{
					continue;
				}
				if (!onComplete[i])
				{
					elapsedTime[i] += Time.deltaTime;
				}
				else
				{
					elapsedTime[i] -= Time.deltaTime;
				}
				
				var step = elapsedTime[i] / BounceTime/2;
				if (step >= 1)
				{
					onComplete[i] = true;
					//elapsedTime[i] = BounceTime;
				}
				Dots[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], step);
				if(step <= 0)
				{
					onComplete[i] = !onComplete[i];
					elapsedTime[i] = 0;
					AnimationDone[i] = true;
				}
			}

			foreach (var dotAnimDone in AnimationDone)
			{
				WaitForRepeatTime = true;
				if (!dotAnimDone)
				{
					WaitForRepeatTime = false;
					break;
				}
			}

			if (WaitForRepeatTime)
			{
				Reset();
				yield return new WaitForSeconds(delay);
			}

			yield return null;
		}
	}
}
