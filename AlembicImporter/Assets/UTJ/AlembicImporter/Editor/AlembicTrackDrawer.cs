#if UNITY_5_4_OR_NEWER
using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Director;
using System.Reflection;
using System.Linq;
using UnityEngine.Experimental.Director;

namespace UTJ
{
    [CustomTrackDrawer(typeof(AlembicTrack))]
    public class AlembicTrackDrawer : TrackDrawer
    {
        public AlembicTrackDrawer()
        {
            m_ClipColor = new Color(0.5f, 0.2f, 0.2f);
        }

        public override bool OnAddSubTrackMenu(TrackAsset track, ISequencerState state)
        {
            Type[] allScriptPlayables = new Type[] { typeof(AlembicPlayableAsset) };

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Alembic Clip"), false, () =>
            {
                var playableAsset = ScriptableObject.CreateInstance<AlembicPlayableAsset>();
                playableAsset.name = "Alembic Clip";
                double trackEndTime = SequencerHelpers.GetTrackEndTime(track);
                SequenceAsset.Clip newClip = track.CreateClip(playableAsset);
                if (newClip != null)
                {
                    newClip.timeScale = 1.0;
                    newClip.start = trackEndTime;
                    newClip.m_ID = state.sequence.GenerateNewId();
                    newClip.mixInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
                    newClip.mixOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
                    newClip.selected = true;
                    SequencerHelpers.SaveAssetIntoObject(playableAsset, track);

                    Rect area = (state.editorWindow as SequencerWindow).timeline.shownArea;
                    double visibleEndTime = area.x + area.width;

                    double clipDuration = newClip.duration;
                    if (System.Double.IsInfinity(clipDuration))
                    {
                        clipDuration = 10.0;
                    }

                    if ((newClip.start + clipDuration) > visibleEndTime)
                    {
                        (state.editorWindow as SequencerWindow).timeline.SetShownHRange(area.x, (float)(newClip.start + clipDuration) - area.x);
                    }

                    state.Refresh();
                }
            });
            menu.ShowAsContext();

            return true;
        }
    };

}
#endif