using System.Collections.Generic;
using UnityEngine;
using QuaternionUtility;

public class FABRIKSolver : MonoBehaviour
{
    [SerializeField] private List<Transform> jointTransforms;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float tolerance = 0.01f;
    [SerializeField] private float smoothSpeed = 10f;

    private List<VectorUtils3D> joints;
    private float[] links;

    private VectorUtils3D rootPosition;
    private VectorUtils3D previousRootPosition;
    private VectorUtils3D target;

    private VectorUtils3D positionOffset;

    void Start()
    {
        joints = GetTransformListAsVectorUtils3DList(jointTransforms);

        links = new float[joints.Count - 1];
        for (int i = 0; i < joints.Count - 1; i++)
        {
            links[i] = VectorUtils3D.Distance(joints[i + 1], joints[i]);
        }

        rootPosition = joints[0];
        previousRootPosition = rootPosition;
    }

    void Update()
    {
        UpdateTargetPosition();

        bool targetReached = VectorUtils3D.Distance(joints[joints.Count - 1], target) < tolerance;
        bool rootMoved = VectorUtils3D.Distance(rootPosition, previousRootPosition) > 0.0001f;

        if (!targetReached || rootMoved)
        {
            ForwardSolve();
            BackwardSolve();
            UpdateUnityTransforms();
        }

        previousRootPosition = rootPosition;
    }

    void UpdateTargetPosition()
    {
        target = VectorUtils3D.ToVectorUtils3D(targetTransform.position);
    }

    void ForwardSolve()
    {
        joints[joints.Count - 1] =
            VectorUtils3D.LERP(joints[joints.Count - 1], target, smoothSpeed * Time.deltaTime);

        for (int i = joints.Count - 2; i >= 0; i--)
        {
            float dist = links[i];
            float currentDist = VectorUtils3D.Distance(joints[i], joints[i + 1]);
            float lambda = dist / currentDist;

            VectorUtils3D dir = joints[i + 1] - joints[i];
            joints[i] = joints[i] + dir * (1f - lambda);
        }
    }

    void BackwardSolve()
    {
        joints[0] = rootPosition;

        for (int i = 1; i < joints.Count; i++)
        {
            float dist = links[i - 1];
            float currentDist = VectorUtils3D.Distance(joints[i - 1], joints[i]);
            float lambda = dist / currentDist;

            VectorUtils3D dir = joints[i - 1] - joints[i];
            joints[i] = joints[i] + dir * (1f - lambda);
        }
    }

    void UpdateUnityTransforms()
    {
        for (int i = 0; i < joints.Count; i++)
        {
            jointTransforms[i].position = joints[i].GetAsUnityVector();

            if (i < joints.Count - 1)
            {
                // Dirección del hueso hacia el siguiente
                VectorUtils3D dir = joints[i + 1] - joints[i];

                if (dir.Magnitude() > 0.0001f)
                {
                    dir = dir.Normalize();

                    VectorUtils3D forward = VectorUtils3D.forward;
                    VectorUtils3D axis = forward.CrossProduct3D(dir);
                    float angle = forward.Angle(dir) * (MathFUtils.PI / 180f); // Convertir a rad

                    QuaternionUtils rot = new QuaternionUtils();
                    if (axis.Magnitude() > 0.0001f)
                    {
                        axis = axis.Normalize();
                        rot = rot.AngleToQuaternion(axis, angle);
                    }
                    else
                    {
                        // La dirección ya coincide con forward, quaternion identidad
                        rot = new QuaternionUtils();
                    }

                    jointTransforms[i].rotation = rot.GetAsUnityQuaternion();
                }
            }
        }
    }

    List<VectorUtils3D> GetTransformListAsVectorUtils3DList(List<Transform> transformsList)
    {
        List<VectorUtils3D> list = new List<VectorUtils3D>();
        foreach (Transform t in transformsList)
        {
            list.Add(VectorUtils3D.ToVectorUtils3D(t.position));
        }
        return list;
    }

    public void SetLocalOffset(VectorUtils3D posOffset)
    {
        positionOffset = posOffset;
    }

    public void UpdateRootJoint(VectorUtils3D playerPos, QuaternionUtils playerRot)
    {
        VectorUtils3D rotatedOffset = playerRot.RotateVector(positionOffset);

        rootPosition = playerPos + rotatedOffset;
        joints[0] = rootPosition;
    }

    public void SetTarget(Transform newTarget)
    {
        targetTransform = newTarget;
        UpdateTargetPosition();
    }
}
