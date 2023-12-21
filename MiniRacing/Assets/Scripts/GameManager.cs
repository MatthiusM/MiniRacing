using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Countdown,
    Playing,
    Paused,
    Ended
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject countdownTimer;
    public static GameManager Instance { get; private set; }

    private GameState currentState = GameState.Countdown;

    private UnityEvent onInitialise = new();

    public GameState CurrentState
    {
        get => currentState;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            CountDown countDown = countdownTimer.GetComponent<CountDown>();
            countDown.AddListenerCountDownFinished(CountDownFinished);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddListenerOnInitialise(UnityAction listener)
    {
        onInitialise.AddListener(listener);
    }

    void CountDownFinished()
    {
        onInitialise.Invoke();
        currentState = GameState.Playing;
    }
}
