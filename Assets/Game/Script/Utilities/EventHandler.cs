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

    public static event Action GameWin;

    public static void CallGameWinEvent()
    {
        GameWin?.Invoke();
    }

}
