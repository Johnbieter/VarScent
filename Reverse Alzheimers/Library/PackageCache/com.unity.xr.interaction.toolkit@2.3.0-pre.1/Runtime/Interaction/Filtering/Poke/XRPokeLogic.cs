using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using Unity.XR.CoreUtils.Bindings.Variables;

namespace UnityEngine.XR.Interaction.Toolkit.Filtering
{
    /// <summary>
    /// Class that encapsulates the logic for evaluating whether an interactable has been poked or not
    /// through enter and exit thresholds.
    /// </summary>
    class XRPokeLogic : IDisposable
    {
        /// <summary>
        /// Length of interaction axis computed from the attached collider bounds and configured interaction direction.
        /// </summary>
        float interactionAxisLength { get; set; } = 1f;

        readonly BindableVariable<PokeStateData> m_PokeStateData = new BindableVariable<PokeStateData>();

        /// <summary>
        /// Bindable variable that updates whenever the poke logic state is evaluated.
        /// </summary>
        public IReadOnlyBindableVariable<PokeStateData> pokeStateData => m_PokeStateData;

        /// <summary>
        /// Squared value of <see cref="interactionAxisLength"/>.
        /// </summary>
        float squaredInteractionAxisLength { get; set; } = 1f;

        Transform m_InitialTransform;
        PokeThresholdData m_PokeThresholdData;
        float m_SelectEntranceVectorDotThreshold;

        readonly Dictionary<object, Pose> m_LastHoverEnterPose = new Dictionary<object, Pose>();
        readonly Dictionary<object, Transform> m_LastHoverTransform = new Dictionary<object, Transform>();

        /// <summary>
        /// Initializes <see cref="XRPokeLogic"/> with properties calculated from the collider of the associated interactable.
        /// </summary>
        /// <param name="associatedTransform"><see cref="Transform"/> object used for poke calculations.</param>
        /// <param name="pokeThresholdData"><see cref="PokeThresholdData"/> object containing the specific poke parameters used for calculating
        /// whether or not the current interaction meets the requirements for poke hover or select.</param>
        /// <param name="collider"><see cref="Collider"/> for computing the interaction axis length used to detect if poke depth requirements are met.</param>
        public void Initialize(Transform associatedTransform, PokeThresholdData pokeThresholdData, Collider collider)
        {
            m_InitialTransform = associatedTransform;
            m_PokeThresholdData = pokeThresholdData;
            m_SelectEntranceVectorDotThreshold = pokeThresholdData.GetSelectEntranceVectorDotThreshold();

            if (collider != null)
            {             
                interactionAxisLength = ComputeInteractionAxisLength(ComputeBounds(collider));
                squaredInteractionAxisLength = interactionAxisLength * interactionAxisLength;
            }
            ResetPokeStateData(m_InitialTransform);
        }

        /// <summary>
        /// This method will reset the underlying interaction length used to determine if the current poke depth has been reached. This is typically
        /// used on UI objects, or objects where poke depth is not appropriately defined by the collider bounds of the object.
        /// </summary>
        /// <param name="pokeDepth">A value representing the poke depth required to meet requirements for select.</param>
        public void SetPokeDepth(float pokeDepth)
        {
            interactionAxisLength = pokeDepth;
            squaredInteractionAxisLength = pokeDepth * pokeDepth;
        }

        /// <summary>
        /// Clears cached data hover enter pose data.
        /// </summary>
        public void Dispose()
        {
            m_LastHoverEnterPose.Clear();
        }

        /// <summary>
        /// Logic to check if the attempted poke interaction meets the requirements for a select action.
        /// </summary>
        /// <param name="interactor">The interactor that is a candidate for selection.</param>
        /// <param name="pokableAttachPosition">The attach transform position of the pokable object, typically an interactable object.</param>
        /// <param name="pokerAttachPosition">The attach transform position for the interactor.</param>
        /// <param name="pokeInteractionOffset">An additional offset that will be applied to the calculation for the depth required to meet requirements for selection.</param>
        /// <param name="pokedTransform">The target Transform that is being poked.</param>
        /// <returns>
        /// Returns <see langword="true"/> if interaction meets requirements for select action.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        public bool MeetsRequirementsForSelectAction(object interactor, Vector3 pokableAttachPosition, Vector3 pokerAttachPosition, float pokeInteractionOffset, Transform pokedTransform)
        {
            if (m_PokeThresholdData == null || pokedTransform == null)
                return false;

            Vector3 axisNormal = ComputeRotatedDepthEvaluationAxis(pokedTransform);

            // Move interaction point towards the target, along the interaction normal, by the determined offset.
            float combinedOffset = pokeInteractionOffset + m_PokeThresholdData.interactionDepthOffset;
            Vector3 toleranceOffset = axisNormal * combinedOffset;
            Vector3 interactionPoint = pokerAttachPosition - toleranceOffset;

            Vector3 interactionPointOffset = pokableAttachPosition - interactionPoint;
            Vector3 axisAlignedInteractionPointOffset = Vector3.Project(interactionPointOffset, axisNormal);
            float squaredMagnitudeInteractionDepth = axisAlignedInteractionPointOffset.sqrMagnitude;

            float depthPercent = Mathf.Clamp01(squaredMagnitudeInteractionDepth / squaredInteractionAxisLength);

            // Compare with hover pose, to ensure interaction started on the right side of the interaction bounds
            bool meetsHoverRequirements = true;
            if (m_PokeThresholdData.enablePokeAngleThreshold && m_LastHoverEnterPose.TryGetValue(interactor, out var hoverPose))
            {
                Vector3 hoverInteractionPointOffset = (hoverPose.position - pokableAttachPosition).normalized;
                meetsHoverRequirements = Vector3.Dot(hoverInteractionPointOffset, axisNormal) > m_SelectEntranceVectorDotThreshold;
            }

            // Either depth lines up, or we've moved passed the goal post
            bool meetsRequirements = meetsHoverRequirements && (depthPercent < 0.01f
                                                                || Vector3.Dot(interactionPointOffset.normalized, axisNormal) > 0f);

            // Remove offset from visual callback to better match the actual poke position.
            var offsetRemoval = depthPercent < 1f && !meetsRequirements ? combinedOffset : 0f;

            // Update poke state data for affordances
            var axisDepth = meetsRequirements ? 0f : meetsHoverRequirements ? Mathf.Max(depthPercent, 0f) : 1f;
            var clampedPokeDepth = Mathf.Clamp(axisDepth * interactionAxisLength + offsetRemoval, 0f, interactionAxisLength);
            
            m_PokeStateData.Value = new PokeStateData
            {
                meetsRequirements = meetsRequirements,
                pokeInteractionPoint = pokerAttachPosition,
                axisAlignedPokeInteractionPoint = pokableAttachPosition + clampedPokeDepth * axisNormal,
                interactionStrength = 1f - Mathf.Clamp01(Mathf.Max(0f, depthPercent)),
                target = pokedTransform,
            };

            return meetsRequirements;
        }

        /// <summary>
        /// Computes the direction of the interaction axis, as configured with the poke threshold data.
        /// </summary>
        /// <param name="associatedTransform">This represents the Transform used to determine the evaluation axis along the specified poke axis.</param>
        /// <param name="isWorldSpace">World space uses the current interactable rotation, local space takes basic vector directions.</param>
        /// <returns>Normalized vector along the axis of interaction.</returns>
        Vector3 ComputeRotatedDepthEvaluationAxis(Transform associatedTransform, bool isWorldSpace = true)
        {
            if (m_PokeThresholdData == null || associatedTransform == null)
                return Vector3.zero;

            Vector3 rotatedDepthEvaluationAxis = Vector3.zero;
            switch (m_PokeThresholdData.pokeDirection)
            {
                case PokeAxis.X:
                case PokeAxis.NegativeX:
                    rotatedDepthEvaluationAxis = isWorldSpace ? associatedTransform.right : Vector3.right;
                    break;
                case PokeAxis.Y:
                case PokeAxis.NegativeY:
                    rotatedDepthEvaluationAxis = isWorldSpace ? associatedTransform.up : Vector3.up;
                    break;
                case PokeAxis.Z:
                case PokeAxis.NegativeZ:
                    rotatedDepthEvaluationAxis = isWorldSpace ? associatedTransform.forward : Vector3.forward;
                    break;
            }

            switch (m_PokeThresholdData.pokeDirection)
            {
                case PokeAxis.X:
                case PokeAxis.Y:
                case PokeAxis.Z:
                    rotatedDepthEvaluationAxis = -rotatedDepthEvaluationAxis;
                    break;
            }

            return rotatedDepthEvaluationAxis;
        }

        float ComputeInteractionAxisLength(Bounds bounds)
        {
            if (m_PokeThresholdData == null || m_InitialTransform == null)
                return 0f;

            Vector3 boundsSize = bounds.size;

            Vector3 center = m_InitialTransform.position;

            float lengthOfInteractionAxis = 0f;
            float centerOffsetLength;

            switch (m_PokeThresholdData.pokeDirection)
            {
                case PokeAxis.X:
                case PokeAxis.NegativeX:
                    centerOffsetLength = bounds.center.x - center.x;
                    lengthOfInteractionAxis = boundsSize.x / 2f + centerOffsetLength;
                    break;
                case PokeAxis.Y:
                case PokeAxis.NegativeY:
                    centerOffsetLength = bounds.center.y - center.y;
                    lengthOfInteractionAxis = boundsSize.y / 2f + centerOffsetLength;
                    break;
                case PokeAxis.Z:
                case PokeAxis.NegativeZ:
                    centerOffsetLength = bounds.center.z - center.z;
                    lengthOfInteractionAxis = boundsSize.z / 2f + centerOffsetLength;
                    break;
            }

            return lengthOfInteractionAxis;
        }

        /// <summary>
        /// Logic for caching pose when an <see cref="IXRInteractor"/> enters a hover state.
        /// </summary>
        /// <param name="interactor">The XR Interactor associated with the hover enter event interaction.</param>
        /// <param name="updatedPose">The pose of the interactor's attach transform, in world space.</param>
        public void OnHoverEntered(object interactor, Pose updatedPose, Transform pokedTransform)
        {
            if (m_LastHoverTransform.TryGetValue(interactor, out var lastTransform) && pokedTransform != lastTransform)
            {
                m_LastHoverEnterPose.Remove(interactor);
                m_LastHoverTransform[interactor] = pokedTransform;
            }
            else if (m_LastHoverEnterPose.TryGetValue(interactor, out _))
                return;

            m_LastHoverEnterPose.Add(interactor, updatedPose);
        }

        /// <summary>
        /// Logic to update poke state data when interaction terminates.
        /// </summary>
        /// <param name="interactor">The XR Interactor associated with the hover exit event interaction.</param>
        public void OnHoverExited(object interactor)
        {
            m_LastHoverEnterPose.Remove(interactor);
            if (m_LastHoverEnterPose.Count < 1)
            {
                if (m_LastHoverTransform.TryGetValue(interactor, out var lastTransform))
                {
                    ResetPokeStateData(lastTransform);
                    m_LastHoverTransform.Remove(interactor);
                }
                else
                {
                    ResetPokeStateData(m_InitialTransform);
                }
            }
        }

        void ResetPokeStateData(Transform transform)
        {
            if (transform == null)
                return;

            var startPos = transform.position;
            var axisExtent = startPos + ComputeRotatedDepthEvaluationAxis(transform) * interactionAxisLength;

            m_PokeStateData.Value = new PokeStateData
            {
                meetsRequirements = false,
                pokeInteractionPoint = axisExtent,
                axisAlignedPokeInteractionPoint = axisExtent,
                interactionStrength = 0f,
                target = null,
            };
        }

        static Bounds ComputeBounds(Collider targetCollider, bool rotateBoundsScale = false, Space targetSpace = Space.World)
        {
            Bounds newBounds = default;
            if (targetCollider is BoxCollider boxCollider)
            {
                newBounds = new Bounds(boxCollider.center, boxCollider.size);
            }
            else if (targetCollider is SphereCollider sphereCollider)
            {
                newBounds = new Bounds(sphereCollider.center, Vector3.one * (sphereCollider.radius * 2));
            }
            else if (targetCollider is CapsuleCollider capsuleCollider)
            {
                Vector3 targetSize = Vector3.zero;
                float diameter = capsuleCollider.radius * 2f;
                float fullHeight = capsuleCollider.height;
                switch (capsuleCollider.direction)
                {
                    // X
                    case 0:
                        targetSize = new Vector3(fullHeight, diameter, diameter);
                        break;
                    // Y
                    case 1:
                        targetSize = new Vector3(diameter, fullHeight, diameter);
                        break;
                    // Z
                    case 2:
                        targetSize = new Vector3(diameter, diameter, fullHeight);
                        break;
                }

                newBounds = new Bounds(capsuleCollider.center, targetSize);
            }

            if (targetSpace == Space.Self)
                return newBounds;

            return BoundsLocalToWorld(newBounds, targetCollider.transform, rotateBoundsScale);
        }

        static Bounds BoundsLocalToWorld(Bounds targetBounds, Transform targetTransform, bool rotateBoundsScale = false)
        {
            Vector3 localScale = targetTransform.localScale;
            Vector3 adjustedSize = localScale.Multiply(targetBounds.size);
            // $TODO resolve issues where collider compresses to zero -> Find better approach for rotation
            Vector3 rotatedSize = rotateBoundsScale ? targetTransform.rotation * adjustedSize : adjustedSize;
            return new Bounds(targetTransform.position + localScale.Multiply(targetBounds.center), rotatedSize);
        }

        /// <summary>
        /// Logic for drawing gizmos in the editor that visualize the collider bounds and vector through which a poke
        /// interaction will be evaluated for interactables that support poke.
        /// </summary>
        public void DrawGizmos()
        {
            if (m_PokeThresholdData == null || m_InitialTransform == null)
                return;

            Vector3 interactionOrigin = m_InitialTransform.position;
            var interactionNormal = ComputeRotatedDepthEvaluationAxis(m_InitialTransform);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(interactionOrigin, interactionOrigin + interactionNormal * interactionAxisLength);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(interactionOrigin, interactionOrigin + interactionNormal * m_PokeThresholdData.interactionDepthOffset);

            if (m_PokeStateData != null && m_PokeStateData.Value.interactionStrength > 0f)
            {
                Gizmos.color = m_PokeStateData.Value.meetsRequirements ? Color.green : Color.yellow;
                Gizmos.DrawWireSphere(m_PokeStateData.Value.pokeInteractionPoint, 0.01f);
                Gizmos.DrawWireSphere(m_PokeStateData.Value.axisAlignedPokeInteractionPoint, 0.01f);
            }
        }
    }
}