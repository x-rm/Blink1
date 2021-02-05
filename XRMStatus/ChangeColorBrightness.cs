using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace XRMStatus
{
	/// <summary>
	/// Change color brightness
	/// https://gist.github.com/zihotki/09fc41d52981fb6f93a81ebf20b35cd5
	/// </summary>
	public static class ColorHelper 
	{
		public static Color ChangeBrightness(Color color, float correctionFactor)
		{
			float red = (float) color.R;
			float green = (float) color.G;
			float blue = (float) color.B;

			if (correctionFactor < 0)
			{
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			}
			else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int) red, (int) green, (int) blue);
		}
	}
}
