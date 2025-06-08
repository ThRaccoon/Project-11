using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private CursorController _cursorController;
    public void OnMenu()
    {
        if (_menu == null && _cursorController == null) return;

        if(Util.ObjectToggle(_menu))
        {
            Time.timeScale = 0f;

        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
