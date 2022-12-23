using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Label de marcador 1
    // Label de marcador 2
    // GameManager restartea la escena

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Goal")){
            Debug.Log("GOOOOOOOOOL");
            string sensor = other.gameObject.name;
            //AddGoal
            //Esperar 3 segundos con un texto
            //Restartear posiciones
            Debug.Log(other.gameObject.name);
        }
    }

    private void AddGoal(string sensor) {
        if(sensor=="Sensor1") {
            //El equipo 2 ha marcado en la portería 1
            //addGoal equipo 2
        } else if (sensor=="Sensor2"){
            //El equipo 1 ha marcado en la portería 2
            //addGoal equipo 1
        }
    }

     IEnumerator WaitToDo(){
        yield return new WaitForSeconds(5.0f); // wait 5 seconds to "Do Something"
        print("Do Something");
    }
}
