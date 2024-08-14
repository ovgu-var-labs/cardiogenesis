using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public int id;
    public string username;
    public int tutorial_completed;
    public int admin;

    public User(int id, string username, int tutorial_completed,int admin)
    {
        this.id = id;
        this.username = username;
        this.tutorial_completed = tutorial_completed;
        this.admin = admin;
    }
}
