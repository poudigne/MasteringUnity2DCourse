using UnityEngine;
using System.Collections;

public class CommandBar : MonoBehaviour {

    private CommandButton[] commandButtons;

    public float buttonSize = 0.4f;
    public float buttonRows = 1;
    public float buttonColumns = 6;
    public float buttonRowSpacing = 0;
    public float buttonColumnSpacing = 0.1f;

    public Sprite defaultButtonImage;
    public Sprite selectedButtonImage;

    private float screenHeight;
    private float screenWidth;

    private bool canSelectButton = true;
    private CommandButton selectedButton;

    public bool anchor = true;
    public Vector2 anchorOffSet = Vector2.zero;
    public ScreenPositionAnchorPoint anchorPoint = ScreenPositionAnchorPoint.BottomCenter;


    void SetCanSelectButton(bool state)
    {
        canSelectButton = !state;
    }

    void Start()
    {
        InitCommandButtons();
        MessagingManager.Instance.SubscribeUIEvent(SetCanSelectButton);
    }

    void OnDestroy()
    {
        if (MessagingManager.Instance != null)
        {
            MessagingManager.Instance.UnSubscribeUIEvent(SetCanSelectButton);
        }
    }

    void Update()
    {
        Vector2 position = Vector2.zero;

        if (anchor)
        {
            position = CalculateAnchorScreenPosition();
        }
        else
        {
            position = transform.position;
        }
        SetPosition(position.x, position.y);
    }

    void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Screen.width / Screen.height;
    }
    public int Layer
    {
        get { return gameObject.layer; }
    }

    float Width
    {
        get
        {
            return (buttonSize * buttonColumns) + Mathf.Clamp((buttonColumnSpacing * (buttonColumns - 1)), 0, int.MaxValue);
        }
    }
    float Height
    {
        get
        {
            return (buttonSize * buttonRows) + Mathf.Clamp((buttonColumnSpacing * (buttonRows - 1)), 0, int.MaxValue);
        }
    }

    void InitCommandButtons()
    {
        commandButtons = new CommandButton[(int)buttonRows * (int)buttonColumns];

        for (int i = 0; i < commandButtons.Length; i++)
        {
            var newButton = CreateButton();

            if (i < GameState.currentPlayer.Inventory.Count)
            {
                newButton.AddInventoryItem(GameState.currentPlayer.Inventory[i]);
            }
            commandButtons[i] = newButton;
        }

        InitButtonPositions();
    }

    CommandButton CreateButton()
    {
        GameObject go = new GameObject("CommandButton");
        go.AddComponent<SpriteRenderer>();
        go.AddComponent<BoxCollider2D>();

        go.transform.parent = transform;

        CommandButton button = go.AddComponent<CommandButton>();
        button.Init(this);

        return button;
    }

    void InitButtonPositions()
    {
        int i = 0;
        float xPos = 0;
        float yPos = 0;

        for (int r = 0; r < buttonRows; ++r)
        {
            xPos = 0;
            for (int c = 0; c <buttonColumns; ++c)
            {
                commandButtons[i].transform.localScale = new Vector3(buttonSize, buttonSize, 0);
                commandButtons[i].transform.localPosition = new Vector3(xPos, yPos, 0);

                i++;

                xPos += buttonSize + buttonColumnSpacing;
            }

            yPos -= buttonSize + buttonRowSpacing;
        }
    }

    void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }

    Vector2 CalculateAnchorScreenPosition()
    {
        Vector2 position = Vector2.zero;

        switch (anchorPoint)
        {
            case ScreenPositionAnchorPoint.TopLeft:
                position.y = (screenHeight / 2) - Height;
                position.x = -(screenWidth / 2) + buttonSize;
                break;
            case ScreenPositionAnchorPoint.TopCenter:
                position.y = (screenHeight / 2) - Height;
                position.x = -(Width / 2);
                break;
            case ScreenPositionAnchorPoint.TopRight:
                position.y = (screenHeight / 2) - Height;
                position.x = (screenWidth / 2) - Width;
                break;
            case ScreenPositionAnchorPoint.MiddleLeft:
                position.y = (Height / 2);
                position.x = -(screenWidth / 2) + buttonSize;
                break;
            case ScreenPositionAnchorPoint.MiddleCenter:
                position.y = (Height / 2);
                position.x = -(Width / 2);
                break;
            case ScreenPositionAnchorPoint.MiddleRight:
                position.y = (Height / 2);
                position.x = (screenWidth / 2) - Width;
                break;
            case ScreenPositionAnchorPoint.BottomLeft:
                position.y = -(screenHeight / 2) + Height;
                position.x = -(screenWidth / 2) + buttonSize;
                break;
            case ScreenPositionAnchorPoint.BottomCenter:
                position.y = -(screenHeight / 2) + Height;
                position.x = -(Width / 2);
                break;
            case ScreenPositionAnchorPoint.BottomRight:
                position.y = -(screenHeight / 2) + Height;
                position.x = (screenWidth / 2) - Width;
                break;
        }

        return anchorOffSet + position;
    }

    public bool CanSelectButton
    {
        get
        {
            return canSelectButton;
        }
    }

    public void ResetSelection(CommandButton button)
    {
        if (selectedButton != null)
        {
            selectedButton.ClearSelection();
        }
        selectedButton = button;
    }

    public void SelectButton(CommandButton button)
    {
        if (selectedButton != null)
        {
            selectedButton.ClearSelection();
        }
        selectedButton = button;
        Debug.Log("Select button");
        if (selectedButton != null)
        {
            MessagingManager.Instance.BroadcastInventoryEvent(selectedButton.item);
        }
        else
        {
            MessagingManager.Instance.BroadcastInventoryEvent(null);
        }
    }
}
