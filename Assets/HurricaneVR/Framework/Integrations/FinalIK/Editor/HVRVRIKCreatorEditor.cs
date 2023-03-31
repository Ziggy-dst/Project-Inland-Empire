using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HurricaneVR.Editor;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.HandPoser;
using HurricaneVR.Framework.Core.Player;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Integrations.FinalIK;
using HurricaneVR.Framework.Shared;
using RootMotion.FinalIK;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace HurricaneVR
{
    public class HVRVRIKCreatorEditor : EditorWindow
    {
        public Transform avatarRoot;

        [MenuItem("Tools/HurricaneVR/VRIK Setup")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<HVRVRIKCreatorEditor>();
            wnd.titleContent = new GUIContent("VRIK Setup");
        }

        private void OnEnable()
        {
            _mirrorText = Mirroring ? "Stop Mirroring" : "Start Mirroring";
            SceneView.duringSceneGui += OnSceneGui;
        }

        private void OnDisable()
        {
            UnsubSceneGui();
        }

        private bool _mirroring => Mirroring;
        private string _mirrorText = "Mirror";

        private GUIStyle helpBoxStyle;
        private bool AutoAddTips = true;
        private bool HasTipsAlready;
        private bool SetupAnimatorSettings = true;

        public bool HandsSetup;
        public bool MirrorDetected;
        public bool RigSetup;
        public bool PosesAssigned;
        public bool ColliderSetup;

        public bool Mirroring;

        private Animator _animator;

        public HVRPosableHand RightHand;
        public HVRPosableHand LeftHand;

        public HVRPhysicsPoser LeftPhysics;
        public HVRPhysicsPoser RightPhysics;

        public HVRHandPoser LeftPoser;
        public HVRHandPoser RightPoser;

        public HVRHandPose OpenPose;
        public HVRHandPose ClosedPose;
        public HVRHandPose RelaxedPose;

        private Vector2 scroll;

        public void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Disabled while in play mode.", MessageType.Info);
                return;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);

            try
            {
                if (helpBoxStyle == null)
                {
                    helpBoxStyle = GUI.skin.GetStyle("HelpBox");
                    helpBoxStyle = new GUIStyle(helpBoxStyle);
                    helpBoxStyle.fontSize = 13;
                }

                var current = avatarRoot;

                EditorGUILayout.LabelField("Assign the Avatar Root", EditorStyles.boldLabel);
                HVREditorExtensions.ObjectField("Avatar", ref avatarRoot);

                if (!avatarRoot || (current && current != avatarRoot))
                {
                    _animator = null;
                    RightHand = LeftHand = null;
                    LeftPoser = RightPoser = null;
                    LeftPhysics = RightPhysics = null;
                    HandsSetup = MirrorDetected = RigSetup = PosesAssigned = false;
                }


                if (!avatarRoot)
                    return;



                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Progress Tracking", EditorStyles.boldLabel);

                HVREditorExtensions.Toggle("VRIK / Hand Setup", ref HandsSetup);
                HVREditorExtensions.Toggle("Mirror Detected", ref MirrorDetected);
                HVREditorExtensions.Toggle("Rig Setup", ref RigSetup);
                HVREditorExtensions.Toggle("Poses Assigned", ref PosesAssigned);
                //HVREditorExtensions.Toggle("Collider Setup", ref ColliderSetup);

                Mirror();

                if (!LeftHand) LeftHand = avatarRoot.transform.GetComponentsInChildren<HVRPosableHand>().FirstOrDefault(e => e.IsLeft);
                if (!RightHand) RightHand = avatarRoot.transform.GetComponentsInChildren<HVRPosableHand>().FirstOrDefault(e => !e.IsLeft);

                if (!HandsSetup) VRIKRigSetup();
                if (!MirrorDetected) DetectMirror();
                if (!RigSetup) SetupRig();
                if (!PosesAssigned) PoseValidation();
                FinalSetup();


            }
            finally
            {
                EditorGUILayout.EndScrollView();
            }

        }

        private void PoseValidation()
        {
            if (!LeftHand || !LeftHand.UseMatchRotation || !RightHand || !RightHand.UseMatchRotation || !RigSetup) return;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Pose Setup", EditorStyles.boldLabel);

            if (!LeftPhysics) LeftHand.TryGetComponent(out LeftPhysics);
            if (!RightPhysics) RightHand.TryGetComponent(out RightPhysics);

            if (!LeftPoser) LeftHand.TryGetComponent(out LeftPoser);
            if (!RightPoser) RightHand.TryGetComponent(out RightPoser);

            var error = new StringBuilder();

            if (!LeftPhysics) error.AppendLine("Left HVRPhysicsPoser missing.");
            else
            {
                if (LeftPhysics.OpenPose == null) error.AppendLine("Left HVRPhysicsPoser missing OpenPose.");
                if (LeftPhysics.ClosedPose == null) error.AppendLine("Left HVRPhysicsPoser missing ClosedPose.");
            }
            if (!RightPhysics) error.AppendLine("Right HVRPhysicsPoser missing.");
            else
            {
                if (RightPhysics.OpenPose == null) error.AppendLine("Right HVRPhysicsPoser missing OpenPose.");
                if (RightPhysics.ClosedPose == null) error.AppendLine("Right HVRPhysicsPoser missing ClosedPose.");
            }

            if (!LeftPoser) error.AppendLine("Left HVRHandPoser missing.");
            else
            {
                if (LeftPoser.PrimaryPose.Pose == null) error.AppendLine("Left HVRHandPoser Primary Pose not assigned.");
            }
            if (!RightPoser) error.AppendLine("Right HVRHandPoser missing.");
            else if (RightPoser.PrimaryPose.Pose == null) error.AppendLine("Right HVRHandPoser Primary Pose not assigned.");


            EditorGUILayout.TextArea("1. Pressing Auto Setup will auto create a prefab of your avatar in the Assets Folder and automate step 2. Either do this manually or let the automation do it for you.\r\n\r\n" +
                                     "2. Locate HVRSettings in your project folder:\r\n" +
                                     "  - Assign the prefab to the 'Full Body' field.\r\n" +
                                     "  - Toggle on 'Inverse Kinematics'.\r\n" +
                                     "  - Clear out the 'Open Hand Pose' field.\r\n\r\n" +
                                     "3. Press Create Hand Poser and begin creating the required poses.\r\n" +
                                     "  - RelaxedPose: for when your hands are not holding something.\r\n" +
                                     "  - ClosedPose: for the physics poser, and for the resting pose to blend into using finger curl information.\r\n" +
                                     "  - OpenPose: a flat 'Open Hand Pose' pose for the physics poser.\r\n\r\n" +
                                     "4. Assign the poses below and press 'Setup Poses'", helpBoxStyle);

            RelaxedPose = EditorGUILayout.ObjectField("Relaxed Pose:", RelaxedPose, typeof(HVRHandPose), true) as HVRHandPose;
            OpenPose = EditorGUILayout.ObjectField("Open Pose:", OpenPose, typeof(HVRHandPose), true) as HVRHandPose;
            ClosedPose = EditorGUILayout.ObjectField("Closed Pose:", ClosedPose, typeof(HVRHandPose), true) as HVRHandPose;

            if (GUILayout.Button("Auto Setup Prefab / HVRSettings"))
            {
                avatarRoot.ResetLocalProps(false);
                var prefabPath = $"Assets/{avatarRoot.name}.prefab";
                var bodyPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(avatarRoot.gameObject, prefabPath, InteractionMode.UserAction);

                Debug.Log($"Prefab created at {prefabPath}");

                var so = new SerializedObject(HVRSettings.Instance);
                var body = so.FindProperty("FullBody");
                so.FindProperty("OpenHandPose").objectReferenceValue = null;
                so.FindProperty("InverseKinematics").boolValue = true;

                body.objectReferenceValue = bodyPrefab;

                so.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log($"HVRSettings:InverseKinematics set to true.");
                Debug.Log($"HVRSettings:OpenHandPose cleared.");
                Debug.Log($"HVRSettings:FullBody = {prefabPath}.");
            }

            if (GUILayout.Button("Create Hand Poser"))
            {
                var poser = GameObject.Find("PoserSetup");
                if (!poser) poser = new GameObject("PoserSetup");
                var p = poser.UndoEnsureComponent<HVRHandPoser>();
                poser.transform.position += Vector3.up * 1.7f;
                Selection.activeGameObject = poser;
            }

            if (GUILayout.Button("Setup Poses"))
            {
                SetupPhysicsPoserPoses(LeftPhysics);
                SetupPhysicsPoserPoses(RightPhysics);

                SetupHandPoser(LeftPoser);
                SetupHandPoser(RightPoser);

                var leftHand = avatarRoot.transform.root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Left);
                var rightHand = avatarRoot.transform.root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Right);

                SetupFallback(leftHand);
                SetupFallback(rightHand);

                PosesAssigned = true;

                EditorApplication.delayCall += () =>
                {
                    var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(avatarRoot.gameObject);
                    Debug.Log($"Applying pose overrides to {path}.");
                    PrefabUtility.ApplyObjectOverride(LeftPhysics, path, InteractionMode.AutomatedAction);
                    PrefabUtility.ApplyObjectOverride(RightPhysics, path, InteractionMode.AutomatedAction);
                    PrefabUtility.ApplyObjectOverride(LeftPoser, path, InteractionMode.AutomatedAction);
                    PrefabUtility.ApplyObjectOverride(RightPoser, path, InteractionMode.AutomatedAction);
                };
            }

            if (error.Length > 0)
            {
                EditorGUILayout.HelpBox(error.ToString(), MessageType.Error);
            }
        }

        private void SetupFallback(HVRHandGrabber hand)
        {
            if (hand && hand.FallbackPoser)
            {
                var so = new SerializedObject(hand.FallbackPoser);
                var blend = so.FindProperty("PrimaryPose");
                var pose = blend.FindPropertyRelative("Pose");
                pose.objectReferenceValue = ClosedPose;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(hand);
            }
        }

        private void SetupHandPoser(HVRHandPoser poser)
        {
            var so = new SerializedObject(poser);
            var blend = so.FindProperty("PrimaryPose");
            var pose = blend.FindPropertyRelative("Pose");
            pose.objectReferenceValue = RelaxedPose;

            so.ApplyModifiedProperties();

            var SP_Blends = so.FindProperty("Blends");

            var i = SP_Blends.arraySize;
            if (i == 0) SP_Blends.InsertArrayElementAtIndex(i);

            i = 0;

            var test = SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("Pose");

            test.objectReferenceValue = ClosedPose;

            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("AnimationParameter").stringValue = HVRHandPoseBlend.DefaultParameter;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("Weight").floatValue = 1f;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("Speed").floatValue = 16f;

            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("Mask").enumValueIndex = (int)HVRHandPoseMask.None;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("Type").enumValueIndex = (int)BlendType.Immediate;

            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("ThumbType").enumValueIndex = (int)HVRFingerType.Close;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("IndexType").enumValueIndex = (int)HVRFingerType.Close;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("MiddleType").enumValueIndex = (int)HVRFingerType.Close;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("RingType").enumValueIndex = (int)HVRFingerType.Close;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("PinkyType").enumValueIndex = (int)HVRFingerType.Close;

            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("ThumbStart").floatValue = 0f;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("IndexStart").floatValue = 0f;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("MiddleStart").floatValue = 0f;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("RingStart").floatValue = 0f;
            SP_Blends.GetArrayElementAtIndex(i).FindPropertyRelative("PinkyStart").floatValue = 0f;

            so.ApplyModifiedProperties();


        }

        private void SetupPhysicsPoserPoses(HVRPhysicsPoser pp)
        {
            var so = new SerializedObject(pp);
            var open = so.FindProperty("OpenPose");
            var closed = so.FindProperty("ClosedPose");

            open.objectReferenceValue = OpenPose;
            closed.objectReferenceValue = ClosedPose;

            so.ApplyModifiedProperties();
        }

        private string _ikText = "Start Solving";
        private bool _solving;
        private Dictionary<Transform, Vector3> _cachePos = new Dictionary<Transform, Vector3>();
        private Dictionary<Transform, Quaternion> _cacheRot = new Dictionary<Transform, Quaternion>();

        private Transform leftTarget;
        private Transform rightTarget;

        private Transform leftParent;
        private Transform rightParent;

        private void FinalSetup()
        {
            if (!LeftHand || !LeftHand.UseMatchRotation || !RightHand || !RightHand.UseMatchRotation || avatarRoot.parent == null || !RigSetup || !PosesAssigned) return;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Final Setup", EditorStyles.boldLabel);

            EditorGUILayout.TextArea("Press Start Solving (avatar transform's position and rotations will be cached) to begin adjusting the IK Targets under the physics hands, orient them so that your avatar hand matches the existing hand model." +
                                     "\r\n\r\nYou can also adjust the IKTargets in play mode if you are not satisfied with the hand orientation." +
                                     "\r\n\r\nLocate 'JointAnchor' on each physics hand and move it to the center of the palm" +
                                     "\r\n\r\nAfter the IKTargets have been positioned, press 'Add Collider Controller' which will add a component to the root that can dynamically manage hand colliders. " +
                                     "\r\n\r\nPress Stop Solving once done. The avatar transform's will be reset to the cached values.\r\n" +
                                     "\r\nA Box collider will be added to the hand rigidbody relative to the avatar's hand, so it is critical that you placed the IKTargets accurately first, and allowed the VRIK to solve in the editor.\r\n" +
                                     "\r\nYou can always adjust the box collider shape and rotation manually in play mode or by enabling the editor IK solve to position the box.\r\n" +
                                     "\r\nAssign an animation curve on the collider controller, which will be used in conjunction with finger curl information to shrink the box to fit a fist pose. The default exponential curve provided by unity seems to work fine.\r\n" +
                                     "\r\nThe collider controller is not required, you can remove it if you like. Just keep in mind the colliders are placed on the hand rigid body, and not on the avatar itself.",
                helpBoxStyle);

            HVREditorExtensions.Toggle("Mirror", ref _solveMirror);

            if (GUILayout.Button(_ikText))
            {
                var ik = avatarRoot.GetComponent<HVREditorIK>();
                var vrik = avatarRoot.GetComponent<VRIK>();

                if (_solving)
                {
                    foreach (var kvp in _cachePos) if (kvp.Key) kvp.Key.localPosition = kvp.Value;
                    foreach (var kvp in _cacheRot) if (kvp.Key) kvp.Key.localRotation = kvp.Value;

                    vrik.solver.locomotion.mode = _saveMode;
                    ik.ForceEditorIK = false;

                    if (leftTarget) leftTarget.transform.parent = leftParent;
                    if (rightTarget) rightTarget.transform.parent = rightParent;
                }
                else
                {
                    leftTarget = vrik.solver.leftArm.target;
                    rightTarget = vrik.solver.rightArm.target;

                    if (!leftTarget || !rightTarget)
                    {
                        EditorUtility.DisplayDialog("Error", "VRIK Missing Left or Right arm targets.", "Ok");
                    }
                    else
                    {

                        _cachePos.Clear();
                        _cacheRot.Clear();

                        foreach (var t in avatarRoot.GetComponentsInChildren<Transform>())
                        {
                            _cachePos[t] = t.localPosition;
                            _cacheRot[t] = t.localRotation;
                        }

                        vrik.solver.SetToReferences(vrik.references);
                        vrik.solver.Initiate(vrik.transform);


                        var leftHand = avatarRoot.root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Left);
                        var rightHand = avatarRoot.root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Right);

                        chest = vrik.references.chest;

                        if (!chest && _animator) chest = _animator.GetBoneTransform(HumanBodyBones.Chest);

                        if (leftHand && rightHand && chest)
                        {
                            leftHand.transform.position = chest.transform.position + avatarRoot.forward * .35f - avatarRoot.right * .1f;
                            rightHand.transform.position = chest.transform.position + avatarRoot.forward * .35f + avatarRoot.right * .1f;

                            leftHand.transform.localRotation = Quaternion.identity;
                            rightHand.transform.localRotation = Quaternion.identity;
                        }

                        leftParent = leftTarget.parent;
                        leftTarget.transform.parent = null;
                        rightParent = rightTarget.parent;
                        rightTarget.transform.parent = null;

                        avatarRoot.root.localRotation = Quaternion.identity;



                        _leftTargetHand = avatarRoot.GetComponentsInChildren<HVRPosableHand>().FirstOrDefault(e => e.IsLeft);
                        _rightTargetHand = avatarRoot.GetComponentsInChildren<HVRPosableHand>().FirstOrDefault(e => !e.IsLeft);

                        Selection.activeGameObject = rightTarget.gameObject;

                        _saveMode = vrik.solver.locomotion.mode;
                        vrik.solver.locomotion.mode = IKSolverVR.Locomotion.Mode.Procedural;
                        ik.ForceEditorIK = true;
                    }
                }

                PatchVRIKInitiated(vrik.solver.spine);
                PatchVRIKInitiated(vrik.solver.leftArm);
                PatchVRIKInitiated(vrik.solver.rightArm);
                PatchVRIKInitiated(vrik.solver.leftLeg);
                PatchVRIKInitiated(vrik.solver.rightLeg);

                _solving = !_solving;
                _ikText = _solving ? "Stop Solving" : "Start Solving";
            }

            if (GUILayout.Button("Add Collider Controller"))
            {
                var root = avatarRoot.transform.root;
                var cc = root.EnsureComponent<HVRHandColliderController>();

                cc.LeftPosableHand = LeftHand;
                cc.RightPosableHand = RightHand;

                var animator = _animator;

                cc.LeftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
                cc.RightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
                cc.LeftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                cc.RightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);

                var leftHand = root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Left);
                var rightHand = root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Right);

                if (leftHand)
                    cc.LeftParent = Add(leftHand.transform, "Colliders");
                if (rightHand)
                    cc.RightParent = Add(rightHand.transform, "Colliders");

                cc.AddHandBoxesF();

                ColliderSetup = true;
            }
        }

        private void PatchVRIKInitiated(IKSolverVR.BodyPart part)
        {
            var t = part.GetType();
            var field = t.GetField("initiated", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) field.SetValue(part, false);
            else Debug.LogWarning($"couldnt find initiate field on vrik body part");
        }

        private void SetupRig()
        {
            if (!LeftHand || !LeftHand.UseMatchRotation || !RightHand || !RightHand.UseMatchRotation) return;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rig Setup", EditorStyles.boldLabel);

            if (avatarRoot.parent == null)
            {
                EditorGUILayout.HelpBox("1. HVRPlayerController: move the avatar under the 'PlayerController' transform.\r\n" +
                                        "2. HexaBody: Add 'HexaVRIKUpdater' component and assign the HexaBody sphere collider on the 'LocoBall' to the Locosphere field. Then move the avatar under the HexaBody root.\r\n", MessageType.Error);
                return;
            }

            if (avatarRoot.root.GetComponentsInChildren<HVRHandGrabber>().Length == 0)
            {
                return;
            }

            EditorGUILayout.TextArea("1. Press 'Setup Rig References', this will create IK Targets for the hands and head and update required component references for you\r\n"

                , helpBoxStyle);

            if (GUILayout.Button("Setup Rig References"))
            {
                SetupRigRefs();
                RigSetup = true;
            }
        }

        private GameObject _clone;

        private void Mirror()
        {
            if (!LeftHand || !LeftHand.UseMatchRotation || !RightHand || !RightHand.UseMatchRotation || !HandsSetup || !MirrorDetected) return;


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mirror Testing", EditorStyles.boldLabel);

            if (GUILayout.Button(_mirrorText))
            {
                if (!Mirroring)
                {
                    var clone = Object.Instantiate(avatarRoot);
                    clone.transform.parent = null;
                    clone.transform.position += Vector3.right * 3f;
                    clone.transform.gameObject.SetExpandedRecursive(true);

                    _leftMirrorHand = clone.GetComponentsInChildren<HVRPosableHand>().FirstOrDefault(e => e.IsLeft);
                    _rightMirrorHand = clone.GetComponentsInChildren<HVRPosableHand>().FirstOrDefault(e => !e.IsLeft);

                    _clone = clone.gameObject;

                    Selection.activeGameObject = _rightMirrorHand.gameObject;
                }
                else
                {
                    if (_clone)
                    {
                        DestroyImmediate(_clone);
                        _leftMirrorHand = null;
                        _rightMirrorHand = null;
                    }

                    if (avatarRoot) Selection.activeGameObject = avatarRoot.gameObject;
                }

                Mirroring = !Mirroring;
                _mirrorText = Mirroring ? "Stop Mirroring" : "Start Mirroring";
            }
        }

        private void UnsubSceneGui()
        {
            SceneView.duringSceneGui -= OnSceneGui;
        }

        private bool _badMirror;
        private string _badMirrorText;

        private void DetectMirror()
        {
            if (!LeftHand || !RightHand) return;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mirror Detect", EditorStyles.boldLabel);

            EditorGUILayout.TextArea("Orient the hands into a mirrored pose with all fingers pointing in the same direction as the " +
                                     "forward (blue) axis of the avatar and the palms facing each other.\r\n\r\n" +
                                     "The thumb bones may need to be bent a bit to align it's forward vector with the avatar's forward vector if the detection fails.\r\n\r\n" +
                                     "Once Done Press Detect Mirror", helpBoxStyle);

            if (GUILayout.Button("Detect Mirror"))
            {
                var leftHand = LeftHand;
                var rightHand = RightHand;

                var forward = avatarRoot.transform.forward;
                var up = avatarRoot.transform.up;

                DetectHandMirror(leftHand, rightHand, forward, up);
                DetectHandMirror(rightHand, leftHand, forward, up);

                leftHand.DetectBoneAxes(rightHand, avatarRoot.transform.forward, avatarRoot.transform.up);
                rightHand.DetectBoneAxes(leftHand, avatarRoot.transform.forward, avatarRoot.transform.up);

                _badMirrorText = "";
                _badMirror = false;

                ValidateMirrorSettings(leftHand);
                ValidateMirrorSettings(rightHand);

                EditorUtility.SetDirty(leftHand);
                EditorUtility.SetDirty(rightHand);

                MirrorDetected = !_badMirror;
            }

            if (_badMirror)
            {
                EditorGUILayout.TextArea($"Unable to detect all forward and up axes of the bones.\r\n\r\n" +
                                         $"Bend the offending fingers so that each bone's forward and up axes align more closely with the avatar's forward and up axes.\r\n\r\n" +
                                         $"{_badMirrorText}", helpBoxStyle);
            }
        }

        private void ValidateMirrorSettings(HVRPosableHand hand)
        {
            for (var i = 0; i < hand.Fingers.Length; i++)
            {
                var finger = hand.Fingers[i];
                for (var j = 0; j < finger.Bones.Count; j++)
                {
                    var bone = finger.Bones[j];
                    if (!Mathf.Approximately(0f, Vector3.Dot(bone.Forward, bone.Up)))
                    {
                        _badMirror = true;
                        _badMirrorText += $"{bone.Transform.name.PadRight(25)}: Forward ({bone.Forward.LogFormatF0()}) Up: ({bone.Up.LogFormatF0()}).\r\n";
                    }

                    if (!Mathf.Approximately(0f, Vector3.Dot(bone.OtherForward, bone.OtherUp)))
                    {
                        _badMirror = true;
                        _badMirrorText += $"{bone.Transform.name.PadRight(25)}: OtherForward ({bone.OtherForward.LogFormatF0()}) OtherUp: ({bone.OtherUp.LogFormatF0()}).\r\n";
                    }
                }
            }
        }

        private void VRIKRigSetup()
        {
            if (!_animator)
            {
                avatarRoot.gameObject.TryGetComponent(out _animator);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("VRIK / Hand Setup", EditorStyles.boldLabel);

            if (!_animator || !_animator.isHuman)
            {
                EditorGUILayout.HelpBox("Animator with Humanoid avatar is required to automate the hand setup.", MessageType.Error);
                return;
            }

            EditorGUILayout.TextArea("Enable 'Last Child is Tip' if your avatar came with tip transforms.\r\n" +
                                 "Enable 'Auto Add tips' if your avatar does not have tip transforms.\r\n" +
                                 "\r\nAfter Setup, move the tip transforms to the end of the finger in the center.\r\n" +
                                 "\r\nMove the 'Palm' transforms that were added to the hands to the center of the palm with the forward (blue) axis facing out of the palm.", helpBoxStyle);

            HVREditorExtensions.Toggle("Auto Add Tips", ref AutoAddTips);
            HVREditorExtensions.Toggle("Last Child is Tip", ref HasTipsAlready);
            HVREditorExtensions.Toggle("Setup VRIK Animator Settings", ref SetupAnimatorSettings);

            if (GUILayout.Button("Setup"))
            {
                Undo.SetCurrentGroupName("IK Setup");

                var vrik = avatarRoot.gameObject.UndoEnsureComponent<VRIK>();

                if (SetupAnimatorSettings)
                {
                    var loco = vrik.solver.locomotion;
                    loco.mode = IKSolverVR.Locomotion.Mode.Animated;
                    loco.rootLerpSpeedWhileMoving = 30f;
                    loco.rootLerpSpeedWhileStopping = 30f;
                    loco.rootLerpSpeedWhileTurning = 30f;
                    loco.maxRootAngleMoving = 10f;
                    loco.maxRootAngleStanding = 25f;
                    loco.weight = 1f;
                }

                var iktargets = avatarRoot.gameObject.UndoEnsureComponent<HVRIKTargets>();
                var eIk = avatarRoot.gameObject.UndoEnsureComponent<HVREditorIK>();


                eIk.Targets = iktargets;
                EditorUtility.SetDirty(eIk);

                var left = SetupHand(true, _animator.GetBoneTransform(HumanBodyBones.LeftHand), _animator);
                var right = SetupHand(false, _animator.GetBoneTransform(HumanBodyBones.RightHand), _animator);

                left.SetupFingerArray();
                right.SetupFingerArray();

                EditorUtility.SetDirty(left);
                EditorUtility.SetDirty(right);

                HandsSetup = true;
            }
        }

        private HVRPosableHand _leftMirrorHand, _rightMirrorHand;
        private IKSolverVR.Locomotion.Mode _saveMode;

        private HVRPosableHand _leftTargetHand, _rightTargetHand;
        private bool _solveMirror = true;
        private Transform chest;


        private void OnSceneGui(SceneView obj)
        {
            if (_mirroring && _leftMirrorHand && _rightMirrorHand)
            {
                Quaternion mirrorL = RootMotion.QuaTools.MirrorYZ(_rightMirrorHand.transform.rotation, _clone.transform.rotation);

                _leftMirrorHand.transform.rotation = RootMotion.QuaTools.MatchRotation(mirrorL,
                    _rightMirrorHand.TargetAxis1.GetVector(),
                    _rightMirrorHand.TargetAxis2.GetVector(),
                    _rightMirrorHand.Axis1.GetVector(),
                    _rightMirrorHand.Axis2.GetVector());

                _rightMirrorHand.HandMirrorer.MirrorFingers(_rightMirrorHand, _leftMirrorHand);
            }

            if (_solveMirror && _solving && leftTarget && rightTarget && _rightTargetHand)
            {
                Quaternion mirrorL = RootMotion.QuaTools.MirrorYZ(rightTarget.rotation, avatarRoot.rotation);

                leftTarget.rotation = RootMotion.QuaTools.MatchRotation(mirrorL,
                    _rightTargetHand.TargetAxis1.GetVector(),
                    _rightTargetHand.TargetAxis2.GetVector(),
                    _rightTargetHand.Axis1.GetVector(),
                    _rightTargetHand.Axis2.GetVector());

                if (avatarRoot)
                {
                    var local = avatarRoot.InverseTransformPoint(rightTarget.position);
                    local.x *= -1f;
                    leftTarget.transform.position = avatarRoot.TransformPoint(local);
                }
            }
        }



        private void DetectHandMirror(HVRPosableHand sourceHand, HVRPosableHand targetHand, Vector3 forward, Vector3 up)
        {
            // Get local orthogonal axes of the right hand pointing forward and up
            var axis1 = GetSignedAxisVectorToDirection(targetHand.transform.rotation, forward);
            var axis2 = GetSignedAxisVectorToDirection(targetHand.transform.rotation, up);

            // Mirror left hand rotation
            Quaternion mirrorL = MirrorYZ(sourceHand.transform.rotation, avatarRoot.transform.rotation);

            // Get local orthogonal mirrored axes of the left hand pointing forward and up
            var targetAxis1 = GetSignedAxisVectorToDirection(mirrorL, forward);
            var targetAxis2 = GetSignedAxisVectorToDirection(mirrorL, up);

            sourceHand.Axis1 = axis1.GetHVRAxis();
            sourceHand.Axis2 = axis2.GetHVRAxis();
            sourceHand.TargetAxis1 = targetAxis1.GetHVRAxis();
            sourceHand.TargetAxis2 = targetAxis2.GetHVRAxis();

            sourceHand.UseMatchRotation = true;
        }

        public static Quaternion MirrorYZ(Quaternion r, Quaternion space)
        {
            r = Quaternion.Inverse(space) * r;
            Vector3 forward = r * Vector3.forward;
            Vector3 up = r * Vector3.up;

            forward.x *= -1;
            up.x *= -1;

            return space * Quaternion.LookRotation(forward, up);
        }

        public static Vector3 GetSignedAxisVectorToDirection(Quaternion r, Vector3 direction)
        {
            direction = direction.normalized;
            Vector3 axis = Vector3.right;

            float dotX = Mathf.Abs(Vector3.Dot(r * Vector3.right, direction));
            float dotY = Mathf.Abs(Vector3.Dot(r * Vector3.up, direction));
            if (dotY > dotX) axis = Vector3.up;
            float dotZ = Mathf.Abs(Vector3.Dot(r * Vector3.forward, direction));
            if (dotZ > dotX && dotZ > dotY) axis = Vector3.forward;

            if (Vector3.Dot(r * axis, direction) < 0f) axis = -axis;

            return axis;
        }

        private void SetupRigRefs()
        {
            var root = avatarRoot.transform.root;
            avatarRoot.transform.ResetLocalProps(false);
            var animator = avatarRoot.gameObject.GetComponent<Animator>();
            var leftHand = root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Left);
            var rightHand = root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Right);

            var tp = root.GetComponentInChildren<HVRTeleporter>();
            if (tp)
            {
                if (avatarRoot.GetComponent<HVRVRIKTeleportHandler>() == null)
                {
                    var h = avatarRoot.gameObject.UndoEnsureComponent<HVRVRIKTeleportHandler>();
                    UnityEventTools.AddVoidPersistentListener(tp.AfterTeleport, new UnityAction(h.AfterTeleport));
                    EditorUtility.SetDirty(tp);
                }
            }


            SetupHandGrabber(leftHand, out var leftTarget);
            SetupHandGrabber(rightHand, out var rightTarget);

            var vrik = avatarRoot.GetComponent<VRIK>();
            vrik.solver.leftArm.target = leftTarget;
            vrik.solver.rightArm.target = rightTarget;


            Transform camera = null;
            var cam = root.GetComponentInChildren<HVRCamera>();
            if (cam)
            {
                camera = cam.transform;
            }
            else
            {
                var cam2 = root.GetComponentInChildren<Camera>();
                if (cam2)
                    camera = cam2.transform;
            }

            if (camera)
            {
                var headTarget = camera.Find("IKTarget");

                if (!headTarget)
                {
                    var IKTarget = new GameObject("IKTarget");
                    IKTarget.transform.parent = camera;
                    IKTarget.transform.ResetLocalProps();
                    headTarget = IKTarget.transform;
                    headTarget.transform.localPosition = new Vector3(0f, -.075f, -.1f);
                }

                if (animator)
                {
                    var head = animator.GetBoneTransform(HumanBodyBones.Head);
                    if (head)
                    {
                        headTarget.transform.rotation = head.rotation;
                    }
                }

                vrik.solver.spine.headTarget = headTarget;
            }


            EditorUtility.SetDirty(vrik);
            //so.ApplyModifiedProperties();
        }

        private void SetupHandGrabber(HVRHandGrabber hand, out Transform ikTarget)
        {
            var avatarHand = avatarRoot.GetComponentsInChildren<HVRPosableHand>().FirstOrDefault(e => e.IsLeft == (hand.HandSide == HVRHandSide.Left));

            var so = new SerializedObject(hand);
            var hover = so.FindProperty("HoverPoser");
            hover.objectReferenceValue = null;
            var grabPoser = so.FindProperty("GrabPoser");
            grabPoser.objectReferenceValue = null;

            var IKTargetName = hand.HandSide + "IKTarget";

            ikTarget = hand.transform.Find(IKTargetName);
            if (!ikTarget)
            {
                var iktarg = new GameObject(IKTargetName);
                iktarg.transform.parent = hand.transform;
                iktarg.transform.ResetLocalProps();

                ikTarget = iktarg.transform;
            }


            var handanimator = avatarHand.gameObject.GetComponent<HVRHandAnimator>();
            so.FindProperty("HandAnimator").objectReferenceValue = handanimator;
            so.FindProperty("PhysicsPoser").objectReferenceValue = avatarHand.gameObject.GetComponent<HVRPhysicsPoser>();
            so.FindProperty("InverseKinematics").boolValue = true;
            so.FindProperty("HandModel").objectReferenceValue = ikTarget;
            so.ApplyModifiedProperties();


            var soAnim = new SerializedObject(handanimator);
            soAnim.FindProperty("HandOverride").objectReferenceValue = ikTarget;
            soAnim.ApplyModifiedProperties();
            
            var fg = hand.gameObject.GetComponentInChildren<HVRForceGrabber>();
            if (fg)
            {
                so = new SerializedObject(fg);
                hover = so.FindProperty("HoverPoser");
                hover.objectReferenceValue = null;
                grabPoser = so.FindProperty("GrabPoser");
                grabPoser.objectReferenceValue = null;
                EditorUtility.SetDirty(fg);
                so.ApplyModifiedProperties();
            }

            if (hand.FallbackPoser)
            {
                so = new SerializedObject(hand.FallbackPoser);
                var blend = so.FindProperty("PrimaryPose");
                var pose = blend.FindPropertyRelative("Pose");
                pose.objectReferenceValue = null;
                so.ApplyModifiedProperties();
            }

            EditorUtility.SetDirty(hand);
        }

        HVRPosableHand SetupHand(bool left, Transform t, Animator a)
        {
            var go = t.gameObject;

            var hand = go.UndoEnsureComponent<HVRPosableHand>();
            var animator = go.UndoEnsureComponent<HVRHandAnimator>();
            var physicsPoser = go.UndoEnsureComponent<HVRPhysicsPoser>();
            var poser = go.UndoEnsureComponent<HVRHandPoser>();
            var hm = go.UndoEnsureComponent<HVRHandMirrorer>();

            var palm = t.Find("Palm");
            if (!palm)
            {
                var p = new GameObject("Palm");
                p.transform.parent = t;
                p.transform.ResetLocalProps();
                palm = p.transform;
            }

            hand.IsLeft = left;
            hand.HandMirrorer = hm;

            animator.DefaultPoseHand = false;
            animator.PhysicsPoser = physicsPoser;
            animator.Hand = hand;
            animator.DefaultPoser = poser;
            physicsPoser.Hand = hand;
            physicsPoser.Palm = palm;

            var whichHand = left ? "Left" : "Right";
            if (left)
            {
                SetupFinger($"{whichHand} Thumb", ref hand.Thumb, a, HumanBodyBones.LeftThumbProximal, HumanBodyBones.LeftThumbIntermediate, HumanBodyBones.LeftThumbDistal);
                SetupFinger($"{whichHand} Index", ref hand.Index, a, HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexDistal);
                SetupFinger($"{whichHand} Middle", ref hand.Middle, a, HumanBodyBones.LeftMiddleProximal, HumanBodyBones.LeftMiddleIntermediate, HumanBodyBones.LeftMiddleDistal);
                SetupFinger($"{whichHand} Ring", ref hand.Ring, a, HumanBodyBones.LeftRingProximal, HumanBodyBones.LeftRingIntermediate, HumanBodyBones.LeftRingDistal);
                SetupFinger($"{whichHand} Pinky", ref hand.Pinky, a, HumanBodyBones.LeftLittleProximal, HumanBodyBones.LeftLittleIntermediate, HumanBodyBones.LeftLittleDistal);
            }
            else
            {
                SetupFinger($"{whichHand} Thumb", ref hand.Thumb, a, HumanBodyBones.RightThumbProximal, HumanBodyBones.RightThumbIntermediate, HumanBodyBones.RightThumbDistal);
                SetupFinger($"{whichHand} Index", ref hand.Index, a, HumanBodyBones.RightIndexProximal, HumanBodyBones.RightIndexIntermediate, HumanBodyBones.RightIndexDistal);
                SetupFinger($"{whichHand} Middle", ref hand.Middle, a, HumanBodyBones.RightMiddleProximal, HumanBodyBones.RightMiddleIntermediate, HumanBodyBones.RightMiddleDistal);
                SetupFinger($"{whichHand} Ring", ref hand.Ring, a, HumanBodyBones.RightRingProximal, HumanBodyBones.RightRingIntermediate, HumanBodyBones.RightRingDistal);
                SetupFinger($"{whichHand} Pinky", ref hand.Pinky, a, HumanBodyBones.RightLittleProximal, HumanBodyBones.RightLittleIntermediate, HumanBodyBones.RightLittleDistal);
            }

            if (hand.Index != null)
            {
                hand.IndexRoot = hand.Index.Bones.FirstOrDefault()?.Transform;
                hand.IndexTip = hand.Index?.Tip;
            }

            if (hand.Thumb != null)
            {
                hand.ThumbRoot = hand.Thumb.Bones.FirstOrDefault()?.Transform;
                hand.ThumbTip = hand.Thumb?.Tip;
            }

            if (hand.Middle != null)
            {
                hand.MiddleRoot = hand.Middle.Bones.FirstOrDefault()?.Transform;
                hand.MiddleTip = hand.Middle?.Tip;
            }

            if (hand.Ring != null)
            {
                hand.RingRoot = hand.Ring.Bones.FirstOrDefault()?.Transform;
                hand.RingTip = hand.Ring?.Tip;
            }

            if (hand.Pinky != null)
            {
                hand.PinkyRoot = hand.Pinky.Bones.FirstOrDefault()?.Transform;
                hand.PinkyTip = hand.Pinky?.Tip;
            }

            EditorUtility.SetDirty(hand);
            EditorUtility.SetDirty(animator);
            EditorUtility.SetDirty(physicsPoser);
            EditorUtility.SetDirty(poser);

            return hand;
        }

        void SetupFinger(string text, ref HVRPosableFinger finger, Animator a, params HumanBodyBones[] bones)
        {
            finger = new HVRPosableFinger();
            finger.Bones = new List<HVRPosableBone>();
            foreach (var b in bones)
            {
                var t = a.GetBoneTransform(b);
                if (t)
                {
                    var bone = new HVRPosableBone();
                    bone.Transform = t;
                    finger.Bones.Add(bone);
                }
                else
                {
                    Debug.LogWarning($"{text} missing bone {b}");
                }
            }

            if (finger.Bones.Count > 0)
            {
                if (AutoAddTips && !HasTipsAlready)
                {
                    var last = finger.Bones.Last();
                    var existing = last.Transform.Find("Tip");

                    if (!existing)
                    {
                        var tip = new GameObject("Tip");
                        tip.transform.parent = last.Transform;
                        tip.transform.ResetLocalProps();
                        finger.Tip = tip.transform;

                        Undo.RegisterCreatedObjectUndo(tip, $"Add Tip to {last.Transform.name}");
                    }
                    else
                    {
                        finger.Tip = existing;
                    }
                }
                else
                {
                    var last = finger.Bones.Last();
                    if (HasTipsAlready && last.Transform.childCount > 0)
                    {
                        finger.Tip = last.Transform.GetChild(0);
                    }
                }

                finger.Root = finger.Bones[0].Transform;
            }
            else
            {
                finger = null;
            }
        }

        private Transform Add(Transform parent, string name)
        {
            var existing = parent.Find(name);

            if (!existing)
            {
                var tip = new GameObject(name);
                tip.transform.parent = parent;
                tip.transform.ResetLocalProps();
                existing = tip.transform;
                Undo.RegisterCreatedObjectUndo(tip, $"Add {name} to {parent.name}");
                Debug.Log($"{tip.name} added to {parent.name}");
            }

            return existing;
        }
    }
}