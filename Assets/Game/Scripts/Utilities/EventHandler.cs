using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventHandler
{
    public static event Action OpenTheDoor;
    public static void CallOpenTheDoorEvent()
    {
        OpenTheDoor?.Invoke();
    }

    public static event Action GameRestart;
    public static void CallGameRestart()
    {
        GameRestart?.Invoke();
    }

    public static event Action GameResume;
    public static void CallGameResume()
    {
        GameResume?.Invoke();
    }

    public static event Action GameToMainMenu;
    public static void CallReturnToMainMenu()
    {
        GameToMainMenu?.Invoke();
    }

}
