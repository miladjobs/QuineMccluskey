using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QuineMaccluskey
{
    class Program
    {
        
        static void Main(string[] args)
        {
            #region InputDataAndSaveData

            Console.WriteLine("Enter Number Of Variable");
            int variableCount = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Numbers Of minterm");
            int mintermNumber = int.Parse(Console.ReadLine());
            List<Minterm> minterms = new List<Minterm>();
            Console.WriteLine("Enter Each Minterm");
            for (int i = 0; i < mintermNumber; i++)
            {
                Console.WriteLine($"Enter Minterm {i + 1}");
                string number = Console.ReadLine();
                Minterm minterm = new Minterm(number);
                minterms.Add(minterm);
            }
            List<int> numberOfOne = new List<int>();
            for (int i = 0; i < minterms.Count; i++)
            {
                numberOfOne.Add(minterms[i].NumberofOnes);
            }
            int maxNumberOfOne = numberOfOne.Max();
            for (int i = 0; i < minterms.Count; i++)
            {
                while (minterms[i].BinaryCode.Length < variableCount)
                {
                    minterms[i].BinaryCode = '0' + minterms[i].BinaryCode;
                }
            }

            #endregion


            #region While

            Minterms groupMinterms = new Minterms(maxNumberOfOne);
            groupMinterms.GroupLists(minterms);
            Minterms gp2 = GroupMintermCreate(groupMinterms);
            gp2.SortGroupList();
            Minterms gp3 = GroupMintermCreate(gp2);
            gp3.SortGroupList();
            //Create Function To Clean ListB
            
            


            #endregion

            Console.ReadLine();
        }

        public static Minterms GroupMintermCreate(Minterms groupMinterms)
        {
            List<List<Minterm>> mintermList1 = new List<List<Minterm>>();
            for (int i = 0; i < groupMinterms.GroupOfMinterms.Length - 1; i++)
            {
                for (int j = 0; j < groupMinterms.GroupOfMinterms[i].Count; j++)
                {
                    for (int k = 0; k < groupMinterms.GroupOfMinterms[i + 1].Count; k++)
                    {
                        if (CheckDifferent(groupMinterms.GroupOfMinterms[i][j].BinaryCode, groupMinterms.GroupOfMinterms[i + 1][k].BinaryCode))
                        {
                            mintermList1.Add(NewGroupMake(groupMinterms, i, j, k));
                        }
                    }
                }
            }
            Minterms mintermsGroup2 = new Minterms(mintermList1.Count);
            
            for (int i = 0; i < mintermList1.Count; i++)
            {
                mintermsGroup2.GroupOfMinterms[i] = mintermList1[i];
            }

            return mintermsGroup2;
        }

        public static List<Minterm> NewGroupMake(Minterms groupMinterms, int i, int j, int k)
        {
            string result = GroupMake(groupMinterms.GroupOfMinterms[i][j].BinaryCode,
                                            groupMinterms.GroupOfMinterms[i + 1][k].BinaryCode);
            Minterm minterm1 = new Minterm(groupMinterms.GroupOfMinterms[i][j].Number);
            Minterm minterm2 = new Minterm(groupMinterms.GroupOfMinterms[i + 1][k].Number);
            minterm1.BinaryCode = result;
            minterm2.BinaryCode = result;
            List<Minterm> MintermsList = new List<Minterm>() { minterm1, minterm2 };
            return MintermsList;
        }

        public static bool CheckDifferent(string a, string b)
        {
            if (a.Length == b.Length)
            {
                int count = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] != b[i])
                        count++;
                }
                if (count > 1)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public static string GroupMake( string a,  string b)
        {
            string result = string.Empty;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    result += '-';
                }
                else
                {
                    result += a[i];
                }
            }

            return result;
        }
    }
}
