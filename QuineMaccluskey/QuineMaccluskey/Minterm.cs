using System;

namespace QuineMaccluskey
{
    public class Minterm : IEquatable<Minterm>
    {
        public string Number { get; set; } 
        public string BinaryCode { get; set; }
        public int NumberofOnes { get; set; }
        public Minterm(string number)
        {
            Number = number;
            this.BinaryCode = Minterm.NumberToBinaryCode(number);
            this.NumberofOnes = Minterm.NumberOfOne(Minterm.NumberToBinaryCode(number));
        }

        private static int NumberOfOne(string binaryCode)
        {
            int count = 0;
            for (int i = 0; i < binaryCode.Length; i++)
            {
                if (binaryCode[i] == '1')
                {
                    count++;
                }
            }

            return count;
        }

        private static string NumberToBinaryCode(string number)
        {
            int numberBinary = int.Parse(number);
            int a = 0;
            for (int i = 0; numberBinary > 0 ; i++)
            {
                a += (numberBinary % 2) *(int) Math.Pow(10, i);
                numberBinary /= 2;
            }

            return a.ToString();
        }


        public bool Equals(Minterm other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Number, other.Number);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Minterm) obj);
        }

        public override int GetHashCode()
        {
            return (Number != null ? Number.GetHashCode() : 0);
        }
    }
}