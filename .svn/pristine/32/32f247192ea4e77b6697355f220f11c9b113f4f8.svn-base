using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix.Event
{
    using Enum;
    using UnityEngine;

    internal sealed class EmptyEvent : HEvent
    {
        public EmptyEvent Set(HEventType evt)
        {
            EventType = evt;
            return this;
        }
    }

    internal class IntEvent : HEvent
    {
        public int Value { get; private set; }
        public IntEvent Set(HEventType et, int val)
        {
            Value = val;
            EventType = et;
            return this;
        }
    }

    internal class IntArrayEvent : HEvent
    {
        public int[] Value { get; private set; }
        public IntArrayEvent Set(HEventType et, params int[] val)
        {
            Value = val;
            EventType = et;
            return this;
        }
    }

    internal class FloatEvent : HEvent
    {
        public float Value { get; private set; }

        public FloatEvent Set(HEventType et, float val)
        {
            Value = val;
            EventType = et;
            return this;
        }
    }
    internal class FloatArrayEvent : HEvent
    {
        public float[] Value { get; private set; }

        public FloatArrayEvent Set(HEventType et, params float[] val) 
        {
            Value = val;
            EventType = et;
            return this;
        }
    }

    internal class StringEvent : HEvent
    {
        public string Value { get; private set; }

        public StringEvent Set(HEventType et, string val)
        {
            Value = val;
            EventType = et;
            return this;
        }
    }
    internal class StringArrayEvent : HEvent
    {
        public string[] Value { get; private set; }


        public StringArrayEvent Set(HEventType et, params string[] val) 
        {
            Value = val;
            EventType = et;
            return this;
        }
    }

    internal class ObjectEvent : HEvent
    {
        public object Value { get; private set; }

        public ObjectEvent Set(HEventType et, object val)
        {
            Value = val;
            EventType = et;
            return this;
        }
    }

    internal class ObjectArrayEvent : HEvent
    {
        public object[] Value { get; private set; }

        public ObjectArrayEvent Set(HEventType et, params object[] val)
        {
            Value = val;
            EventType = et;
            return this;
        }
    }
    internal class BoolEvent : HEvent
    {
        public bool Value { get; private set; }

        public BoolEvent Set(HEventType et, bool val)
        {
            Value = val;
            EventType = et;
            return this;
        }
    }

    internal class Vector2Event : HEvent
    {
        public Vector2 Value { get; private set; }
        public Vector2Event Set(HEventType et, Vector2 val) 
        {
            Value = val;
            EventType = et;
            return this;
        }
    }

    internal class LongEvent : HEvent
    {
        public long Value { get; private set; }
        public LongEvent Set(HEventType et, long val) 
        {
            Value = val;
            EventType = et;
            return this;
        }
    }
}
