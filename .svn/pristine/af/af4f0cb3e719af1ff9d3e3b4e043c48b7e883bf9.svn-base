using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix.Demo
{
    using Event;
    using Mgr;
    using Net;
    using Common;
    using UI;

    public class DemoEntry : MonoBehaviour
    {
        private void Awake()
        {
            EventMgr.Instance.Init();
            FakeServerMgr.Instance.Init();
            TimerMgr.Instance.Init();
            UidMgr.Instance.Init();
            NetMgr.Instance.Init();
            UIMgr.Instance.Init();
            LoginUIMgr.Instance.Init();

            DontDestroyOnLoad(this);
        }
        // Start is called before the first frame update
        void Start()
        {
            UIMgr.Instance.Open<LoginUIView>();
        }

        // Update is called once per frame
        void Update()
        {
            TimerMgr.Instance.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            EventMgr.Instance.UnInit();
            FakeServerMgr.Instance.UnInit();
            TimerMgr.Instance.UnInit();
        }
    }
}
