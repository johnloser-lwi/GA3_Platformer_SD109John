namespace Utility
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}