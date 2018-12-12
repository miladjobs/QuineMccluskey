using System;
using System.Collections.Generic;
using System.Dynamic;

namespace QuineMaccluskey
{
    public class Implicant  : IEquatable<Implicant>
    {
        public List<Minterm> Minterms { get; set; }
        public string BinaryCode { get; private set; }
        public bool Status { get; set; }

        public Implicant(List<Minterm> minterms)
        {
            this.Minterms = minterms;
            this.Status = true;
            this.BinaryCode = minterms[0].BinaryCode;
        }

        public override string ToString()
        {
            string result = "";
            int numberOfValue = this.BinaryCode.Length;
            for (int i = 0; i < this.BinaryCode.Length; i++)
            {
                string innerResult = "";
                if (this.BinaryCode[i] == '1')
                {
                    innerResult += ((char)(65 + i)).ToString();
                }
                else if (this.BinaryCode[i] == '0')
                {
                    innerResult += $"({'!'.ToString() + ((char)(65 + i)).ToString()})" ;
                }

                result += innerResult ;
            }

            return result + "+";
        }

        public bool Equals(Implicant other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(BinaryCode, other.BinaryCode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Implicant) obj);
        }

        public override int GetHashCode()
        {
            return (BinaryCode != null ? BinaryCode.GetHashCode() : 0);
        }
    }
}