using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class MyUIGradient : BaseMeshEffect
{
    public Color my_color1 = Color.white;
    public Color my_color2 = Color.white;
    [Range(-180f, 180f)]
    public float my_angle = 0f;
    public bool my_ignoreRatio = true;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (enabled)
        {
            Rect rect = graphic.rectTransform.rect;
            Vector2 dir = UIGradientUtils.RotationDir(my_angle);

            if (!my_ignoreRatio)
                dir = UIGradientUtils.CompensateAspectRatio(rect, dir);

            UIGradientUtils.Matrix2x3 localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, dir);

            UIVertex vertex = default(UIVertex);
            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);
                Vector2 localPosition = localPositionMatrix * vertex.position;
                vertex.color *= Color.Lerp(my_color2, my_color1, localPosition.y);
                vh.SetUIVertex(vertex, i);
            }
        }
    }
}