using Unity.VisualScripting;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private Texture2D _cursorPointTexture;
    private PlayerCamera _playerCamera;
    private void Awake()
    {
        DisableCursor();

        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        if(camera != null )
        {
            _playerCamera = camera.GetComponent<PlayerCamera>();
        }
    }


    public void EnableCursor()
    {
        if (_cursorTexture != null) 
        {

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            if (_cursorTexture != null)
            {
                Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
            }
        }
        
        _playerCamera?.DisableRotate();
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerCamera?.EnableRotate();
    }

    public void EventEnter()
    {
        if(_cursorPointTexture != null)
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

}
