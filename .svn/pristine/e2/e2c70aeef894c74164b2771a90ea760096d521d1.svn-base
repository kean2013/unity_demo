using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FairyGUI;
using HotFix.Enum;

namespace HotFix.Event
{
    public class HEvent
    {
        public HEventType EventType { get; protected set; }
        public void Send()
        {
            Event.EventMgr.Send(this);
        }
        public void Send(HEventType evtType)
        {
            Event.EventMgr.Send(evtType);
        }
    }
}
