using UnityEngine;
using System.Collections.Generic;
using QuaternionUtility;

public class CCDSolver : MonoBehaviour
{
    [Header("Configuración")]
    public Transform rootObject;      
    public Transform targetObject;    
    public List<Transform> boneTransforms = new List<Transform>(); 
    // El último elemento de la lista se considera el que tiene el "End Effector"

    [Header("Parámetros (Ajustables por Jugador)")]
    [Range(1, 50)]
    public int iterations = 10;
    [Range(0.001f, 1f)]
    public float tolerance = 0.05f;

    private class VirtualBone
    {
        public VectorUtils3D position;      
        public QuaternionUtils orientation; 
        public float length;                
    }

    private List<VirtualBone> virtualBones = new List<VirtualBone>();
    private QuaternionUtils qUtils = new QuaternionUtils(); 

    void Start()
    {
        InitializeVirtualChain();
    }

    void Update()
    {
        // Actualizar la base
        if (virtualBones.Count > 0 && rootObject != null)
        {
            virtualBones[0].position = VectorUtils3D.ToVectorUtils3D(rootObject.position);
        }

        // Ejecutar algoritmo CCD
        SolveCCD();

        
        ApplyToUnityTransforms();
    }

    /// <summary>
    /// Lee la configuración inicial para medir las longitudes de los huesos.
    /// </summary>
    private void InitializeVirtualChain()
    {
        virtualBones.Clear();

        for (int i = 0; i < boneTransforms.Count; i++)
        {
            VirtualBone bone = new VirtualBone();

            // Posicion inicial
            bone.position = VectorUtils3D.ToVectorUtils3D(boneTransforms[i].position);

            // Rotacion inicial
            bone.orientation = new QuaternionUtils();
            bone.orientation.AssignFromUnityQuaternion(boneTransforms[i].rotation);

            // Calcular longitud hacia el siguiente hueso
            if (i < boneTransforms.Count - 1)
            {
                VectorUtils3D nextPos = VectorUtils3D.ToVectorUtils3D(boneTransforms[i + 1].position);
                bone.length = VectorUtils3D.Distance(bone.position, nextPos);
            }
            else
            {
                bone.length = 0; // El último hueso
            }

            virtualBones.Add(bone);
        }
    }

    private void SolveCCD()
    {
        if (virtualBones.Count == 0 || targetObject == null) return;

        VectorUtils3D targetPos = VectorUtils3D.ToVectorUtils3D(targetObject.position);

        // Bucle de Iteraciones
        for (int iter = 0; iter < iterations; iter++)
        {
            //asegurar que las posiciones están actualizadas (Cinemática Directa)
            
            UpdateForwardKinematics();

            //Comprobar si ya llegamos
            VectorUtils3D endEffectorPos = virtualBones[virtualBones.Count - 1].position;
            if (VectorUtils3D.Distance(endEffectorPos, targetPos) < tolerance)
            {
                break;
            }

            //Recorrer huesos desde el penúltimo hasta la base
            // (El último hueso es el end efector, no rota sobre si mismo para acercarse, lo mueven sus padres)
            for (int i = virtualBones.Count - 2; i >= 0; i--)
            {
                // Recalcular posiciones (FK)
                
                UpdateForwardKinematicsFrom(i);

                VirtualBone pivotBone = virtualBones[i];
                VirtualBone endBone = virtualBones[virtualBones.Count - 1];

                VectorUtils3D pivotPos = pivotBone.position;
                VectorUtils3D tipPos = endBone.position;

                // Objetivo actual
                targetPos = VectorUtils3D.ToVectorUtils3D(targetObject.position);

                
                VectorUtils3D r_current = tipPos - pivotPos;
                VectorUtils3D r_target = targetPos - pivotPos;

                r_current = r_current.Normalize();
                r_target = r_target.Normalize();

                // Calcular Rotación
                float dot = r_current.DotProduct(r_target);

                if (dot < 0.9999f)
                {
                    // Clamp
                    if (dot > 1f) dot = 1f;
                    if (dot < -1f) dot = -1f;

                    float angle = System.MathF.Acos(dot); // Radianes

                    // Eje de rotación
                    VectorUtils3D axis = r_current.CrossProduct3D(r_target);
                    axis = axis.Normalize();

                    // Crear Cuaternión Delta
                    QuaternionUtils deltaRot = qUtils.AngleToQuaternion(axis, angle);

                    // Aplicar Rotación: newRot = Delta * Actual
                    // Al ser coordenadas globales, multiplicamos por la izquierda.
                    deltaRot.Multiply(pivotBone.orientation);
                    pivotBone.orientation = deltaRot; // Actualizamos el virtualBone

                }
            }
        }

        // Una última pasada de FK para asegurar que las posiciones finales son coherentes con las rotaciones
        UpdateForwardKinematics();
    }

    /// <summary>
    /// Cinemática Directa (FK): Calcula las posiciones (x,y,z) de todos los huesos
    /// basándose en la posición del padre, la rotación del padre y la longitud del hueso.
    /// Esto reemplaza la jerarquía de Unity.
    /// </summary>
    private void UpdateForwardKinematics()
    {
        UpdateForwardKinematicsFrom(0);
    }

    private void UpdateForwardKinematicsFrom(int startIndex)
    {
        // Si empezamos desde 0, aseguramos que la base está en su sitio
        if (startIndex == 0 && rootObject != null)
        {
            virtualBones[0].position = VectorUtils3D.ToVectorUtils3D(rootObject.position);
        }

        // Recorremos hacia adelante aplicando: Pos_Hijo = Pos_Padre + (Rot_Padre * Vect_Long)
        for (int i = startIndex; i < virtualBones.Count - 1; i++)
        {
            VirtualBone current = virtualBones[i];
            VirtualBone next = virtualBones[i + 1];

            // Para simplificar, usamos la lógica de que la rotación actual apunta hacia el siguiente hijo.

            //Tomamos un vector (0,0,1) o (0,1,0), lo rotamos por la orientación y escalamos.
            // Asumiremos que el eje principal es Forward (0,0,1).
            
            VectorUtils3D localDir = new VectorUtils3D(0, 0, current.length); // Asumiendo eje Z (Forward)
            
            VectorUtils3D rotatedOffset = current.orientation.RotateVector(localDir);

            // Posición siguiente = Pos Actual + Offset Rotado
            next.position = current.position + rotatedOffset;
        }
    }

    /// <summary>
    /// Aplica los cálculos matemáticos a la visualización de Unity.
    /// </summary>
    private void ApplyToUnityTransforms()
    {
        for (int i = 0; i < virtualBones.Count; i++)
        {
            boneTransforms[i].rotation = virtualBones[i].orientation.ToUnityQuaternion();
            boneTransforms[i].position = new Vector3(
                virtualBones[i].position.x,
                virtualBones[i].position.y,
                virtualBones[i].position.z
            );
        }
    }

  
}
