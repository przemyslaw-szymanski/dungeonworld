using UnityEngine;
using System.Collections;

public class TSingleton<T> where T : class, new()
{
    protected static T m_Singleton;

    public static T Singleton
    {
        get
        {
            if (m_Singleton == null)
                m_Singleton = new T();

            return m_Singleton;
        }
    }
}
