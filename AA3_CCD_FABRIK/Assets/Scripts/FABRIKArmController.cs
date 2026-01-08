using UnityEngine;
using QuaternionUtility;
using NUnit.Framework;
using System.Collections.Generic;

public class FABRIKArmController : MonoBehaviour
{
    [SerializeField] private FABRIKSolver solver;
    [SerializeField] private List<Transform> buttons;
    [SerializeField] private Transform defaultTarget;
    void Awake()
    {
        VectorUtils3D playerPos = VectorUtils3D.ToVectorUtils3D(transform.position);

        VectorUtils3D armPos = VectorUtils3D.ToVectorUtils3D(solver.transform.position);

        VectorUtils3D localOffset = armPos - playerPos;

        solver.SetLocalOffset(localOffset);
        solver.SetTarget(defaultTarget);
    }

    void Update()
    {
        VectorUtils3D playerPos = VectorUtils3D.ToVectorUtils3D(transform.position);

        QuaternionUtils playerRot = new QuaternionUtils();
        playerRot.AssignFromUnityQuaternion(transform.rotation);

        solver.UpdateRootJoint(playerPos, playerRot);
    }
}
