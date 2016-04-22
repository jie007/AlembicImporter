#if UNITY_5_4_OR_NEWER
using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.Linq;
using UnityEngine.Experimental.Director;


namespace UTJ
{

    public class AlembicPlayableAsset : PlayableAsset, ISequenceClipAsset
    {
        [SerializeField] AlembicStream m_abcStream;
        [SerializeField] float m_time;


        public AlembicStream alembicStream {
            get { return m_abcStream; }
            set { m_abcStream = value; }
        }
        public float time {
            get { return m_time; }
            set { m_time = value; LiveLink(); }
        }

        public override bool needsSceneReferencing { get { return true; } }

        public override Playable CreateInstance(GameObject go)
        {
            var playable = Playable.Create<AlembicPlayable>();
            playable.SetData(this, go);
            return playable;
        }

        public void LiveLink()
        {
            if(m_abcStream != null)
            {
                Debug.Log("AlembicPlayableAsset.LiveLink()");
                m_abcStream.m_time = m_time;
            }
        }

        public bool supportsLooping { get { return false; } }
        public bool supportsExtrapolation { get { return true; } }
    }

    public class AlembicPlayable : ScriptPlayable
    {
        private AlembicPlayableAsset m_asset;
        private GameObject m_go;

        public void SetData(AlembicPlayableAsset data, GameObject go)
        {
            m_asset = data;
            m_go = go;
        }

        public override void PrepareFrame(FrameData info)
        {
            Debug.Log("AlembicPlayable.PrepareFrame()");
            m_asset.time = info.time;
        }

        public override void OnSetPlayState(PlayState newState)
        {
        }
    }


    public class AlembicMixer : ScriptPlayable
    {
        public override void PrepareFrame(FrameData info)
        {
            Debug.Log("AlembicClipMixer.PrepareFrame()");
        }
    }
}
#endif