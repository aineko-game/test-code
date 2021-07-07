using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using TMPro;

public static class ExtensionsInt
{
    public static int Abs(this int self)
    {
        return Mathf.Abs(self);
    }

    public static int CalcDigit(this int self)
    {
        int digit = 1;
        for (int num = self.Abs(); num > 9; num /= 10)
        {
            digit++;
        }

        return (self >= 0) ? digit : digit + 1;
    }

    public static int Clamp(this int self, int min, int max)
    {
        return Mathf.Min(Mathf.Max(min, self), max);
    }

    public static int[] DevideToArray(this int self, int length)
    {
        int[] result = new int[length];
        for (int i = 0, sum = 0; i < length; i++)
        {
            result[i] = ((float)self * (i + 1) / length).ToInt() - sum;
            sum += result[i];
        }
        return result;
    }

    public static bool IsBetween(this int self, int min, int max)
    {
        return (self >= min && self <= max);
    }

    public static int Mod(this int self, int divisor)
    {
        return (self % divisor + divisor) % divisor;
    }

    public static int ReduceAbsValue(this int self, int value_)
    {
        if (self.Abs() < value_) return 0;
        return (self - self.Sign() * value_);
    }

    public static int Sign(this int self)
    {
        if (self == 0) return 0;
        if (self > 0) return 1;
        return -1;
    }

    public static int Square(this int self)
    {
        return (self * self);
    }
}

public static class ExtensionsFloat
{
    public static float Abs(this float self)
    {
        return Mathf.Abs(self);
    }

    public static float Ceil(this float self)
    {
        return Mathf.Ceil(self);
    }

    public static int CeilToInt(this float self)
    {
        return Mathf.CeilToInt(self);
    }

    public static float Clamp(this float self, float min, float max)
    {
        return Mathf.Min(Mathf.Max(min, self), max);
    }

    public static float Cube(this float self)
    {
        return self * self * self;
    }

    public static float Floor(this float self)
    {
        return Mathf.Floor(self);
    }

    public static int FloorToInt(this float self)
    {
        return Mathf.FloorToInt(self);
    }

    public static bool IsBetween(this float self, float min, float max)
    {
        return (self >= min && self <= max);
    }

    public static float Mod(this float self, float divisor)
    {
        return (self % divisor + divisor) % divisor;
    }

    public static float ReduceAbsValue(this float self, float value_)
    {
        if (self.Abs() < value_) return 0;
        return (self - self.Sign() * value_);
    }

    public static float Round(this float self)
    {
        return Mathf.Round(self);
    }

    public static int RoundToInt(this float self)
    {
        return Mathf.RoundToInt(self);
    }

    public static int Sign(this float self)
    {
        if (self == 0) return 0;
        if (self > 0) return 1;
        return -1;
    }

    public static float Square(this float self)
    {
        return (self * self);
    }

    public static byte ToByte(this float self)
    {
        return (byte)(self.ToInt().Clamp(0, 255));
    }

    public static float ToDegree(this float self)
    {
        return self / Mathf.PI * 180;
    }

    public static int ToInt(this float self)
    {
        return Mathf.RoundToInt(self);
    }

    public static float ToRadian(this float self)
    {
        return self / 180 * Mathf.PI;
    }

    public static float ToSigmoid(this float self, string type = "")
    {
        return -2 * self.Cube() + 3 * self.Square();
    }

    public static Vector3 ToVector3(this float self)
    {
        return new Vector3(self, self, self);
    }
}

public static class ExtensionsVector2
{
    public static Vector2 ClampByVector2(this Vector2 self, Vector2 v3Min, Vector2 v3Max)
    {
        return new Vector2(self.x.Clamp(v3Min.x, v3Max.x), self.y.Clamp(v3Min.y, v3Max.y));
    }

    public static Vector2 ClampByScalar(this Vector2 self, float min, float max)
    {
        return new Vector2(self.x.Clamp(min, max), self.y.Clamp(min, max));
    }

    public static bool IsInThisRect(this Vector2 self, Vector2 PosMin, Vector2 PosMax)
    {
        return (self.x.IsBetween(PosMin.x, PosMax.x) && 
                self.y.IsBetween(PosMin.y, PosMax.y));
    }

    public static Vector2 MulVector2(this Vector2 self, Vector2 v2)
    {
        return new Vector2(self.x * v2.x, self.y * v2.y);
    }

    public static Vector3 ScreenToGroundPoint(this Vector2 self)
    {
        Ray ray = Camera.main.ScreenPointToRay(self);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, /*layer = Ground*/ 1 << 8))
        {
            return hit.point;
        }
        else
        {
            return Vector3.down;
        }
    }

    public static float ToAngleFrom(this Vector2 self, Vector2 vectorBase)
    {
        return Vector2.SignedAngle(vectorBase, self);
    }

    public static float GetRadianFrom(this Vector2 self, Vector2 vectorBase)
    {
        return Vector2.SignedAngle(vectorBase, self).ToRadian();
    }

    public static Vector2Int ToVector2Int(this Vector2 self)
    {
        return new Vector2Int(self.x.ToInt(), self.y.ToInt());
    }
}

public static class ExtensionsVector3
{
    public static Vector3 ClampByLength(this Vector3 self, float lengthMax)
    {
        return self * (lengthMax / self.magnitude).Clamp(0, 1);
    }

    public static Vector3 ClampByVector3(this Vector3 self, Vector3 v3Min, Vector3 v3Max)
    {
        return new Vector3(self.x.Clamp(v3Min.x, v3Max.x), self.y.Clamp(v3Min.y, v3Max.y), self.z.Clamp(v3Min.z, v3Max.z));
    }

    public static Vector3 ClampByScalar(this Vector3 self, int min, int max)
    {
        return new Vector3(self.x.Clamp(min, max), self.y.Clamp(min, max), self.z.Clamp(min, max));
    }

    public static Vector3[] CubicPointsToLowerXZ(this Vector3[] self)
    {
        return new Vector3[4] { self[0], self[1], self[5], self[4] };
    }

    public static Vector3[] CubicPointsToMeanXZ(this Vector3[] self)
    {
        return new Vector3[4]
        {
            (self[0] + self[3]) * 0.5f,
            (self[1] + self[2]) * 0.5f,
            (self[4] + self[7]) * 0.5f,
            (self[5] + self[6]) * 0.5f
        };
    }

    public static Vector3[] CubicPointsToUpperXZ(this Vector3[] self)
    {
        return new Vector3[4] { self[3], self[2], self[6], self[7] };
    }

    public static float Dot(this Vector3 self, Vector3 v3)
    {
        return Vector3.Dot(self, v3);
    }

    public static Vector2 ToVector2_XZ(this Vector3 self)
    {
        return new Vector2(self.x, self.z);
    }
    
    public static Vector3 Inverse(this Vector3 self)
    {
        if (self.x == 0 || self.y == 0 || self.z == 0)
        {
            Debug.LogError("Vector3.Inverse() => Invalid value");
        }

        return new Vector3(1 / self.x, 1 / self.y, 1 / self.z);
    }

    public static bool IsInThisCuboid(this Vector3 self, Vector3 posMin, Vector3 posMax)
    {
        return (self.x.IsBetween(posMin.x, posMax.x) && self.y.IsBetween(posMin.y, posMax.y) && self.z.IsBetween(posMin.z, posMax.z));
    }

    public static Vector3 MulVector(this Vector3 self, Vector3 v3)
    {
        return new Vector3(self.x * v3.x, self.y * v3.y, self.z * v3.z);
    }

    public static Vector3 RotateThisByRadian(this Vector3 self, float rad_x, float rad_y, float rad_z)
    {
        Vector3 vector = self;
        vector = new Vector3(vector.x, vector.y * Mathf.Cos(rad_x) - vector.z * Mathf.Sin(rad_x), vector.y * Mathf.Sin(rad_x) + vector.z * Mathf.Cos(rad_x));
        vector = new Vector3(vector.x * Mathf.Cos(rad_y) + vector.z * Mathf.Sin(rad_y), vector.y, vector.x * -Mathf.Sin(rad_y) + vector.z * Mathf.Cos(rad_y));
        vector = new Vector3(vector.x * Mathf.Cos(rad_z) - vector.y * Mathf.Sin(rad_z), vector.x * Mathf.Sin(rad_z) + vector.y * Mathf.Cos(rad_z), vector.z);
        return vector;
    }

    public static void SetX(ref this Vector3 self, float x)
    {
        self.Set(x, self.y, self.z);
    }

    public static void SetY(ref this Vector3 self, float y)
    {
        self.Set(self.x, y, self.z);
    }

    public static void SetZ(ref this Vector3 self, float z)
    {
        self.Set(self.x, self.y, z);
    }

    public static Vector3[] ToCubicPoints(this Vector3 self, float width, float height, float length)
    {
        return new Vector3[8]
        {
            new Vector3(self.x - width / 2, self.y - height / 2, self.z - length / 2),
            new Vector3(self.x + width / 2, self.y - height / 2, self.z - length / 2),
            new Vector3(self.x + width / 2, self.y + height / 2, self.z - length / 2),
            new Vector3(self.x - width / 2, self.y + height / 2, self.z - length / 2),
            new Vector3(self.x - width / 2, self.y - height / 2, self.z + length / 2),
            new Vector3(self.x + width / 2, self.y - height / 2, self.z + length / 2),
            new Vector3(self.x + width / 2, self.y + height / 2, self.z + length / 2),
            new Vector3(self.x - width / 2, self.y + height / 2, self.z + length / 2)
        };
    }

    public static Vector2Int ToVector2Int(this Vector3 self)
    {
        return new Vector2Int(self.x.ToInt(), self.x.ToInt());
    }
}

public static class ExtensionsVector2Int
{
    public static int Min(this Vector2Int self)
    {
        return Mathf.Min(self.x, self.y);
    }

    public static int Max(this Vector2Int self)
    {
        return Mathf.Max(self.x, self.y);
    }

    public static Vector2Int ClampByScalar(this Vector2Int self, int min, int max)
    {
        return new Vector2Int(Mathf.Clamp(self.x, min, max), Mathf.Clamp(self.y, min, max));
    }

    public static bool IsAxial(this Vector2Int self)
    {
        return (self.x * self.y) == 0;
    }

    public static bool IsOctagonal(this Vector2Int self)
    {
        return ((self.x * self.y) == 0 || self.x.Abs() == self.y.Abs());
    }

    public static Vector2Int RotateDegree(this Vector2Int self, int degree)
    {
        degree = degree.Mod(360);

        if (degree == 0)
            return self;
        else if (degree == 45)
            return (self.x * self.y == 0) ? new Vector2Int(self.x - self.y, self.x + self.y) : new Vector2Int((self.x - self.y) / 2, (self.x + self.y) / 2);
        else if (degree == 90)
            return new Vector2Int(-self.y, self.x);
        else if (degree == 135)
            return (self.x * self.y == 0) ? new Vector2Int(-self.x - self.y, self.x - self.y) : new Vector2Int((-self.x - self.y) / 2, (self.x - self.y) / 2);
        else if (degree == 180)
            return new Vector2Int(-self.x, -self.y);
        else if (degree == 225)
            return (self.x * self.y == 0) ? new Vector2Int(-self.x + self.y, -self.x - self.y) : new Vector2Int((-self.x + self.y) / 2, (-self.x - self.y) / 2);
        else if (degree == 270)
            return new Vector2Int(self.y, -self.x);
        else if (degree == 315)
            return (self.x * self.y == 0) ? new Vector2Int(self.x + self.y, -self.x + self.y) : new Vector2Int((self.x + self.y) / 2, (-self.x + self.y) / 2);
        else
        {
            Debug.LogError("Invalid argument(int degree)");
            return self;
        }
    }

    public static Vector2Int Sign(this Vector2Int self)
    {
        return new Vector2Int(self.x.Clamp(-1, 1), self.y.Clamp(-1, 1));
    }

    public static Vector2Int Abs(this Vector2Int self)
    {
        return new Vector2Int(Mathf.Abs(self.x), Mathf.Abs(self.y));
    }

    public static int ToAngle(this Vector2Int self)
    {
        self = self.ClampByScalar(-1, 1);

        if (self == new Vector2Int(1, 0))
            return 0;
        else if (self == new Vector2Int(1, 1))
            return 45;
        else if (self == new Vector2Int(0, 1))
            return 90;
        else if (self == new Vector2Int(-1, 1))
            return 135;
        else if (self == new Vector2Int(-1, 0))
            return 180;
        else if (self == new Vector2Int(-1, -1))
            return 225;
        else if (self == new Vector2Int(0, -1))
            return 270;
        else if (self == new Vector2Int(1, -1))
            return 315;
        else
        {
            Debug.LogError("ExtensionMethods.Vector2.Rotate => invalid argument(Vector2Int " + self + " )");
            return 0;
        }
    }
}

public static class ExtensionsString //String
{
    public static bool IsNullOrEmpty(this string self)
    {
        return string.IsNullOrEmpty(self);
    }

    public static string Slice(this string self, int startIndex, int length)
    {
        if (self.Length > startIndex + length)
        {
            return self.Substring(startIndex, length);
        }
        else if (self.Length > startIndex)
        {
            return self.Substring(startIndex);
        }
        else
        {
            return "";
        }
    }

    public static Color32 ToColor32(this string self)
    {
        self = self.Replace("0x", "").Replace("#", "");

        if ((self.Length == 6 || self.Length == 8) &&
            byte.TryParse(self.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, default, out byte r) &&
            byte.TryParse(self.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, default, out byte g) &&
            byte.TryParse(self.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, default, out byte b))
        {
            if (self.Length == 8 &&
                byte.TryParse(self.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, default, out byte a))
            {
                return new Color32(r, g, b, a);
            }
            else
            {
                return new Color32(r, g, b, 255);
            }
        }
        else
        {
            Debug.LogError("Invalid argument");
            return new Color32(255, 255, 255, 255);
        }
    }

    public static Vector3 ToVector3(this string self)
    {
        Vector3 result = default;
        self = self.Replace("new ", "").Replace("Vector3", "").Replace("f", "");
        string[] strElems = self.Trim('(', ')').Split(',');

        for (int i_ = 0; i_ < strElems.Length.Clamp(0, 3); i_++)
        {
            if (float.TryParse(strElems[i_], out float value_))
            {
                result[i_] = value_;
            }
            else
            {
                Debug.LogError("Invalid value : " + self);
            }
        }
        return result;
    }
}

public static class ExtensionsGameObject
{
    public static void DestroyAllChildren(this GameObject self)
    {
        foreach (Transform child in self.transform)
        {
            Object.Destroy(child.gameObject);
        }
    }

    public static Transform Find(this GameObject self, string key_)
    {
        return self.transform.Find(key_);
    }

    public static T[] GetComponentsInChildrenOnlyInactive<T>(this GameObject self) where T: Component
    {
        List<T> result = new List<T>(self.GetComponentsInChildren<T>(true));

        for (int i = result.Count - 1; i >= 0; i--)
        {
            if (result[i].gameObject.activeSelf == true)
            {
                result.RemoveAt(i);
            }
        }
        return result.ToArray();
    }

    public static T[] GetComponentsInChildrenWithoutSelf<T>(this GameObject self, bool isIncludeInactive = false) where T : Component
    {
        List<T> result = new List<T>();

        foreach (Transform child in self.transform)
        {
            result.AddRange(child.GetComponentsInChildren<T>(isIncludeInactive));
        }

        return result.ToArray();
    }

    public static T GetRandom<T>(this T[] self, _Random random_ = default)
    {
        if (random_ == default)
            random_ = new _Random();

        if (self.Length == 0)
        {
            Debug.LogError("Array.Length is 0.");
            return default;
        }
        return self[random_.Range(0, self.Length)];
    }


}

public static class ExtensionsTransform
{
    public static Transform FindTag(this Transform self, string tag)
    {
        foreach (Transform child in self)
        {
            if (child.tag == tag)
            {
                return child;
            }
        }
        Debug.LogError("Invalid tag : " + tag);
        return default;
    }

    public static Transform FindTagInChildren(this Transform self, string tag)
    {
        Transform[] children = self.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.tag == tag)
            {
                return child;
            }
        }
        Debug.LogError("Invalid tag : " + tag);
        return default;
    }

    public static GameObject[] GetChildrenGameObject(this Transform self)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (Transform child in self)
        {
            result.Add(child.gameObject);
        }

        return result.ToArray();
    }

    public static T[] GetComponentsInChildrenOnlyInactive<T>(this Transform self) where T : Component
    {
        List<T> result = new List<T>(self.GetComponentsInChildren<T>(true));

        for (int i = result.Count - 1; i >= 0; i--)
        {
            if (result[i].gameObject.activeSelf == true)
            {
                result.RemoveAt(i);
            }
        }
        return result.ToArray();
    }

    public static T[] GetComponentsInChildrenWithoutSelf<T>(this Transform self, bool isIncludeInactive = false) where T : Component
    {
        List<T> List = new List<T>();

        foreach (Transform child in self)
        {
            Debug.Log(child.name);
            List.AddRange(child.GetComponentsInChildren<T>(isIncludeInactive));
        }

        return List.ToArray();
    }
}

//public static class ExtensionMethodsArray2D
//{
//    //#ExtensionMethods T[,]
//    public static T GetRandom<T>(this T[,] self)
//    {
//        if (self.GetLength(0) * self.GetLength(1) == 0)
//        {
//            Debug.LogError("ExtensionMethod.GetRandom<T> => self.GetLength(0) * self.GetLength(1) == 0");
//        }
//        return (self.GetLength(0) * self.GetLength(1) != 0) ? self[Random.Range(0, self.GetLength(0)), Random.Range(0, self.GetLength(1))] : default(T);
//    }
//}


public static class ExtensionsT
{
    public static bool IsContains<T>(this T[] self, T item)
    {
        foreach (T elem in self)
        {
            if (EqualityComparer<T>.Default.Equals(elem, item))
                return true;
        }
        return false;
    }

    public static T DeepCopy<T>(this T self)
    {
        T result;
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        try
        {
            bf.Serialize(ms, self);
            ms.Position = 0;
            result = (T)bf.Deserialize(ms);
        }
        finally
        {
            ms.Close();
        }

        return result;
    }

    public static bool IsEqual<T>(this T self, T other)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms00 = new MemoryStream())
        {
            bf.Serialize(ms00, self);
            using (MemoryStream ms01 = new MemoryStream())
            {
                bf.Serialize(ms01, other);
                ms00.Position = 0;
                ms01.Position = 0;
                byte[] a = new byte[ms00.Length];
                byte[] b = new byte[ms01.Length];
                ms00.Read(a, 0, a.Length);
                ms01.Read(b, 0, b.Length);

                return a.SequenceEqual(b);
            }
        }
    }

    public static T GetIndexOf<T>(this T[] self, int index)
    {
        if (self == null || index >= self.Length) return default;
        return self[index];
    }

    public static T GetRandom<T>(this List<T> self)
    {
        if (self.Count == 0)
        {
            Debug.LogError("List.Count == 0");
            return default;
        }
        return self[Random.Range(0, self.Count)];
    }

    public static T[] GetRandom<T>(this List<T[]> self)
    {
        if (self.Count == 0)
        {
            Debug.LogError("List.Count == 0");
            return default;
        }
        return self[Random.Range(0, self.Count)];
    }

    public static T Last<T> (this List<T> self)
    {
        if (self.Count == 0)
            return default;
        else
            return self[self.Count - 1];
    }

    public static T Last<T> (this T[] self)
    {
        if (self.Length == 0)
            return default;
        return self[self.Length - 1];
    }

    public static T[] Shuffle<T>(this T[] self, _Random random = default)
    {
        if (random == default) random = new _Random();

        T[] newArray = new T[self.Length];
        self.CopyTo(newArray, 0);
        for (int i = 0; i < newArray.Length; i++)
        {
            T temp = newArray[i];
            int randomIndex = random.Range(i, newArray.Length);
            newArray[i] = newArray[randomIndex];
            newArray[randomIndex] = temp;
        }

        return newArray;
    }

    public static List<T> Shuffle<T>(this List<T> self, _Random random = default)
    {
        List<T> result = new List<T>(self);

        if (random == default) random = new _Random();

        for (int i = 0; i < result.Count; i++)
        {
            int indexRandom = random.Range(i, result.Count);
            T temp = result[i];
            result[i] = result[indexRandom];
            result[indexRandom] = temp;
        }

        return result;
    }
}

public static class ExtensionMethodDictionary
{
    public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue = default)
    {
        if (self == null)
        {
            self = new Dictionary<TKey, TValue>();
        }
        
        if (self.ContainsKey(key) == false)
        {
            return defaultValue;
        }

        return self[key];
    }
}

//public static class ExtensionMethodsList
//{
//    //#ExtensionMethods List<T>
//    public static T GetRandom<T>(this List<T> self)
//    {
//        if (self.Count == 0)
//        {
//            Debug.LogError("ExtensionMethods.GetRandom<T> => self.Count == 0");
//        }
//        return (self.Count != 0) ? self[Random.Range(0, self.Count)] : default(T);
//    }

//    public static List<T> Shuffle<T>(this List<T> self)
//    {
//        List<T> newList = new List<T>(self);
//        for (int i = 0; i < self.Count; i++)
//        {
//            T temp = newList[i];
//            int randomIndex = Random.Range(i, newList.Count);
//            newList[i] = newList[randomIndex];
//            newList[randomIndex] = temp;
//        }
//        return newList;
//    }

//    public static List<T> Unique<T>(this List<T> self) where T : class
//    {
//        List<T> newList = new List<T>();

//        for (int i = 0; i < self.Count; i++)
//        {
//            bool flag = true;

//            for (int j = 0; j < newList.Count; j++)
//            {
//                if (ReferenceEquals(self[i], newList[j]))
//                {
//                    flag = false;
//                    break;
//                }
//            }
//            if (flag == true)
//                newList.Add(self[i]);
//        }

//        return newList;
//    }

//    public static void DebugLog<T>(this List<T> self)
//    {
//        string output = "";
//        for (int i = 0; i < self.Count; i++)
//        {
//            output += self[i].ToString() + " , ";
//        }

//        Debug.Log(output);
//    }

//}

//public static class EstensionMethodsRectTransform
//{
//    public static void CopyTo(this RectTransform self, RectTransform rt)
//    {
//        rt.anchorMax = self.anchorMax;
//        rt.anchorMin = self.anchorMin;
//        rt.pivot = self.pivot;
//        rt.position = self.position;
//        rt.sizeDelta = self.sizeDelta;
//        rt.rotation = self.rotation;
//        rt.localScale = self.localScale;
//    }
//}

public static class ExtensionsRectTransform
{
    public static void StretchToThisAnchor(this RectTransform self, Vector2 anchorMin, Vector2 anchorMax)
    {
        self.anchorMin = anchorMin;
        self.anchorMax = anchorMax;
        self.anchoredPosition = Vector3.zero;
        self.sizeDelta = Vector3.zero;
    }

    public static Vector3[] GetWorldCornersPos(this RectTransform self)
    {
        Vector3[] result = new Vector3[4];
        self.GetWorldCorners(result);
        return result;
    }
}

public static class ExtensionsTMPro
{
    public static string Text(this TMP_Dropdown self)
    {
        return self.options[self.value].text;
    }
}