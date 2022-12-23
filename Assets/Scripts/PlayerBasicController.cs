using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicController : MonoBehaviour
{
    private CharacterController characterController;
    private Transform characterTransform;
    public Camera camera;
    public float speed = 3f;
    public float gravity = 10f;
    public float jumpHeight = 5f;
    public float fallVelocity;
    public float range = 5;
    public float strength = 6000f;
    public float maxPower = 6000f;
    public float minPower = 2000f;
    public float medPower = 4000f;
    public GameObject greenImg;
    public GameObject yellowImg;
    public GameObject redImg;
    private float holdDownStartTime;
    private Vector3 move;
    private Transform orientation;
    private Transform cameraPos;
    private Ray theRay;
    private bool buttonHeldDown;
    private int level;
    private float holdDownTime = 0f;
    private bool isClicking = false;
    // Start is called before the first frame update
    void Start()
    {
        characterTransform = transform.Find("PlayerObj").transform;
        characterController = transform.GetComponent<CharacterController>();
        orientation = transform.Find("Orientation").GetComponent<Transform>();
        cameraPos = transform.Find("CameraPos").GetComponent<Transform>();        
        redImg.SetActive(false);            
        yellowImg.SetActive(false);
        greenImg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = camera.ScreenToWorldPoint(mousePos);
        //Raycast
        Vector3 direction = Vector3.forward;
        theRay = new Ray(orientation.position, mousePos-orientation.position);
        Debug.DrawRay(theRay.origin, theRay.direction*range);        


        //Move
        transform.position = characterTransform.position;
        move = orientation.forward * Input.GetAxis("Vertical") + orientation.right * Input.GetAxis("Horizontal");        
        move = move*speed;
        SetGravity();
        
        //Click        
        PlayerSkills();
        if(isClicking){
            ShowPower();
        } else {
            redImg.SetActive(false);            
            yellowImg.SetActive(false);
            greenImg.SetActive(false);
        }
        move.y += gravity * Time.deltaTime;
        characterController.Move(move*Time.deltaTime);
        transform.rotation = orientation.rotation;                

    }

    void PlayerSkills() {
        bool grounded = characterController.isGrounded;
        Debug.Log(grounded);
        if(characterController.isGrounded && Input.GetButtonDown("Jump")) {            
            fallVelocity = jumpHeight;
            move.y = fallVelocity;
        }                 
        PlayerShoot();
        PlayerAlterShoot();       
    }

    void SetGravity() {
        if (characterController.isGrounded){
            //
            fallVelocity = -gravity * Time.deltaTime;
            move.y = fallVelocity;
        } else {
            fallVelocity -= gravity * Time.deltaTime;
            move.y = fallVelocity;
        }
    }

    private float CalculateHoldDownForce(float time) {
        float maxForceHoldDownTime = 1.5f;
        float holdTimeNormalized = Mathf.Clamp01(time / maxForceHoldDownTime);
        float force = holdTimeNormalized * maxPower;
        if(force>maxPower || force>medPower) {
            force = maxPower;
        } else if (force > minPower && force < maxPower) {
            force = medPower;
        } else if (force<minPower && force<medPower && force<maxPower) {
            force = minPower;
        }
        Debug.Log(force);
        return force;
    }

    void ShowPower() {     
    holdDownTime = Time.time - holdDownStartTime;      
     Debug.Log(holdDownTime);
     float homerMaxPower = CalculateHoldDownForce(holdDownTime);
        if(homerMaxPower>=maxPower) {
            level = 2;
            redImg.SetActive(true);            
            yellowImg.SetActive(false);
            greenImg.SetActive(false);            
        } else if (homerMaxPower==medPower) {
            level = 1;
            redImg.SetActive(false);            
            yellowImg.SetActive(true);
            greenImg.SetActive(false);            
        } else if (homerMaxPower<=minPower) {
            level = 0;
            redImg.SetActive(false);            
            yellowImg.SetActive(false);
            greenImg.SetActive(true);            
        } else {
            redImg.SetActive(false);            
            yellowImg.SetActive(false);
            greenImg.SetActive(false);
        }
    }

    void Dash() {

    }

    void Sprint() {

    }

    void PlayerShoot() {
        if(Input.GetMouseButtonDown(0)){    
            speed = 7f;
            holdDownStartTime = Time.time;                                             
            isClicking=true;
        }
        if(Input.GetMouseButtonUp(0)){            
            holdDownTime = Time.time - holdDownStartTime;            
            if(Physics.Raycast(theRay, out RaycastHit hit, range)){
                if (hit.collider.tag == "Ball") {
                    Vector3 dir = hit.collider.transform.position - transform.position;
                    dir.Normalize();                    
                    hit.collider.GetComponent<Rigidbody>().AddForce(dir * CalculateHoldDownForce(holdDownTime), ForceMode.Impulse);
                }
            }
            isClicking=false;
            speed = 10f;
        }
    }

    void PlayerAlterShoot() {
        if(Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0)){    
            speed = 7f;
            holdDownStartTime = Time.time;                                             
            isClicking=true;
        }
        if(Input.GetMouseButtonUp(1) && !Input.GetMouseButtonUp(0)){            
            holdDownTime = Time.time - holdDownStartTime;            
            if(Physics.Raycast(theRay, out RaycastHit hit, range)){
                if (hit.collider.tag == "Ball") {
                    float underAngle = hit.collider.transform.position.y +2;
                    Vector3 tmp = new Vector3(hit.collider.transform.position.x, underAngle, hit.collider.transform.position.z);
                    Vector3 dir = tmp - transform.position;
                    dir.Normalize();                    
                    hit.collider.GetComponent<Rigidbody>().AddForce(dir * CalculateHoldDownForce(holdDownTime), ForceMode.Impulse);
                }
            }
            isClicking=false;
            speed = 10f;
        }
    }
}
