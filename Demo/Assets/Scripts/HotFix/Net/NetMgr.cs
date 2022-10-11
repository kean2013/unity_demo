using UnityEngine;
using UnityWebSocket;
using System.Collections.Generic;
using System;
using Google.Protobuf;
using Com.Linekong.Game.Message.Proto;
using pb = global::Google.Protobuf;

namespace HotFix.Net
{
    using Common;
    public class NetMgr: Singleton<NetMgr>
    {
        private EventDelegate<COMMAND_CODE, byte[]> m_Delegate;

        public override void OnInit()
        {
            base.OnInit();
            m_Delegate = new EventDelegate<COMMAND_CODE, byte[]>();
        }

        public override void OnUnInit()
        {
            m_Delegate.Clear();
            base.OnUnInit();
        }

        public void Send(COMMAND_CODE msgid, IMessage imsg)
        {
            FakeServerMgr.Instance.SendTest(msgid);
        }

        public void Register(COMMAND_CODE msgid, Action<byte[]> action)
        {
            m_Delegate.Register(msgid, action);
        }

        public void UnRegister(COMMAND_CODE msgid, Action<byte[]> action)
        {
            m_Delegate.UnRegister(msgid, action);
        }

        public void Broadcast(COMMAND_CODE msgid, byte[] buffer)
        {
            m_Delegate.Broadcast(msgid, buffer);
        }

        public T GetMsg<T>(byte[] buffer) where T : IMessage, new()
        {
            try
            {
                CodedInputStream stream = new CodedInputStream(buffer);
                T t = new T();
                t.MergeFrom(stream);
                return t;
            }
            catch(Exception exp)
            {
                Debug.LogError( $"error msg and type {typeof(T)}, {exp}");
            }
            finally
            {

            }
            return default(T);
        }
    }
}
