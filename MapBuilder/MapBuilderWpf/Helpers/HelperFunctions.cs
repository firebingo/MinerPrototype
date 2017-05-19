using System;
using System.Windows.Media;

namespace MapBuilderWpf.Helpers
{
	public static class HelperFunctions
	{
		public static Color lerpColors(Color c1, Color c2, float lerpValue)
		{
			lerpValue = clampValue<float>(lerpValue, 0.0f, 1.0f);
			return Color.Add(c1, (Color.Multiply(Color.Subtract(c2,c1), lerpValue)));
			//Color retval = new Color();
			//int newA = rangeInt((int)(c1.A + (c2.A - c1.A) * lerpValue),255,0);
			//int newR = rangeInt((int)(c1.R + (c2.R - c1.R) * lerpValue),255,0);
			//int newG = rangeInt((int)(c1.G + (c2.G - c1.G) * lerpValue),255,0);
			//int newB = rangeInt((int)(c1.B + (c2.B - c1.B) * lerpValue),255,0);
			//retval = Color.FromArgb((byte)newA, (byte)newR, (byte)newG, (byte)newB);
			//return retval;
		}

		public static T clampValue<T>(T i, T lower, T upper) where T : IComparable
		{
			if (i.CompareTo(upper) > 0)
				return upper;
			else if (i.CompareTo(lower) < 0)
				return lower;
			else
				return i;
		}

		public static bool hasProperty(dynamic obj, string name)
		{
			Type objType = obj.GetType();
			return objType.GetProperty(name) != null;
		}
	}
}
