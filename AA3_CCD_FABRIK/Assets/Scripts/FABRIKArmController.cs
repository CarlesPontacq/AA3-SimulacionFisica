using UnityEngine;
using QuaternionUtility;

public class FABRIKArmController : MonoBehaviour
{
    [SerializeField] private FABRIKSolver solver;

    void Awake()
    {
        VectorUtils3D playerPos = VectorUtils3D.ToVectorUtils3D(transform.position);

        VectorUtils3D armPos = VectorUtils3D.ToVectorUtils3D(solver.transform.position);

        VectorUtils3D localOffset = armPos - playerPos;

        solver.SetLocalOffset(localOffset);
    }

    void Update()
    {
        VectorUtils3D playerPos = VectorUtils3D.ToVectorUtils3D(transform.position);

        QuaternionUtils playerRot = new QuaternionUtils();
        playerRot.AssignFromUnityQuaternion(transform.rotation);

        solver.UpdateRootJoint(playerPos, playerRot);
    }
}
