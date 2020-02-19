using UnityEngine;

public class SystemRoot : MonoBehaviour
{
    protected ResourceService resourceService;
    protected AudioService audioService;

    public virtual void InitSystem()
    {
        resourceService = GameRoot.instance.resourceService;
        audioService = GameRoot.instance.audioService;
    }

    protected void CloseAllWindow()
    {
        Transform canvas = transform.Find("Canvas");

        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        canvas.Find("DynamicWindow").GetComponent<DynamicWindow>().SetWindowState(true);
    }
}
