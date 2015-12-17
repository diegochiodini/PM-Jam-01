using System;
using System.Collections.Generic;

public abstract class Singleton<T> where T:class, new()
{
    private static T _instance;

    public T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }

        private set { }
    }
}