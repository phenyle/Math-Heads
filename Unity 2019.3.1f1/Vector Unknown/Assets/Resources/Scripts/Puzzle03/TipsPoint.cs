using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TipsPoint : MonoBehaviour
{
    public Transform point1, point2, point3;
    public Text txtPoint1, txtPoint2, txtPoint3;
    public LineRenderer line;

    private void Update()
    {
        line.SetPosition(0, point1.localPosition);
        line.SetPosition(1, point2.localPosition);
        line.SetPosition(2, point3.localPosition);

        txtPoint1.text = point1.localPosition.x + "\n" +
                                  point1.localPosition.z + "\n" +
                                  point1.localPosition.y;

        txtPoint2.text = point2.localPosition.x.ToString("F0") + "\n" +
                                  point2.localPosition.z.ToString("F0") + "\n" +
                                  point2.localPosition.y.ToString("F0");

        txtPoint3.text = point3.localPosition.x.ToString("F0") + "\n" +
                                  point3.localPosition.z.ToString("F0") + "\n" +
                                  point3.localPosition.y.ToString("F0");
    }
}
