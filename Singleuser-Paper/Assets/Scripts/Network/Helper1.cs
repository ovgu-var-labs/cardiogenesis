using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper1
{
    public static GameObject FindComponentInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Flattens the vector onto the xz-plane
    /// </summary>
    /// <param name="inVector"></param>
    /// <returns>Vector with y-component = zero</returns>
    public static Vector3 MakeVectorFlat(Vector3 inVector)
    {
        return new Vector3(inVector.x, 0, inVector.z);
    }
}
