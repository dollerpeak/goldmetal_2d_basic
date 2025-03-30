using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint = 0;
    public int stagePoint = 0;

    public int playerHp = 5;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStage(string nSceneName)
    {
        totalPoint += stagePoint;
        stagePoint = 0;

        SceneManager.LoadScene(nSceneName);
    }

    //public int StagePoint
    //{
    //    get { return stagePoint; }
    //    set { stagePoint = value; }
    //}


}




