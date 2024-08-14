using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage 
{
    public int id;
    public string stageName;
    public string description;

    public Stage(int id,string stageName, string description)
    {
        this.id = id;
        this.stageName = stageName;
        this.description = description;
    }
    
}
