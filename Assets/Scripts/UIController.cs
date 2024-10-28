using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public event Action<Vector2> MovementRequested;
    public event Action ColorChangeRequested;
    public event Action PauseGameRequested;

    [Space(4)]
    [SerializeField]
    private Button _pauseButton;
    [Space(8)]
    [SerializeField]
    private Button _changeColorButton;
    [Header("Movement Buttons")]
    [SerializeField]
    private Button _upButton;
    [SerializeField]
    private Button _downButton;
    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;

    void Start()
    {
        SetupDirectionalButton(_upButton, new Vector2(0, 1));
        SetupDirectionalButton(_downButton, new Vector2(0, -1));
        SetupDirectionalButton(_leftButton, new Vector2(-1, 0));
        SetupDirectionalButton(_rightButton, new Vector2(1, 0));

        if (_changeColorButton != null)
        {
            _changeColorButton.onClick.AddListener(() => ColorChangeRequested?.Invoke());
        }
        if (_pauseButton != null)
        {
            _pauseButton.onClick.AddListener(() => PauseGameRequested?.Invoke());
        }
    }

    private void SetupDirectionalButton(Button button, Vector2 dir)
    {
        if (button == null) return;
        
        if (!button.TryGetComponent<EventTrigger>(out var eventTrigger))
        {
            Debug.Log("No EventTrigger component found, creating new one");
            eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // On button DOWN - start movement
        EventTrigger.Entry pointerDownEntry = new()
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDownEntry.callback.AddListener(data => MovementRequested?.Invoke(dir));
        eventTrigger.triggers.Add(pointerDownEntry);

        // On button UP - stop movement (reset direction to zero)
        EventTrigger.Entry pointerUpEntry = new()
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener(data => MovementRequested?.Invoke(Vector2.zero));
        eventTrigger.triggers.Add(pointerUpEntry);
    }
}
