using UnityEngine;
using CameraFading;

[CreateAssetMenu(fileName = "ButtonFunctions", menuName = "ScriptableObject/ButtonFunctions", order = 0)]
public class ButtonFunctions : ScriptableObject
{
	public void FadeIn()
	{
		CameraFade.In(2f);
	}

	public void FadeInWithCallback()
	{
		CameraFade.In(() =>
		{
			Debug.Log("fade in finished");
		},2f);
	}

	public void RestartAndFadeIn()
	{
		CameraFade.In(2f, true);
	}

	public void FixedDurationFadeIn()
	{
		CameraFade.In(2f, false, true);
	}

	public void RestartAndFadeOut()
	{
		CameraFade.Out(2f, true);
	}

	public void FadeOut()
	{
		CameraFade.Out(2f);
	}

	public void FadeOutWithCallback()
	{
		CameraFade.Out(() =>
		{
			Debug.Log("fade out finished");
		}, 2f);
	}

	public void FixedDurationFadeOut()
	{
		CameraFade.Out(2f, false, true);
	}

	public void ColorBlue()
	{
		CameraFade.Color = Color.blue;
	}

	public void ColorWhite()
	{
		CameraFade.Color = Color.white;
	}

	public void ColorBlack()
	{
		CameraFade.Color = Color.black;
	}
}
