using HotFix.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix.Event
{
    internal sealed class LoadingProgressEvent : HEvent
    {
        public float CurProgress { get; private set; }
        public LoadingProgressEvent Init(float progress) 
        {
            CurProgress = progress;
            EventType = HEventType.LoadingProgressEvent;
            return this;
        }
    }
}
