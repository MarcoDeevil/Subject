using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public ParticleSystem ps;
    delegate void Del();
    Del del;
    float posY = -252;
    float[] posX = {-396.7f, -267.9f, -136.8f, -1.4f, 126};
    int freeButtonMovePosition = 0;
    [SerializeField]
    public List<MoveButton> chosenMovesButtons;  
    GameObject currentChangedObject;

    public GameObject CoverButton;
    public GameObject ChangeWaitCard;
    public GameObject ChangeMoveCard;
    public GameObject ChangeJumpCard;
    public TextMeshProUGUI ChangeTimeText;
    public TextMeshProUGUI ChangePowerText;
    public TextMeshProUGUI ChangeAngleText;
    public TextMeshProUGUI ChangeSpeedText;
    public TextMeshProUGUI ChangeDistanceText;

    public Slider PowerChangeSlider;
    public Slider AngleChangeSlider;
    public Slider SpeedChangeSlider;
    public Slider DistanceChangeSlider;
    public Slider TimeChangeSlider;


    [Serializable]
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
        if(!dead)
            ps.transform.position = transform.position;
        t += Time.deltaTime/time;
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
    }

    public void Move(float speed, float distance){
        dead = false;
        if(freeButtonMovePosition < 5){
        float newDistance = distance/4;
        Debug.Log("Dodano akcje");
        GameObject newButton = Instantiate(MoveChosenCard) as GameObject;
        newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
        newButton.transform.Find("ChosenCardMove/SpeedValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)speed).ToString();
        newButton.transform.Find("ChosenCardMove/DistanceValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)distance).ToString();
        newButton.transform.SetParent(Canvas.transform, false);
        Button copy = newButton.transform.Find("CopyButton").gameObject.GetComponent<Button>();
        copy.onClick.AddListener(() => CopyMoveCard(newButton));
        Button siema = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
        Button card = newButton.transform.Find("ChosenCardMove").gameObject.GetComponent<Button>();
        card.onClick.AddListener(() => MoveChange(newButton));
        siema.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
        MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "move" ,transform, (int)speed, newDistance, 0, 0, 0);
        chosenMovesButtons.Add(x);
        freeButtonMovePosition++;
        }
    }

    public void Jump(float power, float angle){
        dead = false;
        if(freeButtonMovePosition < 5){
        Debug.Log("Dodano skok");
        GameObject newButton = Instantiate(JumpChosenCard) as GameObject;
        newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
        newButton.transform.Find("ChosenCardJump/PowerValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)power).ToString();
        newButton.transform.Find("ChosenCardJump/AngleValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)angle).ToString();
        newButton.transform.SetParent(Canvas.transform, false);
        Button copy = newButton.transform.Find("CopyButton").gameObject.GetComponent<Button>();
        copy.onClick.AddListener(() => CopyJumpCard(newButton));
        Button delete = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
        delete.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
        Button card = newButton.transform.Find("ChosenCardJump").gameObject.GetComponent<Button>();
        card.onClick.AddListener(() => JumpChange(newButton));
        MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "jump" ,null, 0, 0, 0, (int)power, (int)angle);
        chosenMovesButtons.Add(x);
        freeButtonMovePosition++;
        }
        
    }

    public void Wait(float time){
        dead = false;
        if(freeButtonMovePosition < 5){
        Debug.Log("dodano czekanie");
        GameObject newButton = Instantiate(WaitChosenCard) as GameObject;
        newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
        newButton.transform.Find("ChosenCardWait/TimeValue").gameObject.GetComponent<TextMeshProUGUI>().text = ((int)time).ToString();
        newButton.transform.SetParent(Canvas.transform, false);
        Button copy = newButton.transform.Find("CopyButton").gameObject.GetComponent<Button>();
        copy.onClick.AddListener(() => CopyWaitCard(newButton));
        Button siema = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
        siema.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
        Button card = newButton.transform.Find("ChosenCardWait").gameObject.GetComponent<Button>();
        card.onClick.AddListener(() => WaitChange(newButton));
        MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "wait" ,null, 0, 0, (int)time, 0, 0);
        chosenMovesButtons.Add(x);
        freeButtonMovePosition++;
        }
        
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

         if(isGrounded)
            rb.velocity = Vector2.zero;
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

    public IEnumerator MoveToPosition (Transform obj, float speed, float distance){
     // speed should be 1 unit per second
     Debug.Log("zaczunam isc");
     Vector3 end = transform.position + new Vector3(distance,0,0);
     while (obj.position != end)
     {
         if(dead)
            break;
        if(isGrounded == false){
            Vector3 dir = new Vector3(0,0,0);
            dir = Quaternion.AngleAxis(0, Vector3.forward) * Vector3.right;
            rb.AddForce(dir*speed*50);
            break;
          }        
         obj.position = Vector3.MoveTowards(obj.transform.position, end, speed * Time.deltaTime);
         yield return null;
     }
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
            else if(func.action == "move")
                yield return StartCoroutine(MoveToPosition(func.transform,  func.speed, func.distance));
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

        else if(col.gameObject.tag == "Saw"){
            ps.Play();
            RestartLevel();
        }
    }

    public void RestartLevel(){
        dead = true;
        transform.position = spawn;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    public void ClearMoves(){
       for(int i = 0; i<chosenMovesButtons.Count; i++){
             Destroy(chosenMovesButtons[i].obj);
         }
         chosenMovesButtons.Clear();
         freeButtonMovePosition = 0;
    }

    public void WaitChange(GameObject obj){
        ChangeWaitCard.SetActive(true);
        CoverButton.SetActive(true);
        CoverButton.GetComponent<Image>().color = new Color(255/255f,241/255,105/255f,108/255f);    
        int positionToRemove = 0;
         for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                 positionToRemove = i;
             }
         }
         TimeChangeSlider.value = chosenMovesButtons[positionToRemove].time;
         //obj = chosenMovesButtons[positionToRemove].obj;
         ChangeTimeText.text = chosenMovesButtons[positionToRemove].time.ToString();

        Button change = ChangeWaitCard.transform.Find("CardWaitChange/ChangeWaitButton").gameObject.GetComponent<Button>();
        change.onClick.RemoveAllListeners();
        change.onClick.AddListener(() => AcceptWaitChange(obj));
    }

    public void AcceptWaitChange(GameObject obj){
        int positionToRemove = 0;
         for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                 positionToRemove = i;
             }
         }
        chosenMovesButtons[positionToRemove].time = int.Parse(ChangeTimeText.text);
 
        obj.transform.Find("ChosenCardWait/TimeValue").gameObject.GetComponent<TextMeshProUGUI>().text = ChangeTimeText.text;

        ChangeWaitCard.SetActive(false);
        CoverButton.SetActive(false);
    }

    public void MoveChange(GameObject obj){
        ChangeMoveCard.SetActive(true);
        CoverButton.SetActive(true);
        CoverButton.GetComponent<Image>().color = new Color(224/255f,69/255,69/255f,108/255f); 
        int positionToRemove = 0;
         for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                 positionToRemove = i;
             }
         }
        // obj = chosenMovesButtons[positionToRemove].obj;
         SpeedChangeSlider.value = chosenMovesButtons[positionToRemove].speed;
         DistanceChangeSlider.value = chosenMovesButtons[positionToRemove].distance; 
         ChangeSpeedText.text = chosenMovesButtons[positionToRemove].speed.ToString();
         ChangeDistanceText.text = chosenMovesButtons[positionToRemove].distance.ToString();

        Button change = ChangeMoveCard.transform.Find("CardMoveChange/ChangeMoveButton").gameObject.GetComponent<Button>();
        change.onClick.RemoveAllListeners();
        change.onClick.AddListener(() => AcceptMoveChange(obj));
    }

    public void AcceptMoveChange(GameObject obj){      
        int positionToRemove = 0;
         for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                 positionToRemove = i;
             }
         }
        chosenMovesButtons[positionToRemove].speed = int.Parse(ChangeSpeedText.text);
        chosenMovesButtons[positionToRemove].distance = float.Parse(ChangeDistanceText.text)/4;

        obj.transform.Find("ChosenCardMove/SpeedValue").gameObject.GetComponent<TextMeshProUGUI>().text = ChangeSpeedText.text;
        obj.transform.Find("ChosenCardMove/DistanceValue").gameObject.GetComponent<TextMeshProUGUI>().text = ChangeDistanceText.text;
        ChangeMoveCard.SetActive(false);
        CoverButton.SetActive(false);
    }

    public void JumpChange(GameObject obj){
        ChangeJumpCard.SetActive(true);
        CoverButton.SetActive(true);
       // CoverButton.GetComponent<Image>().color = new Color(73/255f,187/255f,217/255f,108/255f);  
        int positionToRemove = 0;
         for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                 positionToRemove = i;
                 ChangePowerText.text = chosenMovesButtons[i].power.ToString();
                 ChangeAngleText.text = chosenMovesButtons[i].angle.ToString();
                 PowerChangeSlider.value = chosenMovesButtons[i].power;
                 AngleChangeSlider.value = chosenMovesButtons[i].angle;
             }
         }

        Button change = ChangeJumpCard.transform.Find("CardJumpChange/ChangeJumpButton").gameObject.GetComponent<Button>();
        change.onClick.RemoveAllListeners();
        change.onClick.AddListener(() => AcceptJumpChange(obj));
    }

    public void AcceptJumpChange(GameObject obj){
       
        int positionToRemove = 0;
         for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                 positionToRemove = i;
             }
         }
        chosenMovesButtons[positionToRemove].power = int.Parse(ChangePowerText.text);
        chosenMovesButtons[positionToRemove].angle = int.Parse(ChangeAngleText.text);

        obj.transform.Find("ChosenCardJump/PowerValue").gameObject.GetComponent<TextMeshProUGUI>().text = ChangePowerText.text;
        obj.transform.Find("ChosenCardJump/AngleValue").gameObject.GetComponent<TextMeshProUGUI>().text = ChangeAngleText.text;
        ChangeJumpCard.SetActive(false);
        CoverButton.SetActive(false);
    }

    public void CopyWaitCard(GameObject obj){
        if(freeButtonMovePosition < 5){
        for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                GameObject newButton = Instantiate(WaitChosenCard) as GameObject;
                newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
                newButton.transform.Find("ChosenCardWait/TimeValue").gameObject.GetComponent<TextMeshProUGUI>().text = chosenMovesButtons[i].time.ToString();
                newButton.transform.SetParent(Canvas.transform, false);
                Button copy = newButton.transform.Find("CopyButton").gameObject.GetComponent<Button>();
                copy.onClick.AddListener(() => CopyWaitCard(newButton));
                Button siema = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
                siema.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
                Button card = newButton.transform.Find("ChosenCardWait").gameObject.GetComponent<Button>();
                card.onClick.AddListener(() => WaitChange(newButton));
                MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "wait" ,transform, chosenMovesButtons[i].speed, 
                chosenMovesButtons[i].distance, chosenMovesButtons[i].time, chosenMovesButtons[i].power, chosenMovesButtons[i].angle);
                chosenMovesButtons.Add(x);
                freeButtonMovePosition++;             
             }
         }
        }
    }

    public void CopyMoveCard(GameObject obj){
        if(freeButtonMovePosition < 5){
        for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                GameObject newButton = Instantiate(MoveChosenCard) as GameObject;
                newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
                newButton.transform.Find("ChosenCardMove/SpeedValue").gameObject.GetComponent<TextMeshProUGUI>().text = chosenMovesButtons[i].speed.ToString();
                newButton.transform.Find("ChosenCardMove/DistanceValue").gameObject.GetComponent<TextMeshProUGUI>().text = (chosenMovesButtons[i].distance*4).ToString();
                newButton.transform.SetParent(Canvas.transform, false);
                 Button copy = newButton.transform.Find("CopyButton").gameObject.GetComponent<Button>();
                copy.onClick.AddListener(() => CopyMoveCard(newButton));
                Button siema = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
                Button card = newButton.transform.Find("ChosenCardMove").gameObject.GetComponent<Button>();
                card.onClick.AddListener(() => MoveChange(newButton));
                siema.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
                MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "move", transform, chosenMovesButtons[i].speed, 
                chosenMovesButtons[i].distance, chosenMovesButtons[i].time, chosenMovesButtons[i].power, chosenMovesButtons[i].angle);
                chosenMovesButtons.Add(x);
                freeButtonMovePosition++;      
                break;       
             }
            }
         }
    }

    public void CopyJumpCard(GameObject obj){
        if(freeButtonMovePosition < 5){
        for(int i = 0; i<chosenMovesButtons.Count; i++){
             if(chosenMovesButtons[i].obj == obj){
                GameObject newButton = Instantiate(JumpChosenCard) as GameObject;
                newButton.GetComponent<RectTransform>().localPosition = new Vector3(posX[freeButtonMovePosition], posY, 0);
                newButton.transform.Find("ChosenCardJump/PowerValue").gameObject.GetComponent<TextMeshProUGUI>().text = chosenMovesButtons[i].power.ToString();
                newButton.transform.Find("ChosenCardJump/AngleValue").gameObject.GetComponent<TextMeshProUGUI>().text = chosenMovesButtons[i].angle.ToString();
                newButton.transform.SetParent(Canvas.transform, false);
                Button copy = newButton.transform.Find("CopyButton").gameObject.GetComponent<Button>();
                copy.onClick.AddListener(() => CopyMoveCard(newButton));
                Button delete = newButton.transform.Find("Button").gameObject.GetComponent<Button>();
                delete.onClick.AddListener(() => RearangeChosenMovesButtons(newButton));
                Button card = newButton.transform.Find("ChosenCardJump").gameObject.GetComponent<Button>();
                card.onClick.AddListener(() => JumpChange(newButton));
                MoveButton x = new MoveButton(freeButtonMovePosition, newButton, "jump" ,transform, chosenMovesButtons[i].speed, 
                chosenMovesButtons[i].distance, chosenMovesButtons[i].time, chosenMovesButtons[i].power, chosenMovesButtons[i].angle);
                chosenMovesButtons.Add(x);
                freeButtonMovePosition++;              
             }
            }
         }    
    }

    public void CoverButtonMoveClick(){
        CoverButton.GetComponent<Image>().color = new Color(224/255f,69/255f,69/255f,108/255f);        
    }

    public void CoverButtonJumpClick(){
        CoverButton.GetComponent<Image>().color = new Color(73/255f,187/255f,217/255f,108/255f);        
    }

    public void CoverButtonWaitClick(){
        CoverButton.GetComponent<Image>().color = new Color(255/255f,241/255f,105/255f,108/255f);       
    }
}
