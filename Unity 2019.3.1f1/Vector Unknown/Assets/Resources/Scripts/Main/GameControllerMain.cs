using UnityEngine;

public class GameControllerMain : GameControllerRoot
{
    [HideInInspector]
    public MainWindow MW;

    public override void InitGameController(MainWindow MW)
    {
        Debug.Log("Init GameController Main");
        base.InitGameController();

        Debug.Log("Connect Main Window");
        this.MW = MW;

        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Main_00"));
    }
}
