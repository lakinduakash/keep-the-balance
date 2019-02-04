
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass
{
    public static Vector3 getCenterOfMass(float m1,Vector3 c1, float m2, Vector3 c2)
    {

        return (m1*c1+m2*c2)/(m1+m2);
    }

    public static Vector3 getCenterOfMass(float[] m,Vector3[] c)
    {
        Vector3 pos =Vector3.zero;

        int loopCount = 0;
        float totalMass = 0;
        foreach(float i in m)
        {
            pos = pos+ i * c[loopCount];
            totalMass = totalMass + i;
            loopCount++;
        }

        return pos / totalMass;
    }

    public static Vector3 getCenterOfMass(Dictionary<float,Vector3> ob)
    {
        Vector3 pos = Vector3.zero;

        int loopCount = 0;
        float totalMass = 0;
        foreach (float i in ob.Keys)
        {
            pos = pos + i * ob[i];
            totalMass = totalMass + i;
            loopCount++;
        }

        return pos / totalMass;
    }
}
