using UnityEngine;

public class CursorController : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    private PlayerCamera _playerCamera;
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private Texture2D _cursorPointTexture;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private void Awake()
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (Util.IsNotNull(mainCamera))
        {
            _playerCamera = mainCamera.GetComponent<PlayerCamera>();
        }

        DisableCursor();
    }


    public void EnableCursor()
    {
        if (Util.IsNotNull(_cursorTexture))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
        }

        if (Util.IsNotNull(_playerCamera))
        {
            _playerCamera.DisableRotation();
        }
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Util.IsNotNull(_playerCamera))
        {
            _playerCamera.EnableRotation();
        }
    }


    public void EventEnter()
    {
        if (Util.IsNotNull(_cursorPointTexture))
        {
            Cursor.SetCursor(_cursorPointTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    public void EventExit()
    {
        if (Util.IsNotNull(_cursorPointTexture))
        {
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}