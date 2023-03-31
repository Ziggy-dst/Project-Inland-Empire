using HurricaneVR.Framework.Core.HandPoser;
using HurricaneVR.Framework.Shared;
using RootMotion.FinalIK;
using UnityEngine;

namespace HurricaneVR.Framework.Integrations.FinalIK
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(VRIK))]
    [RequireComponent(typeof(HVRIKTargets))]
    public class HVREditorIK : MonoBehaviour
    {
        public Transform LeftTarget => Targets.LeftTarget;
        public Transform RightTarget => Targets.RightTarget;

        public HVRIKTargets Targets;

        public bool ForceEditorIK;

        private VRIK ik;

        void Start()
        {
            ik = GetComponent<VRIK>();
            Targets = GetComponent<HVRIKTargets>();

            if (Application.isPlaying)
            {
                ForceEditorIK = false;
                enabled = false;
                return;
            }


            if (Targets.IsPoser)
            {
                ik.solver.SetToReferences(ik.references);
                ik.solver.Initiate(ik.transform);
                ik.solver.locomotion.weight = 0f;
                ik.solver.spine.positionWeight = 0f;
                ik.solver.spine.rotationWeight = 0f;
                ik.solver.spine.headTarget = null;
            }
        }

        void OnEnable()
        {
            Start();
        }

        void Update()
        {
            if (Application.isPlaying || !Targets || !Targets.IsPoser)
            {
                if (!ForceEditorIK)
                    return;
            }

            if (!Targets)
            {
                if (!TryGetComponent(out Targets))
                    return;
            }

            if (ik.fixTransforms)
                ik.solver.FixTransforms();

            if (!ik.solver.initiated)
            {
                ik.solver.SetToReferences(ik.references);
                ik.solver.Initiate(ik.transform);
            }

            if (Targets.IsPoser)
            {
                ik.solver.leftArm.target = LeftTarget;
                ik.solver.rightArm.target = RightTarget;
            }

            ik.solver.Update();
        }

#if UNITY_EDITOR

        [InspectorButton("HandToTarget")]
        public string SolveHands;

        public void HandToTarget()
        {
            if (!ik.solver.initiated)
            {
                ik.solver.SetToReferences(ik.references);
                ik.solver.Initiate(ik.transform);
            }

            ik.solver.Update();
        }

#endif
    }
}