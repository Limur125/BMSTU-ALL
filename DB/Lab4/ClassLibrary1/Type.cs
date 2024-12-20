﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
namespace ClassLibrary1
{
    [Serializable]
    [SqlUserDefinedType(Format.UserDefined, MaxByteSize = 8000)]
    public struct Email : INullable, IBinarySerialize
    {
        private string _email;
        private bool _null;
        public override string ToString()
        {
            return _email;
        }
        public bool IsNull
        {
            get
            {
                return _null;
            }
        }
        public static Email Null
        {
            get
            {
                Email h = new Email();
                h._null = true;
                return h;
            }
        }
        public static Email Parse(SqlString s)
        {
            if (s.IsNull)
                return Null;
            Email u = new Email();
            string str = s.ToString();
            if (isValidEmail(str))
            {
                u._email = str;
            }
            else
            {
                throw new Exception("Invalid data format");
            };
            return u;
        }
        private static bool isValidEmail(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }
        void IBinarySerialize.Read(System.IO.BinaryReader r)
        {
            this._email = r.ReadString();
        }
        void IBinarySerialize.Write(System.IO.BinaryWriter w)
        {
            w.Write(this._email);
        }
    }
}