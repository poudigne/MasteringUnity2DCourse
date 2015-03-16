using UnityEngine;
using System.Collections;

public class FadeinOutManager : Singleton<FadeinOutManager>
{
  private Material fadeMaterial;
  private float fadeOutTime, fadeInTime;
  private Color fadeColor;

  private string navigateToLevelName = "";
  private int navigateToLevelIndex = 0;
  private bool fading = false;
  
  protected FadeinOutManager() { }

  void Awake()
  {
    fadeMaterial = new Material("Shader \"Plane/No zTest\" {"+
    "Subshader { Pass { "+
    "   Blend SrcAlpha OneMinusSrcAlpha "+
    "   ZWrite Off Cull Off Fog { Mode Off } "+
    "   BindChannels {" +
    "     Bind \"color\", color } "+
    "}  } }" );
  }

  public static void FadeToLevel(string levelName, float fadeOutTime, float fadeInTime, Color color)
  {
    if (Fading) return;
    Instance.navigateToLevelName = levelName;
    Instance.StartFade(fadeOutTime, fadeInTime, color);
  }


  private void StartFade(float fadeOutTime, float fadeInTime, Color color)
  {
    fading = true;
    Instance.fadeOutTime = fadeOutTime;
    Instance.fadeInTime = fadeInTime;
    Instance.fadeColor = color;

    StopAllCoroutines();
    StartCoroutine("Fade");
  }

  public static bool Fading
  {
    get { return Instance.fading; }
  }

  private IEnumerator Fade()
  {
    float t = 0.0f;
    while (t < 1.0f)
    {
      yield return new WaitForEndOfFrame();
      t = Mathf.Clamp01(t + Time.deltaTime/fadeOutTime);

      DrawingUtilities.DrawQuad(fadeMaterial, fadeColor, t);
    }
    if (navigateToLevelName != "")
      Application.LoadLevel(navigateToLevelName);
    else
      Application.LoadLevel(navigateToLevelIndex);

    while (t > 0.0f)
    {
      yield return new WaitForEndOfFrame();
      t = Mathf.Clamp01(t - Time.deltaTime/fadeInTime);

      DrawingUtilities.DrawQuad(fadeMaterial, fadeColor, t);
    }
    fading = false;
  }
	  
}

public static class DrawingUtilities
{
  public static void DrawQuad(Material aMaterial, Color aColor, float aAlpha)
  {
    aColor.a = aAlpha;
    aMaterial.SetPass(0);
    GL.PushMatrix();
    GL.LoadOrtho();
    GL.Begin(GL.QUADS);
    GL.Color(aColor);
    GL.Vertex3(0, 0, -1);
    GL.Vertex3(0, 1, -1);
    GL.Vertex3(1, 1, -1);
    GL.Vertex3(1, 0, -1);
    GL.End();
    GL.PopMatrix();
  }

  
}
