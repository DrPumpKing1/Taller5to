using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomsWeakPointsHandler : MonoBehaviour
{
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsEnable;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsDisable;
    public class OnWeakPointsEventArgs : EventArgs
    {
        public List<ShowcaseRoomWeakpoint> weakPoints;
    }
}
