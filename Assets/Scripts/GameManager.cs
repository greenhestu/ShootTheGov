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
        stage += 1;
    }
}
