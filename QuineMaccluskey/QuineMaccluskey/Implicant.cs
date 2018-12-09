using System.Collections.Generic;

namespace QuineMaccluskey
{
    public class Implicant
    {
        public List<Minterm> Minterms { get; set; }
        public bool Status { get; set; }

        public Implicant(List<Minterm> minterms)
        {
            this.Minterms = minterms;
            this.Status = true;
        }
    }
}