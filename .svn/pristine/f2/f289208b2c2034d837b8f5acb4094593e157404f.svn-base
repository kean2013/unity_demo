using UnityEngine;
using UnityWebSocket;
using System.Collections.Generic;
using System;
using Google.Protobuf;
using Com.Linekong.Game.Message.Proto;
using pb = global::Google.Protobuf;
using HotFix.Common;

namespace HotFix.Net
{
    public class FakeServerMgr: Singleton<FakeServerMgr>
    {
        private Dictionary<COMMAND_CODE, Tuple<COMMAND_CODE, IMessage>> m_Actions;

        public override void OnInit()
        {
            m_Actions = new Dictionary<COMMAND_CODE, Tuple<COMMAND_CODE, IMessage>>();
            base.OnInit();
        }

        public override void OnUnInit()
        {
            base.OnUnInit();
        }

        public void Register(COMMAND_CODE send, COMMAND_CODE reply, IMessage msg)
        {
            m_Actions[send] = Tuple.Create(reply, msg);
        }

        public void SendTest(COMMAND_CODE msgid)
        {
            Tuple<COMMAND_CODE, IMessage> tmp;
            if (m_Actions.TryGetValue(msgid, out tmp))
            {
                NetMgr.Instance.Broadcast(tmp.Item1, tmp.Item2.ToByteArray());
            }
        }
    }
}
