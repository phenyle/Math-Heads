using UnityEngine;

[ExecuteInEditMode]
public class TipsPoint : MonoBehaviour
{
    public Transform point1, point2, point3;
    public LineRenderer line;

    private void Update()
    {
        line.SetPosition(0, point1.localPosition);
        line.SetPosition(1, point2.localPosition);
        line.SetPosition(2, point3.localPosition); 
    }
}
