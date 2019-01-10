using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AL.Tools
{
    public static class Extensions
    {
        /// <summary>
        /// Extension to validate whether a string is empty or not, checks for:
        /// <list type="bullet">
        /// <item><code>NULL</code></item>
        /// <item><code>String.Empty</code></item>
        /// <item>White Space</item>
        /// </list>
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsEmpty(this string str)
        {
            return (String.IsNullOrEmpty(str) && String.IsNullOrWhiteSpace(str));
        }

        /// <summary>
        /// Extension to validate whether a string is empty or not, checks for:
        /// <list type="bullet">
        /// <item><code>NULL</code></item>
        /// <item><code>String.Empty</code></item>
        /// <item>White Space</item>
        /// </list>
        /// Also if specified, performs a <code>Trim</code> on the String prior validation.
        /// </summary>
        /// <param name="Trim">Specify as <code>True</code> to perform a <code>Trim</code>.</param>
        /// <returns>Boolean</returns>
        public static bool IsEmpty(this string str, bool Trim)
        {
            if (Trim)
                str = str.Trim();
            return (String.IsNullOrEmpty(str) && String.IsNullOrWhiteSpace(str));
        }

        /// <summary>
        /// Extension to validate wether a string has information.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsNotEmpty(this string str)
        {
            return !IsEmpty(str);
        }

        /// <summary>
        /// Extension to validate if an Object is <code>NULL</code>
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsNull(this object obj)
        {
            return (obj == null);
        }

        /// <summary>
        /// Extension to validate if an Object is not <code>NULL</code> 
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsNotNull(this object obj)
        {
            return !IsNull(obj);
        }

        /// <summary>
        /// Extension to validate wether the content of a string is a number.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsNumber(this String str)
        {
            try
            {
                Convert.ToInt64(str);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Extension to validate wether the content of a string is not a number.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsNotANumber(this String str)
        {
            return !IsNumber(str);
        }

        /// <summary>
        /// Extension to validate wether the content of a string is a DateTime.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsDateTime(this String str)
        {
            try
            {
                Convert.ToDateTime(str);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Extension to validate wether the content of a string is not a DateTime.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsNotDateTime(this String str)
        {
            return !IsDateTime(str);
        }

        /// <summary>
        /// Extension to validate wether a DateTime has the default value "MinDate"
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsMinDate(this DateTime dt)
        {
            return (dt == DateTime.MinValue);
        }

        /// <summary>
        /// Extension to validate wether a DateTime has a value different than "MinDate"
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsNotMinDate(this DateTime dt)
        {
            return !IsMinDate(dt);
        }

        /// <summary>
        /// Extension to validate if a DataSet has DataTables
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool HasTables(this DataSet dsData)
        {
            return (dsData != null && dsData.Tables.Count > 0);
        }

        /// <summary>
        /// Extension that returns wether the DataSet has at least 1 Table and that that the first DataTable has records.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool HasRows(this DataSet dsData)
        {
            return (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0);
        }

        /// <summary>
        /// Extension that returns wether the DataTable has records.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool HasRows(this DataTable dtData)
        {
            return (dtData.Rows.Count > 0);
        }

        /// <summary>
        /// Extension to validate wether the content of a string is an IP Address.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsIPAddress(this String str)
        {
            try
            {
                IPAddress.Parse(str);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Extension to validate wether the content of a string is not an IP Address.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsNotIPAddress(this String str)
        {
            return !IsIPAddress(str);
        }
    }
}
