using System;
using UnityEngine;

namespace HA
{
    public class HotfixMain : MonoBehaviour
    {
        public static HotfixMain instance { get; private set; }

        [SerializeField, Header("Dont Destroy")]
        private GameObject[] m_DontDestroyObjects;

        Transform FollowTarget; 

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            foreach (GameObject go in m_DontDestroyObjects)
            {
                if (go != null) { DontDestroyOnLoad(go); }
            }
            instance = this;
        }

        void Start()
        {

        }

        private void FixedUpdate()
        {
        }

        private void LateUpdate()
        {

        }
    }
}
