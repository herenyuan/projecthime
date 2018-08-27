using UnityEngine;
using System.Collections;

/// <summary>
/// FPS 收集器
/// </summary>
public class FPSCollection : Singleton<FPSCollection> {

	//public UILabel fps;
	public float FPSBase = 30;
	public float FPSCurrent = 30;
	float SpecPercent = 0;
	public void LateUpdate()
	{
		if(SlowDuration > 0)
		{
			SlowDuration -= FPSCollection.Ins.NormalizedDeltaTime;
			if(SlowDuration <= 0)
			{
				SlowDuration = 0;
				Time.timeScale = 1;
			}
		}
		else
		{
//			FPSBalance();
		}


	}
	void FPSBalance()
	{
		FPSCurrent = 1/FPSCollection.Ins.NormalizedDeltaTime;
		//fps.text = FPSCurrent.ToString();
	}
	float SlowDuration = 0;
	public void Slow(float Duration)
	{
		SlowDuration = Duration;
		Time.timeScale = 0.1f;
	}
	public float NormalizedDeltaTime
	{
		get
		{
			float deltaTime = Time.deltaTime;
			if(deltaTime<=Time.maximumDeltaTime){return deltaTime;}
			else{return Time.maximumDeltaTime;}
		}
	}
}
