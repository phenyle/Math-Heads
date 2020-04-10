using UnityEngine;

public class CreditsWindow : WindowRoot
{
    private void Start()
    {
        InitWindow();
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Credits window");
        base.InitWindow();
    }
}
