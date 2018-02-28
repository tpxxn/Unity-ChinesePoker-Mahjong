using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 扩展类:
/// 王拓
/// </summary>
internal static class HelperClass
{
    internal static string MapGetString<T>(this Dictionary<int, T> map)
    {
        if (map == null)
        {
            return null;
        }
        List<string> list = new List<string>(map.Select(keyValuePair => "[" + keyValuePair.Key + ":" + keyValuePair.Value + "]"));
        var toString = string.Join(", ", list.ToArray());
        return toString;
    }

    internal static Tvalue GetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue value;
        dict.TryGetValue(key, out value);
        return value;
    }

    public static T LoadObjectByAb<T>(this AssetBundle ab, string abName) where T : UnityEngine.Object
    {
        return ab.LoadAsset<T>(abName);
    }

    internal static string EncodingString(this byte[] self)
    {
        return System.Text.Encoding.UTF8.GetString(self);
    }

    internal static string SerializerToJson(this object self)
    {
        return MySerializerUtil.SerializerToJson(self);
    }

    internal static T DeSerialFromJson<T>(this string self)
    {
        return MySerializerUtil.DeSerialFromJson<T>(self);
    }

    internal static T DeSerialFromJson<T>(this byte[] self)
    {
        return MySerializerUtil.DeSerialFromJson<T>(self);
    }

    internal static byte[] GetBytes(this string self)
    {
        return GetSBytesForEncoding_byte(System.Text.Encoding.UTF8, self);
    }

    private static byte[] GetSBytesForEncoding_byte(System.Text.Encoding encoding, string s)
    {
        byte[] sbytes = new byte[encoding.GetByteCount(s)];
        encoding.GetBytes(s, 0, s.Length, (byte[])(object)sbytes, 0);
        return sbytes;
    }


}
 
internal static class ListHelperClass
{
    internal static  string GetString<T>(this List<T> list)
    {
        if (list == null)
        {
            return null;
        }
        StringBuilder sb = new StringBuilder();
        foreach (var item in list)
        {
            sb.Append(item);
            sb.Append(",");
        }

        return sb.ToString();
    }
}
