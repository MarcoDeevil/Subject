using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    delegate void Del();
    Del del;
    float posY = -252;
    float[] posX = {-396.7f, -267.9f, -136.8f, -1.4f, 126};
    int freeButtonMovePosition = 0;
    [SerializeField]
    public List<MoveButton> chosenMovesButtons;  

    public class MoveButton {
        public MoveButton(float ind, GameObject go, string x, Transform trans, float _speed, float _distance, float _time, float _power, float _angle){
            index = ind;
            obj = go;
            action = x;
            transform = trans;
            speed = _speed;
            distance = _distance;
            time = _time;
            power = _power;
            angle = _angle;
        }
        public float index;
        public GameObject obj;
        public string action;
        public Transform transform;
        public float speed;
        public float distance;
        public float time;
        public float power;
        public float angle;
    }
    bool koniec = false;
    public GameObject JumpChosenCard;
    public GameObject MoveChosenCard;
    public GameObject WaitChosenCard;
    public GameObject Canvas;
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
    //public List<IEnumerator> list;
    bool dead = false;
    private bool isRunning = false;

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

    void Start()
    {
        chosenMovesButtons = new List<MoveButton>();
        gm = GameManager.instance;
        startPosition = target = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        t += Time.deltaTime/time;
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
    }


  public void Move(float speed, float distance){
        dead = false;
        if(distance < 0)
            MoveLeft(speed,distance);     
        else
            MoveRight(speed,distance);    
    }

   public void MoveLeft(float speed, float distance){
        float newDistance = distance/5;
        Debug.Log("Dodano akcje"); 
        GameObject newButton = Instantiate(MoveChosenCard) as GameObject;
        newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
        newButton.transform.Find("ChosenCardMove/SpeedValue").gameObject.GetComponent<TextMeshProUGUI>().text = speed.ToString();
        newButton.transform.Find("ChosenCardMove/DistanceValue").gameObject.GetComponent<TextMeshProUGUI>().text = distance.ToString();
        newButton.transform.SetParent(Canvas.transform, false);
        Button siema = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
        Debug.Log(siema);
        siema.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
        MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "moveLeft" ,transform, speed, newDistance, 0, 0, 0);
        chosenMovesButtons.Add(x);
        freeButtonMovePosition++;    
    }

    public void MoveRight(float speed, float distance){
        float newDistance = distance/5;
        Debug.Log("Dodano akcje");
        GameObject newButton = Instantiate(MoveChosenCard) as GameObject;
        newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
        newButton.transform.Find("ChosenCardMove/SpeedValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)speed).ToString();
        newButton.transform.Find("ChosenCardMove/DistanceValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)distance).ToString();
        newButton.transform.SetParent(Canvas.transform, false);
        Button siema = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
        Debug.Log(siema);
        siema.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
        MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "moveRight" ,transform, (int)speed, (int)newDistance, 0, 0, 0);
        chosenMovesButtons.Add(x);
        freeButtonMovePosition++;
    }

    public void Jump(float power, float angle){
        dead = false;
        Debug.Log("Dodano skok");
        GameObject newButton = Instantiate(JumpChosenCard) as GameObject;
        newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
        newButton.transform.Find("ChosenCardJump/PowerValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)power).ToString();
        newButton.transform.Find("ChosenCardJump/AngleValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)angle).ToString();
        newButton.transform.SetParent(Canvas.transform, false);
        Button siema = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
        siema.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
        MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "jump" ,null, 0, 0, 0, (int)power, (int)angle);
        chosenMovesButtons.Add(x);
        freeButtonMovePosition++;
        
    }

    public void Wait(float time){
        dead = false;
        Debug.Log("dodano czekanie");
        GameObject newButton = Instantiate(WaitChosenCard) as GameObject;
        newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
        newButton.transform.Find("ChosenCardWait/TimeValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)time).ToString();
        newButton.transform.SetParent(Canvas.transform, false);
        Button siema = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
        siema.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
        MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "wait" ,null, 0, 0, (int)time, 0, 0);
        chosenMovesButtons.Add(x);
        freeButtonMovePosition++;
        
    }

    public void RearangeChosenMovesButtons(GameObject obj){
         int positionToRemove = 0;
         for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                 positionToRemove = i;
             }
         }
         Destroy(chosenMovesButtons[positionToRemove].obj);
         chosenMovesButtons.RemoveAt(positionToRemove);
         freeButtonMovePosition--;
         for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].index > positionToRemove)
                 chosenMovesButtons[i].index -= 1;
             chosenMovesButtons[i].obj.GetComponent<RectTransform>().localPosition = new Vector3(posX[(int)chosenMovesButtons[i].index],posY,0);
            }
        }

    IEnumerator JumpAction(float power, float angle){
         Debug.Log("Bede skakac");
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
         //yield break;
         Debug.Log("Koniec skoku");
          
    }

    IEnumerator WaitAction(float time){
        Debug.Log("Bede czekac");
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(time);
        yield return null;
        Debug.Log("Koniec Czekania");
    }

    

    IEnumerator MoveToPosition(Transform transform, float timeToMove, float speed, float distance)
    {
      Debug.Log("Bede sie ruszac");
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
             
            // Debug.Log("lol");
             t += Time.deltaTime / timeToMove;
             transform.position = Vector3.Lerp(currentPos, position, t);
             yield return null;
             
      }
      yield return null;
      Debug.Log("Koniec chodzenia");
     // yield break;
    }
    
    public void Sequence()
    {
        //StopAllCoroutines();
        dead = false;
        if(isRunning == false){
            StartCoroutine(GO());
        }
    }

    public IEnumerator GO(){
        isRunning = true;
        foreach(MoveButton func in chosenMovesButtons){

            if(dead)
                break; 

            if(func.action == "wait")
                yield return StartCoroutine(WaitAction(func.time));
            else if(func.action == "jump")
                yield return StartCoroutine(JumpAction(func.power, func.angle));
            else if(func.action == "moveLeft")
                yield return StartCoroutine(MoveToPosition(func.transform, -func.distance/func.speed, func.speed, func.distance));
            else if(func.action == "moveRight")
                yield return StartCoroutine(MoveToPosition(func.transform, func.distance/func.speed, func.speed, func.distance));
            Debug.Log(func.action);

            if(dead)
                break;    

            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
        isRunning = false;
        Debug.Log("Koniec essa");
        if(koniec == false)
            RestartLevel();
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Bound"){  
           RestartLevel();
           // StopAllCoroutines(); 
            //chosenMovesButtons.Clear();
            //Instantiate(this, spawn, this.transform.rotation);  
            //Destroy(gameObject);
               
           // gm.restartScene();           
        }
        
        else if(col.gameObject.tag == "Finish"){
            koniec = true;
            Destroy(gameObject);
            gm.nextScene();
        }
    }

    public void RestartLevel(){
        dead = true;
        transform.position = spawn;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }

    public void ClearMoves(){
       for(int i = 0; i<chosenMovesButtons.Count; i++){
             Destroy(chosenMovesButtons[i].obj);
         }
         chosenMovesButtons.Clear();
         freeButtonMovePosition = 0;
    }
}
