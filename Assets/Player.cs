using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    delegate void Del();
    Del del;
    
    Rigidbody2D rb;
    public float speed = 1;
    float direction;
    public float length = 1;
    Vector3 startPosition;
    public float time = 3;
    float t;
    Vector3 target;
    public Transform feetPos;
    public bool isGrounded;
    public float checkRadius;
    public LayerMask whatisGround;
    public static Player instance;
    int currentScene = 0;
    GameManager gm;
    public List<IEnumerator> list;
    bool dead = false;

    public Vector3 spawn;

   
    List<Action> actions = new List<Action>();


    private void DequeueTasks(){
        
    }

    void Awake(){
        if(instance == null){
                //DontDestroyOnLoad(gameObject);
                instance = this;     
            }
            else {
                Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        list = new List<IEnumerator>();
        gm = GameManager.instance;
        startPosition = target = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime/time;
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
       // transform.position = Vector3.Lerp(startPosition, target, t);
    }


  public void Move(float speed, float distance){
        dead = false;
        if(distance < 0)
            MoveLeft(speed,distance);     
        else
            MoveRight(speed,distance);
    }

   public void MoveLeft(float speed, float distance){
        distance = distance/5;
        list.Add(MoveToPosition(transform, -(distance)/speed, speed, distance));    
        Debug.Log("Dodano akcje");     
    }

    public void MoveRight(float speed, float distance){
        distance = distance/5;
        list.Add(MoveToPosition(transform,distance/speed, speed, distance));    
        Debug.Log("Dodano akcje");
    }

    public void Jump(float power, float angle){
        dead = false;
        list.Add(JumpAction(power, angle));
        Debug.Log("Dodano skok");
    }

    public void Wait(float time){
        dead = false;
        list.Add(WaitAction(time));
        Debug.Log("dodano czekanie");
    }

    IEnumerator JumpAction(float power, float angle){
        Vector3 dir = new Vector3(0,0,0);

        if(angle >= 0)
             dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        
        else if(angle < 0)
             dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.left;
        
         rb.AddForce(dir*power*50);
        yield return new WaitForSeconds(0.1f);
         while(!isGrounded){
            yield return null;
         }
         yield return null;
          
    }

    IEnumerator WaitAction(float time){
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(time);
    }

    

    IEnumerator MoveToPosition(Transform transform, float timeToMove, float speed, float distance)
    {
      Debug.Log(distance);
      Vector3 position = transform.position + new Vector3(distance,0,0);
      var currentPos = transform.position;
      var t = 0f;
       while(t < 1)
       {
            if(dead)
                break;            
             if(isGrounded == false){
                 Vector3 dir = new Vector3(0,0,0);
                 dir = Quaternion.AngleAxis(0, Vector3.forward) * Vector3.right;
                 rb.AddForce(dir*speed*50);
                 break;
             }
             
             Debug.Log("lol");
             t += Time.deltaTime / timeToMove;
             transform.position = Vector3.Lerp(currentPos, position, t);
             yield return null;
      }
    }
    
    public void Sequence()
    {
        StartCoroutine(GO());
    }

    public IEnumerator GO(){
        foreach(IEnumerator func in list){
            if(dead)
                break;
            yield return StartCoroutine(func);
            Debug.Log(func);
            if(dead)
                break;
            yield return new WaitForSeconds(0.2f);
        }

        list.Clear();
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Bound"){  
            dead = true;
            transform.position = spawn;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0; 
            list.Clear();
            //Instantiate(this, spawn, this.transform.rotation);  
            //Destroy(gameObject);
               
           // gm.restartScene();           
        }
        
        else if(col.gameObject.tag == "Finish"){
            Destroy(gameObject);
            gm.nextScene();
        }
    }
}
