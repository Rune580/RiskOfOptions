using RiskOfOptions.Resources;
using UnityEngine;

namespace RiskOfOptions.Components.Gizmos;

public class DrawRect : MonoBehaviour
{
    private static Material _lineMaterial = null!;
    private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
    private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
    private static readonly int Cull = Shader.PropertyToID("_Cull");
    private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    public Color color;
    public Rect rect;
    
    private void OnRenderObject()
    {
        if (!gameObject.activeInHierarchy)
            return;

        var material = GetLineMaterial();
        material.SetColor(Color1, color);
        material.SetPass(0);
        
        GL.PushMatrix();
        // GL.MultMatrix(transform.localToWorldMatrix);
        GL.LoadPixelMatrix();
        
        GL.Begin(GL.LINES);
        GL.Color(color);
        
        GL.Vertex3(rect.min.x, rect.min.y, 0);
        GL.Vertex3(rect.min.x, rect.max.y, 0);
        
        GL.Vertex3(rect.min.x, rect.max.y, 0);
        GL.Vertex3(rect.max.x, rect.max.y, 0);
        
        GL.Vertex3(rect.max.x, rect.max.y, 0);
        GL.Vertex3(rect.max.x, rect.min.y, 0);
        
        GL.Vertex3(rect.max.x, rect.min.y, 0);
        GL.Vertex3(rect.min.x, rect.min.y, 0);
        
        GL.End();
        GL.PopMatrix();
    }

    private static Material GetLineMaterial()
    {
        if (!_lineMaterial)
        {
            var shader = Prefabs.LoadShader("GizmoShader.shader");
            _lineMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            
            _lineMaterial.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _lineMaterial.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // _lineMaterial.SetInt(Cull, (int)UnityEngine.Rendering.CullMode.Off);
            // _lineMaterial.SetInt(ZWrite, 0);
        }

        return _lineMaterial;
    }

    public static DrawRect Create(Color color, Rect rect)
    {
        var go = new GameObject("DrawRectGizmo");
        go.SetActive(false);
        
        var instance = go.AddComponent<DrawRect>();
        instance.color = color;
        instance.rect = rect;
        
        go.SetActive(true);

        return instance;
    }
}