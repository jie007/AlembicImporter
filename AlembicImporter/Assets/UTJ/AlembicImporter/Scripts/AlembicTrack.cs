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
    [ClipClass(typeof(AlembicStream))]
    [ClipClass(typeof(AlembicPlayableAsset))]
    [TrackMediaType(SequenceAsset.MediaType.Script)]
    [UseGameObject]
    public class AlembicTrack : TrackAsset
    {
        public override Playable OnCreatePlayableGraph(GameObject go, IntervalTree tree)
        {
            var mixer = Playable.Create<AlembicMixer>();
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
            if (asset == null)
            {
                throw new System.ArgumentException("asset cannot be null");
            }

            if (asset is AlembicStream)
            {
                var go = asset as AlembicStream;
                var psasset = ScriptableObject.CreateInstance<AlembicPlayableAsset>();
                psasset.alembicStream = go;
                psasset.name = go.name;
                asset = psasset;
            }

            if (asset is AlembicPlayableAsset)
            {
                var clip = CreateNewClipContainerInternal();
                clip.asset = asset;
                clip.displayName = asset.name;
                clip.duration = (asset as AlembicPlayableAsset).duration;
                clip.timeScale = 1;
                clip.postExtrapolationMode = SequenceAsset.Clip.Extrapolation.None;
                return clip;
            }
            return null;
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