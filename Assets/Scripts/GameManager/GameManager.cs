using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UnityAction onGameOver;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver()
    {
        Debug.Log("게임 오버!");
        onGameOver?.Invoke();
    }
}
