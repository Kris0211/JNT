using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
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

    [Header("Events")]
    public UnityEvent<Vector2> movementRequested;
    public UnityEvent colorChangeRequested;

    void Awake()
    {
        SetupDirectionalButton(_upButton, new Vector2(0, 1));
        SetupDirectionalButton(_downButton, new Vector2(0, -1));
        SetupDirectionalButton(_leftButton, new Vector2(-1, 0));
        SetupDirectionalButton(_rightButton, new Vector2(1, 0));

        if (_changeColorButton != null)
        {
            _changeColorButton.onClick.AddListener(() => colorChangeRequested?.Invoke());
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
        pointerDownEntry.callback.AddListener(data => movementRequested?.Invoke(dir));
        eventTrigger.triggers.Add(pointerDownEntry);

        // On button UP - stop movement (reset direction to zero)
        EventTrigger.Entry pointerUpEntry = new()
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener(data => movementRequested?.Invoke(Vector2.zero));
        eventTrigger.triggers.Add(pointerUpEntry);
    }
}
