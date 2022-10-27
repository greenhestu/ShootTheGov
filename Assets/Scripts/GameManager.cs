using UnityEngine;

public class GameManager
{
    private static GameManager instance;
    private int stage = 0;

    private GameManager()
    { }

    public static GameManager Instance
    {
        get
        {
            instance ??= new GameManager();
            return instance;
        }
    }

    public void WinStage()
    {
        var pWinUI = Resources.Load<GameObject>("Prefabs/WinUI");
        Object.Instantiate(pWinUI);
    }

    public void NextStage()
    {
        Debug.Log("New Stage");
        stage += 1;
    }
}
