using System;
using System.IO;
using System.Text.RegularExpressions;
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

		/// <summary>
		/// Blends a opaque and semi transparent color using normal blending.
		/// </summary>
		/// <param name="back"></param>
		/// <param name="front"></param>
		/// <returns></returns>
		public static Color normalBlendColor(Color back, Color front)
		{
			float r = front.ScA * front.ScR + back.ScR * (1 - front.ScA);
			float g = front.ScA * front.ScG + back.ScG * (1 - front.ScA);
			float b = front.ScA * front.ScB + back.ScB * (1 - front.ScA);
			return Color.FromScRgb(1, r, g, b);
			//var result = Color.Multiply(Color.Multiply(Color.Add(front, back), front.ScA), (1 - front.ScA));
			//result.ScA = 1;
			//return result;
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

		/// <summary>
		/// Check if a dynamic object has a property.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool hasProperty(dynamic obj, string name)
		{
			Type objType = obj.GetType();
			return objType.GetProperty(name) != null;
		}

		/// <summary>
		/// Checks if a string contains invalid characters for a file name.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool isValidFilename(string filename)
		{
			Regex reg = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]");
			if (reg.IsMatch(filename))
				return false;

			return true;
		}

		/// <summary>
		/// Removes invalid characters from a string for usage as a filename.
		/// Does not check 
		/// </summary>
		/// <param name="filename"></param>
		/// <returns>Returns string with invalid characters replaced with a _</returns>
		public static string escapeFilename(string filename)
		{
			if (isReservedName(filename))
				filename = "reserved";

			string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
			string invalidRegStr = $@"([{invalidChars}]*\.+$)|([{invalidChars}]+)";

			return Regex.Replace(filename, invalidRegStr, "_");
		}

		/// <summary>
		/// Checks if a string is a reserved filename in windows.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private static bool isReservedName(string filename)
		{
			Regex names = new Regex("^COM[0-9]$|^LPT[0-9]$|^CON$|^AUX$|^PRN$|^NUL$", RegexOptions.IgnoreCase);
			return names.IsMatch(filename);
		}
	}
}
