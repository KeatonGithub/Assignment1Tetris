using UnityEngine;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    void OnDestroy()
    {
        if (Score.instance != null)
        {
            Score.instance.AddScore(10); 
        }
    }
}
