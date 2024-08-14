using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHighestTask
{
    public static int highestTaskID;

    public void SetHighestTaskID(int id_)
    {
        highestTaskID = id_;
    }

    public int GetHighestTaskID()
    {
        return highestTaskID;
    }
}
