using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FairyGUI;
using HotFix.Enum;

namespace HotFix.Event
{
    internal sealed class NetConnectEvent : HEvent
    {
        public int State { get; private set; }
        public NetConnectEvent Set(int state)
        {
            State = state;
            EventType = Enum.HEventType.NetConnectEvent;
            return this;
        }
    }
}
