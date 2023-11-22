using System.Globalization;
using UnityEngine;

public class UIPalette
{
    public static readonly Color white          = Color.white;
    public static readonly Color black          = Color.black;
    public static readonly Color cyanLight      = FromHex("1abc9c");
    public static readonly Color cyanDark       = FromHex("16a085");
    public static readonly Color greenLight     = FromHex("2ecc71");
    public static readonly Color greenDark      = FromHex("27ae60");
    public static readonly Color blueLight      = FromHex("3498db");
    public static readonly Color blueDark       = FromHex("2980b9");
    public static readonly Color purpleLight    = FromHex("9b59b6");
    public static readonly Color purpleDark     = FromHex("8e44ad");
    public static readonly Color blueGraDark    = FromHex("34495e");
    public static readonly Color blueGrayDarker = FromHex("2c3e50");
    public static readonly Color yellowLight    = FromHex("f1c40f");
    public static readonly Color yellowDark     = FromHex("f39c12");
    public static readonly Color orangeLight    = FromHex("e67e22");
    public static readonly Color orangeDark     = FromHex("d35400");
    public static readonly Color redLight       = FromHex("e74c3c");
    public static readonly Color redDark        = FromHex("c0392b");
    public static readonly Color grayLightest   = FromHex("ecf0f1");
    public static readonly Color grayLight      = FromHex("bdc3c7");
    public static readonly Color grayMid        = FromHex("95a5a6");
    public static readonly Color grayDark       = FromHex("7f8c8d");

    public static Color FromHex(string hex)
    {
        if (hex.Length < 6)
        {
            throw new System.FormatException("Needs a string with a length of at least 6");
        }

        var r = hex.Substring(0, 2);
        var g = hex.Substring(2, 2);
        var b = hex.Substring(4, 2);
        string alpha;
        if (hex.Length >= 8)
            alpha = hex.Substring(6, 2);
        else
            alpha = "FF";

        return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
                        (int.Parse(g, NumberStyles.HexNumber) / 255f),
                        (int.Parse(b, NumberStyles.HexNumber) / 255f),
                        (int.Parse(alpha, NumberStyles.HexNumber) / 255f));
    }
}
