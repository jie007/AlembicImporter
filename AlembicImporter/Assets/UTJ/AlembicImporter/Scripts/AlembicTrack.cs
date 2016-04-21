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

    [System.Serializable]
    [ClipClass(typeof(MyStuff.CameraShotExample))]
    [TrackMediaType(SequenceAsset.MediaType.Script, true)]
    public class AlembicTrack : TrackAsset
    {
        protected AlembicPlayableAsset m_abcPlayableAsset;

        public override Playable OnCreatePlayableGraph(GameObject go, IntervalTree tree)
        {
            var mixer = Playable.Create<AlembicClipMixer>();
            foreach (var c in clips)
            {
                var clip = c.asset as AlembicPlayableAsset;
                if (clip == null) { continue; }

                Playable source = clip.CreateInstance(go);
                tree.Add(new RuntimeClip(c, source, mixer));
                Playable.Connect(source, mixer);
            }

            return mixer;
        }

        public override SequenceAsset.Clip CreateClip(UnityEngine.Object asset)
        {
            double currentDuration = 0;
            foreach (var c in clips)
            {
                double clipEndTime = c.start + c.duration;
                currentDuration = Math.Max(currentDuration, clipEndTime);
            }
            var newClip = base.CreateClip(asset);
            newClip.duration = 10.0f;
            newClip.displayName = "shot" + clips.Length;
            newClip.start = currentDuration;
            return newClip;
        }

        public override bool recordable
        {
            get { return true; }
        }

        public override bool isOutputTrack
        {
            get { return false; }
        }
    }

}
#endif