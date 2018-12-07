using System.Collections.Generic;

namespace QuineMaccluskey
{
    public class Minterms
    {
        public List<Minterm>[] GroupOfMinterms;
        private int maxNumber;

        public Minterms(int groupsNumber)
        {
            this.GroupOfMinterms = new List<Minterm>[groupsNumber];
            maxNumber = groupsNumber;
        }

        public Minterms()
        {
            
        }

        public void GroupLists(List<Minterm> minterms)
        {
            int maxNumberOfOne = this.maxNumber;
            List<Minterm>[] mintermGroups = new List<Minterm>[maxNumberOfOne];
            for (int i = 0; i < maxNumberOfOne; i++)
            {
                mintermGroups[i] = new List<Minterm>();
                for (int j = 0; j < minterms.Count; j++)
                {
                    if (minterms[j].NumberofOnes == i + 1)
                    {
                        mintermGroups[i].Add(minterms[j]);
                    }
                }
            }

            this.GroupOfMinterms = mintermGroups;
        }
    }
}