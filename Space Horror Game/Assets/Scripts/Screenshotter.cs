using UnityEngine;

/// <summary>
/// Handles screenshots and puts them in <see cref="Application.persistentDataPath"/>, locallow for computers
/// </summary>
public static class Screenshotter
{
	static string FileLocation => $"{Application.persistentDataPath}/Screenshots";

	/// <summary>
	/// Takes a screenshot of the current scene.
	/// </summary>
	public static void TakeScreenshot()
	{
		Console.Log("Taking Screenshot");
		if (!System.IO.Directory.Exists(FileLocation)) System.IO.Directory.CreateDirectory(FileLocation);
		ScreenCapture.CaptureScreenshot(GetScreenshotName());
	}

	/// <summary>
	/// Gets the filename for a screenshot.
	/// </summary>
	private static string GetScreenshotName()
	{
		System.DateTime now = System.DateTime.Now;
		string size = $"{Camera.main.pixelWidth},{Camera.main.pixelHeight}";
		string date = $"{now.Month},{now.Day},{now.Year}";
		string time = $"{now.Hour},{now.Minute},{now.Second}";
		return $"{FileLocation}/Creme-{date}-{time}-{size}.png";
	}
}