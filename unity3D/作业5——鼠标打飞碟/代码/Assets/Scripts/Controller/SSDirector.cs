using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 当前场景的director，单例模式
public class SSDirector : System.Object
{
    private static SSDirector _instance;
    public ISceneController currentSceneController { get; set; }
    public static SSDirector GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }
}