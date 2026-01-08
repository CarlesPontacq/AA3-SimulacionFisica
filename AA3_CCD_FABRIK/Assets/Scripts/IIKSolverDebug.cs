public interface IIKSolverDebug
{
    string AlgorithmName { get; }
    int LastFrameIterations { get; }
    float CurrentDistanceToTarget { get; }
}
