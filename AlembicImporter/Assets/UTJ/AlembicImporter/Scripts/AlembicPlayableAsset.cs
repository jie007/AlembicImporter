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

        public AlembicStream alembicStream { get { return m_abcStream; } }
        public float time {
            get { return m_time; }
            set { m_time = value; LiveLink(); }
        }

        public override bool needsSceneReferencing { get { return true; } }

        public override Playable CreateInstance(GameObject go)
        {
            var playable = Playable.Create<AlembicClipInstance>();
            playable.SetData(this);
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

    public class AlembicClipInstance : ScriptPlayable
    {

        private AlembicPlayableAsset m_asset;

        public void SetData(AlembicPlayableAsset data)
        {
            m_asset = data;
        }

        public override void PrepareFrame(FrameData info)
        {
            Debug.Log("AlembicClipInstance.PrepareFrame()");
            m_asset.time = info.time;
        }

        public override void OnSetPlayState(PlayState newState)
        {
        }
    }


    public class AlembicClipMixer : ScriptPlayable
    {
        public override void PrepareFrame(FrameData info)
        {
            Debug.Log("AlembicClipMixer.PrepareFrame()");
        }
    }
}
#endif