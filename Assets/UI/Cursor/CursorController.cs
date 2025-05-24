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

        if (mainCamera != null)
        {
            _playerCamera = mainCamera.GetComponent<PlayerCamera>();
        }

        DisableCursor();
    }


    public void EnableCursor()
    {
        if (_cursorTexture != null)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
        }

        if (_playerCamera != null)
        {
            _playerCamera.DisableRotation();
        }
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (_playerCamera != null)
        {
            _playerCamera.EnableRotation();
        }
    }

    public void EventEnter()
    {
        if (_cursorPointTexture != null)
        {
            Cursor.SetCursor(_cursorPointTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    public void EventExit()
    {
        if (_cursorPointTexture != null)
        {
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

   public bool IsVisiable()
    {
        return Cursor.visible;
    }
}