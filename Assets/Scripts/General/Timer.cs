using UnityEngine;
using System.Collections;

public class Timer
{
	private float m_loopTime;
    private float m_startTime;
	
    public Timer()
	{
		m_loopTime = Time.time;
        m_startTime = Time.time;
	}
	
	public float LoopTime
	{
		get { return m_loopTime; }
	}
	
	public bool TimeLooped(float deltaTime)
	{
		float loopTime = Time.time - m_loopTime;
		//Debug.Log(string.Format("time loopTime: {0}, m_loopTime: {1}, time: {2}", loopTime, m_loopTime, Time.time));
		if(loopTime >= deltaTime)
		{
			m_loopTime = Time.time;
			return true;
		}
		
		return false;
	}

    public void Start()
    {
        m_startTime = Time.time;
    }

    public float ElapsedTime
    {
        get { return Time.time - m_startTime; }
    }

    public float StartTime
    {
        get { return m_startTime; }
        set { m_startTime = value; }
    }

    public bool Wait(float time)
    {
        if (ElapsedTime < time)
            return true;
        return false;
    }
}
