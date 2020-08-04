using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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

    void Awake(){
        if(instance == null){
                DontDestroyOnLoad(gameObject);
                instance = this;     
            }
            else {
                Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
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

   public void MoveLeft(float speed, float distance){
       distance = distance/5;
        target = transform.position + new Vector3(-distance,0,0);
        StartCoroutine(MoveToPosition(transform, target, distance/speed, speed));

        
    }

    public void MoveRight(float speed, float distance){
        distance = distance/5;
         target = transform.position + new Vector3(distance,0,0);
        StartCoroutine(MoveToPosition(transform, target, distance/speed, speed));
    }

    public void Jump(float power, float angle){
        Vector3 dir = new Vector3(0,0,0);

        if(angle >= 0)
             dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        
        else if(angle < 0)
             dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.left;
        
         rb.AddForce(dir*power*50);
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove, float speed)
   {
      var currentPos = transform.position;
      var t = 0f;
       while(t < 1)
       {
             if(isGrounded == false){
                 Vector3 dir = new Vector3(0,0,0);
                 dir = Quaternion.AngleAxis(0, Vector3.forward) * Vector3.right;
                 rb.AddForce(dir*speed*50);
                 break;
             }
                
             t += Time.deltaTime / timeToMove;
             transform.position = Vector3.Lerp(currentPos, position, t);
             yield return null;
      }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Bound"){  
            Destroy(gameObject);     
            gm.restartScene();           
        }
        
        else if(col.gameObject.tag == "Finish"){
            Destroy(gameObject);
            gm.nextScene();
        }
    }
}
