﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace AL.Tools
{
    public static class Converters
    {
        readonly static string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string ToXML<T>(this T obj)
        {
            using (var stringWriter = new StringWriter(new StringBuilder()))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                var XmlString = stringWriter.ToString();
                XmlString = XmlString.Replace("<?xml version=\"1.0\" encoding=\"utf - 16\"?>", "");
                XmlString = XmlString.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                XmlString = XmlString.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                return XmlString;
            }
        }

        public static T FromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());
            var textWriter = new Utf8StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }

        public static bool ToBoolean(this string str)
        {
            if (str.IsNull())
                return false;

            switch (str.ToLower())
            {
                case "true":
                case "t":
                case "1":
                    return true;
                case "0":
                case "false":
                case "f":
                case "":
                    return false;
                default:
                    throw new InvalidCastException("You can't cast a weird value to a bool!");
            }
        }

        public static bool ToBoolean(this object obj)
        {
            return Convert.ToBoolean(obj);
        }

        public static DateTime ToDateTime(this object obj)
        {
            return Convert.ToDateTime(obj);
        }

        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static int ToInt(this Decimal obj)
        {
            return Convert.ToInt32(obj);
        }

        public static long ToLong(this object obj)
        {
            return Convert.ToInt64(obj);
        }

        public static float ToSingle(this double number)
        {
            return Convert.ToSingle(number);
        }

        public static Image ToImage(this byte[] img)
        {
            byte[] imageData = (byte[])img;

            Image newImage;

            using (var ms = new MemoryStream(imageData, 0, imageData.Length))
            {
                ms.Write(imageData, 0, imageData.Length);

                newImage = Image.FromStream(ms, true);
            }

            return newImage;
        }

        public static Guid ToGuid(this string str)
        {
            return new Guid(str);
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static string ToReadableFileSize(this long Size)
        {
            const long Base = 1024;
            //In case negative value is provided
            Size = Math.Abs(Size);
            //Check for zero
            if (Size == 0) { return "0.0 bytes"; }

            //Get the unit based on the logarithm of the number
            int Magnitude = (int)Math.Log(Size, Base);

            //Get the size in the unit
            double adjustedSize = (Size / Math.Pow(Base, Magnitude));

            //Return readable string
            return string.Format("{0:n2} {1}", adjustedSize, SizeSuffixes[Magnitude]);
        }

        public static string ToJSON(this object obj)
        {
            var srlz = new JavaScriptSerializer();
            return srlz.Serialize(obj);
        }

        public static string ToJSON(this object obj, int recursionDepth)
        {
            var srlz = new JavaScriptSerializer();
            srlz.RecursionLimit = recursionDepth;
            return srlz.Serialize(obj);
        }

        public static T JSONtoList<T>(this object jsonObj)
        {
            var _jsserializer = new JavaScriptSerializer();
            return _jsserializer.Deserialize<T>(jsonObj as string);
        }

        public static IPAddress ToIPAddress(this string str)
        {
            return IPAddress.Parse(str);
        }

        public static string ToOracleParameterString(this DateTime dt)
        {
            if (dt.IsMinDate())
                return "NULL";
            else
                return $"'{dt.ToString("yyyy-MM-dd HH:mm:ss")}'";
        }

        public static string ToStringNullSafe(this DateTime dt, string Pattern = "yyyy-MM-dd HH:mm:ss")
        {
            if (dt.IsMinDate())
                return string.Empty;
            else
                return $"{dt.ToString(Pattern)}";
        }

        public static byte[] EncodeString(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static string DecodeString(this byte[] byteArray)
        {
            return Encoding.ASCII.GetString(byteArray);
        }

        public static string DecodeString(this byte[] byteArray, int Index, int Count)
        {
            return Encoding.ASCII.GetString(byteArray, Index, Count);
        }

        public static byte[] EncodeObject(this object obj)
        {
            if (obj.IsNull())
                return null;

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        public static object DecodeObject(this byte[] ba)
        {
            if (ba.IsNull() || ba.Length == 0)
                return null;

            var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            ms.Write(ba, 0, ba.Length);
            ms.Seek(0, SeekOrigin.Begin);

            return (bf.Deserialize(ms) as object);
        }

        public static string ToBase(this int num, int Base)
        {
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (num < 0)
                throw new Exception("Negative numbers not supported.");

            // check if we can convert to another base
            if (Base < 2 || Base > chars.Length)
                throw new Exception("Only Bases from 2 to 36 are supported.");

            string newNumber = "";
            
            while (num >= Base)
            {
                var remainder = num % Base;
                newNumber += chars[remainder];
                num = num / Base;
            }
            // the last number to convert
            newNumber += chars[num];

            char[] array = newNumber.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static string ToCSV(this List<int> eList)
        {
            var str = string.Empty;

            foreach (var e in eList)
                str += $"{e}, ";

            str = str.TrimEnd();
            str = str.TrimEnd(',');

            return str;
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        // Use UTF8 encoding but write no BOM to the wire
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); } // in real code I'll cache this encoding.
        }
    }
}
