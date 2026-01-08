using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// Asegúrate de que detecta tu librería, si está en namespace pon: using QuaternionUtility; 
// Si no tiene namespace, no hace falta.

public class MissionController : MonoBehaviour
{
    [Header("Referencias Principales")]
    public Transform player;
    public Transform ikTarget;

    [Header("Objetivos")]
    public List<GameObject> objectives = new List<GameObject>();
    public float activationDistance = 10.0f;

    [Header("Configuración de Movimiento")]
    [Range(0.1f, 20f)]
    public float moveSpeed = 5.0f; // <--- NUEVO: Velocidad de movimiento del target

    [Header("Visuales")]
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.gray;

    private int currentIndex = 0;
    private bool isMissionComplete = false;

    [Header("Scene")]
    public string sceneName;

    void Start()
    {
        UpdateObjectiveVisuals();
    }

    void Update()
    {
        if (isMissionComplete || objectives.Count == 0) return;

        GameObject currentObj = objectives[currentIndex];

        // 1. Convertir posiciones a TUS vectores para calcular distancia
        VectorUtils3D playerPos = VectorUtils3D.ToVectorUtils3D(player.position);
        VectorUtils3D objectivePos = VectorUtils3D.ToVectorUtils3D(currentObj.transform.position);

        float distanceToObjective = VectorUtils3D.Distance(playerPos, objectivePos);

        // 2. Determinar el DESTINO deseado (A dónde queremos ir)
        Vector3 desiredPositionUnity;

        if (distanceToObjective <= activationDistance)
        {
            // Queremos ir al objetivo
            desiredPositionUnity = currentObj.transform.position;
        }
        else
        {
            // Queremos ir a la posición de reposo (junto al jugador)
            desiredPositionUnity = player.position + Vector3.up * 1.5f + player.forward * 0.5f;
        }

        // 3. MOVER SUAVEMENTE (Usando tu librería matemática)

        // Convertimos la posición actual del Target y el destino a tu sistema
        VectorUtils3D currentPosUtils = VectorUtils3D.ToVectorUtils3D(ikTarget.position);
        VectorUtils3D desiredPosUtils = VectorUtils3D.ToVectorUtils3D(desiredPositionUnity);

        // Usamos TU función LERP
        // t = Time.deltaTime * moveSpeed (hace que sea suave e independiente de los FPS)
        float t = Time.deltaTime * moveSpeed;

        // Evitamos que t sea mayor que 1 para que no se pase
        if (t > 1.0f) t = 1.0f;

        VectorUtils3D smoothedPos = VectorUtils3D.LERP(currentPosUtils, desiredPosUtils, t);

        // 4. Aplicar el resultado al objeto de Unity
        ikTarget.position = new Vector3(smoothedPos.x, smoothedPos.y, smoothedPos.z);
    }

    public void CompleteCurrentObjective()
    {
        if (currentIndex < objectives.Count - 1)
        {
            currentIndex++;
            UpdateObjectiveVisuals();
            Debug.Log("Objetivo completado. Siguiente: " + currentIndex);
        }
        else
        {
            isMissionComplete = true;
            Debug.Log("¡Todos los objetivos completados!");

            Renderer rend = objectives[currentIndex].GetComponent<Renderer>();
            if (rend != null) rend.material.color = inactiveColor;

            SceneManager.LoadScene(sceneName);

            // Al acabar, forzamos que el destino sea el jugador (se actualizará en el Update)
            // No hace falta poner ikTarget.position = player.position aquí porque el Update lo hará suavemente.
        }
    }

    private void UpdateObjectiveVisuals()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            Renderer rend = objectives[i].GetComponent<Renderer>();
            if (rend != null)
            {
                if (i == currentIndex)
                {
                    rend.material.color = activeColor;
                    rend.material.EnableKeyword("_EMISSION");
                    rend.material.SetColor("_EmissionColor", activeColor);
                }
                else
                {
                    rend.material.color = inactiveColor;
                    rend.material.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}