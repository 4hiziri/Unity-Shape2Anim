using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultShape2Anim {
    public class DefaultShape2AnimMenu : MonoBehaviour {
        private string _anim_path = "Assets/";

        [MenuItem("CONTEXT/SkinnedMeshRenderer/Write all blend shape to Animation clip", false, 200)]
        static private void CreateAnimFromAllBlendShapes(MenuCommand cmd) {
            SkinnedMeshRenderer smrCmp = cmd.context as SkinnedMeshRenderer;
            List<(string, float)> blendshapes = ExtractBlendShapes(smrCmp);
            AnimationClip clip = new AnimationClip();

            foreach (var (shapeName, shapeVal) in blendshapes) {
                SetBlendShapeCurve(clip, GetRelativePath(smrCmp.gameObject), shapeName, shapeVal);
            }

            AssetDatabase.CreateAsset(clip, GetAnimSavePath(smrCmp.name));
            AssetDatabase.Refresh();

            Debug.Log("Write Clip: " + GetRelativePath(smrCmp.gameObject));
            Debug.Log("Write Clip: success");
        }

        [MenuItem("CONTEXT/SkinnedMeshRenderer/Write non-zero blend shape to Animation clip", false, 200)]
        static private void CreateAnimFromNonZeroBlendShapes(MenuCommand cmd) {
            SkinnedMeshRenderer smrCmp = cmd.context as SkinnedMeshRenderer;
            List<(string, float)> blendshapes = ExtractBlendShapes(smrCmp);
            AnimationClip clip = new AnimationClip();

            foreach (var (shapeName, shapeVal) in blendshapes) {
                if (shapeVal == 0.0f) continue;

                SetBlendShapeCurve(clip, GetRelativePath(smrCmp.gameObject), shapeName, shapeVal);
            }

            AssetDatabase.CreateAsset(clip, GetAnimSavePath(smrCmp.name));
            AssetDatabase.Refresh();

            Debug.Log("Write Clip: success");
        }

        // ignore vrc.* blendshape
        // conclude vrc., VRC.
        [MenuItem("CONTEXT/SkinnedMeshRenderer/Write blend shape except vrc.* to Animation clip", false, 200)]
        static private void CreateAnimFromNotVrcBlendShapes(MenuCommand cmd) {
            SkinnedMeshRenderer smrCmp = cmd.context as SkinnedMeshRenderer;
            List<(string, float)> blendshapes = ExtractBlendShapes(smrCmp);
            AnimationClip clip = new AnimationClip();

            foreach (var (shapeName, shapeVal) in blendshapes) {
                if (shapeName.StartsWith("vrc.")) continue;

                SetBlendShapeCurve(clip, GetRelativePath(smrCmp.gameObject), shapeName, shapeVal);
            }

            AssetDatabase.CreateAsset(clip, GetAnimSavePath(smrCmp.name));
            AssetDatabase.Refresh();

            Debug.Log("Write Clip: success");
        }

        static private void SetBlendShapeCurve(AnimationClip clip, string path, string shapeName, float shapeVal) {
            string propName = "blendShape." + shapeName;

            clip.SetCurve(path,
                          typeof(SkinnedMeshRenderer),
                          propName,
                          new AnimationCurve(new Keyframe(0.0f, shapeVal)));
        }

        static private string GetAnimSavePath(string basename) {
            return "Assets/" + basename + ".anim";
        }

        // extarct blendshape's name and value and return as list
        static private List<(string, float)> ExtractBlendShapes(SkinnedMeshRenderer smr) {
            List<(string, float)> blendShapes = new List<(string, float)>();
            Mesh mesh = smr.sharedMesh;
            int shape_num = mesh.blendShapeCount;

            for (int i = 0; i < shape_num; i++) {
                blendShapes.Add((mesh.GetBlendShapeName(i), smr.GetBlendShapeWeight(i)));
            }

            return blendShapes;
        }

        static private string GetRelativePath(GameObject cmdObj) {
            string path = cmdObj.name;
            Transform cur = (cmdObj.GetComponent(typeof(Transform)) as Transform).parent;

            while (true) {
                if (cur != null && cur.gameObject.GetComponent(typeof(Animator)) == null) {
                    path = cur.gameObject.name + "/" + path;
                    cur = cur.parent;
                } else {
                    break;
                }
            }

            return path;
        }
    }
}