using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JEngine.Core
{
    // 异步状态机的中心在动作上
    // 一般倾向下，动作直接改变状态；异步状态机，需要等待动作的结果返回。过程中，动作可能被打断

    public class StateBase
    {
        public JFsm FSM;

        public Enum ID;
        public StateBase(JFsm fsm, Enum Id)
        {
            ID = Id;
            FSM = fsm;
        }

        public virtual void OnExit() { }
        public virtual void OnEnter() { }
        public virtual void OnStay() { }
    }

    // 响应外界的输入，等待操作的返回
    public class JFsm
    {
        public Dictionary<Enum, StateBase> _dict = new Dictionary<Enum, StateBase>();
        public Dictionary<string, Enum> _actMgr = new Dictionary<string, Enum>();

        private StateBase _curr = null;
        private GameObject _owner;

        public UnityEvent OnExitFsm = new UnityEvent();

        public JFsm(GameObject owner, StateBase beginState)
        {
            _owner = owner;

            AddState(beginState);
            TranslateState(beginState.ID);
        }

        public void AddState(StateBase state)
        {
            if (!_dict.ContainsKey(state.ID))
            {
                _dict.Add(state.ID, state);
            }
        }

        public Enum State => _curr.ID;
        public GameObject Owner => _owner;

        public void TranslateState(Enum Id)
        {
            if (!_dict.ContainsKey(Id)) return;

            _curr?.OnExit();
            _curr = _dict[Id];
            _curr.OnEnter();
        }

        public void Tick() => _curr?.OnStay();
        
        public void Exit()
        {
            _curr?.OnExit();
            _curr = null;

            OnExitFsm?.Invoke();
        }

        public bool Progressing = false;

        //JAction是一个Action的派发系统，派发完成即完成
        //执行完成的检查可使用其中的Until来实现
        //JAction使用了一个静态列表，所有的对象都使用此列表
        public IEnumerator OnAction<T>(string action, Action doRequest, JAwaiter<T> awaiter, Action<T> onSucc, Action<int> onFail, float timeout = 6.0f)
        {
            if (! _actMgr.ContainsKey(action))
            {
                yield break;
            }

            Progressing = true;
            doRequest?.Invoke();

            if (timeout > 0f)
            {
                while ((timeout -= Time.deltaTime) > 0)
                {
                    if (awaiter.IsCompleted ) break;
                    yield return 1;
                }
            }
            else
            {
                yield return new WaitUntil(() => awaiter.IsCompleted);
            }

            Progressing = false;

            if (awaiter.IsCompleted)
            {
                onSucc?.Invoke(awaiter.GetResult());

                if ( _actMgr.TryGetValue(action, out Enum Id))
                {
                    TranslateState(Id);
                }
            }else
            {
                onFail?.Invoke( timeout < 0f ? -255 : 0);
            }
        }

        public IEnumerator OnAction<T>(string action, Action doRequest, JAsyncResult<T> jAr, float timeout)
        {
            return OnAction<T>(action, doRequest, jAr.Awaiter, jAr.onSuccess, jAr.onFail, timeout);
        }
    }

    // 将Timeout定义为一个错误代码
    public class JAsyncResult<T>
    {
        JAwaiter<T> _awaiter;

        public JAwaiter<T> Awaiter => _awaiter;
        public JAsyncResult (JAwaiter<T> awaiter){ _awaiter = awaiter; }
        public virtual void onSuccess(T r) { }
        public virtual void onFail(int errno) { if (errno == -255) OnTimeout(); }
        public virtual void OnTimeout() { }
    }
}
