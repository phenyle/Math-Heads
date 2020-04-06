using UnityEngine;
using UnityEngine.UI;

public class CreditsWindow : WindowRoot
{
    private void Start()
    {
        InitWindow();
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();
    }
}
