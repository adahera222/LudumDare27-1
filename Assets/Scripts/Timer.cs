using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	
	public enum TimerType {
		Stopwatch,
		Countdown,
	}
	
	public delegate void Callback();
	
	public TimerType Type { get; private set; }
	public float CountdownTime { get; private set; }
	public float CurTime { get; private set; }
	public bool Started { get; private set; }
	public bool Paused { get; private set; }
	Callback callback;
	
	void FixedUpdate() {
		if (Started && !Paused) {
			if (Type == TimerType.Stopwatch) {
				CurTime += Time.fixedDeltaTime;
			}
			else if (Type == TimerType.Countdown) {
				CurTime -= Time.fixedDeltaTime;
				if (CurTime <= 0f) {
					Stop();
					if (callback != null) {
						callback();
					}
				}
			}
		}
	}
	
	public void SetCountdown(float time) {
		Type = TimerType.Countdown;
		CountdownTime = time;
	}
	
	public void SetCountdown(float time, Callback _callback) {
		Type = TimerType.Countdown;
		CountdownTime = time;
		callback = _callback;
	}
	
	public void SetStopwatch() {
		Type = TimerType.Stopwatch;
	}
	
	public void Start() {
		if (Type == TimerType.Stopwatch) {
			CurTime = 0f;
		}
		else if (Type == TimerType.Countdown) {
			CurTime = CountdownTime;
		}
		Started = true;
		Paused = false;
	}
	
	public void Pause() {
		Paused = true;
	}
	
	public void Resume() {
		Paused = false;
	}
	
	public void Stop() {
		CurTime = 0f;
		Started = false;
		Paused = false;
	}
	
	public void Reset() {
		Type = TimerType.Stopwatch;
		CountdownTime = 0f;
		CurTime = 0f;
		Started = false;
		Paused = false;
		callback = null;
	}
	
	public void SetCallback(Callback _callback) {
		callback = _callback;
	}
	
}
