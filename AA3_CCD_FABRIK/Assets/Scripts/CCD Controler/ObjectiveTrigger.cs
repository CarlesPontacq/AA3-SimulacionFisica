using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [Header("Configuración")]
    public MissionController missionController; 
    string endEffectorTag = "EndEffector"; 

    private bool hasBeenHacked = false;

    
    private void OnTriggerEnter(Collider other)
    {
        
        if (hasBeenHacked) return;

        
        if (other.CompareTag(endEffectorTag))
        {
            

            Debug.Log("¡Hackeo Exitoso!");
            hasBeenHacked = true;

            
            if (missionController != null)
            {
                missionController.CompleteCurrentObjective();
            }

            
            GetComponent<Collider>().enabled = false;
        }
    }
}
