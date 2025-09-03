using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Loader.Load(Loader.Scene.GameScene);
        }
    }
}
