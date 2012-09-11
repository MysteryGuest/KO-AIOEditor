using System;
using System.Data;
using System.Data.Common;

namespace MysteryGuest_INC
{
    class DBParameter
    {
        private object _data;
        private Type _type;

        private void _DBParameter(object data, Type type)
        {
            _data = data;
            _type = type;
        }

        // NOTE: Avoid using generics here, lest we end up having to use reflection.
        public DBParameter(string data)
        {
            _DBParameter(data, data.GetType());
        }

        public DBParameter(int data)
        {
            _DBParameter(data, data.GetType());
        }

        public DBParameter(short data)
        {
            _DBParameter(data, data.GetType());
        }

        public DBParameter(byte data)
        {
            _DBParameter(data, data.GetType());
        }

        public DBParameter(byte[] data)
        {
            _DBParameter(data, data.GetType());
        }

        public Type GetDataType()
        {
            return _type;
        }

        public object GetData()
        {
            return _data;
        }

        public bool CopyParameter(ref DbParameter param)
        {
            switch (GetDataType().Name)
            {
                case "String":
                    param.DbType = DbType.String;
                    break;

                case "Int32":
                case "UInt32":
                    param.DbType = DbType.Int32;
                    break;

                case "Int16":
                case "UInt16":
                    param.DbType = DbType.Int16;
                    break;

                case "SByte":
                case "Byte":
                    param.DbType = DbType.SByte;
                    break;

                default:
                    return false;
            }

            param.Value = GetData();
            return true;
        }
    }
}