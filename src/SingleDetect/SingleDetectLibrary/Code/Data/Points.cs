using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Kunukn.SingleDetectLibrary.Code.Contract;

namespace Kunukn.SingleDetectLibrary.Code.Data
{
    public class Points : IPoints, ISerializable
    {
        public List<IP> Data { get; set; }
        public Points()
        {
            Data = new List<IP>();
        }

        public IP this[int i]
        {
            get { return Data[i]; }
            set { Data[i] = value; }
        }

        public void Add(IP p)
        {
            Data.Add(p);
        }

        public int Count { get { return Data.Count; } }


        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in Data) sb.Append(string.Format("[{0}] ", p));
            return sb.ToString();
        }
       
        public void Round(int decimals)
        {
            foreach (var p in Data)
            {
                p.X = Math.Round(p.X, decimals);
                p.Y = Math.Round(p.Y, decimals);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Data", this.Data);
        }

    }
}
