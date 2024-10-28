using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vuforia;

public class Move : MonoBehaviour
{
	public GameObject model;
	public ObserverBehaviour[] ImageTargets;
	public int currentTarget;
	public float speed = 1.0f;
	private bool isMoving = false;
    // Start is called before the first frame update
    
	public void moveToNextMarker(){
		if(!isMoving){
			StartCoroutine(MoveModel());	
		}
	}
	
	private IEnumerator MoveModel(){
		isMoving = true;
		ObserverBehaviour target = GetNextDetectedTarget();	
		if(target == null){
			isMoving = false;
			yield break;
		}
		
		Vector3 startPosition = model.transform.position;
		Vector3 endPosition = target.transform.position;	
		
		float journey = 0.0f;
		
		while(journey <= 1.0f){
			journey += Time.deltaTime * speed;
			model.transform.position = Vector3.Lerp(startPosition, endPosition, journey);
			yield return null;
		}

		currentTarget = (currentTarget+1)%ImageTargets.Length;	
		isMoving = false;
	}

	private ObserverBehaviour GetNextDetectedTarget(){
        // Verificar que el próximo marcador sea detectado
        if (currentTarget < ImageTargets.Length)
        {
            ObserverBehaviour target = ImageTargets[currentTarget];

            if (target != null && (target.TargetStatus.Status == Status.TRACKED || target.TargetStatus.Status == Status.EXTENDED_TRACKED))
            {
                return target; // Si el siguiente en la secuencia está detectado, muévete a él
            }
        }
        return null; // Si el siguiente en la secuencia no está detectado, no te muevas
    }	
    // Update is called once per frame
    void Update()
    {
        
    }
}
