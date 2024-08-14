using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Puk
{
    public GameObject pukGameobject;

    public void SetPukGameobject(GameObject gameObject)
    {
        pukGameobject = gameObject;
    }
}

[System.Serializable] 
public class ListOfPuks : List<GameObject>
{
    public List<GameObject> listOfPuks;

    public  void AddElement(GameObject gameObject)
    {
        listOfPuks.Add(gameObject);
    }
}