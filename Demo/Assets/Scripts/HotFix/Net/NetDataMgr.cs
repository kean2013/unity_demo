using System.Collections;
using System.Collections.Generic;
using Com.Linekong.Game.Message.Proto;
using UnityEngine;

namespace HotFix.Net
{
    using Common;
    public class NetDataMgr : Singleton<NetDataMgr>
    {
        public AccountData Account;

        public override void OnInit()
        {
            Account = new AccountData();
        }
    }
}