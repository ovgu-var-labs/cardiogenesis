using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NewHelper
{
    // Start is called before the first frame update
    
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
