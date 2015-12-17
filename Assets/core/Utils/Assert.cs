using UnityEngine;
using System.Collections;

public class Assert
{
	public static void That(bool condition, string message)
	{
#if DEBUG
		if (!condition)
		{
			Debug.LogError(message);
		}
#endif
	}

    public static void NotNullSafe(System.Object aObj, string message)
    {
#if DEBUG
        if (aObj == null || aObj.Equals(null))
        {
            Debug.LogError(message);
        }
#endif
    }

    [System.Obsolete("This is an obsolete method, change it with NotNullSafe")]
	public static void NotNull(object obj, string message)
	{
		#if DEBUG
		if (obj == null)
		{
			Debug.LogError(message);
		}
		#endif
	}

    public static void PositiveLength(object[] obj, string message)
    {
#if DEBUG
        if (obj.Length <= 0)
        {
            Debug.LogError(message);
        }
#endif
    }

    public static void Conditional(bool trigger, bool condition, string message)
    {
#if DEBUG
        if (trigger)
        {
            if (!condition)
            {
                Debug.LogError(message);
            }
        }
#endif
    }
}
