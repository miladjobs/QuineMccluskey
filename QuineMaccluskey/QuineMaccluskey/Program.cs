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

            #region FindeGroups
            Minterms groupMinterms = new Minterms(maxNumberOfOne);
            groupMinterms.GroupLists(minterms);

            List<List<Implicant>> firstPrimeImplicants = new List<List<Implicant>>();
            List<Implicant> implicants = new List<Implicant>();
            for (int i = 0; i < groupMinterms.GroupOfMinterms.Length; i++)
            {
                for (int j = 0; j < groupMinterms.GroupOfMinterms[i].Count; j++)
                {
                    Implicant implicant = new Implicant(new List<Minterm>() { groupMinterms.GroupOfMinterms[i][j] });
                    implicants.Add(implicant);
                }
            }
            firstPrimeImplicants.Add(implicants);
            Minterms gpMinterms = groupMinterms;
            while (true)
            {
                Minterms gp = GroupMintermCreate(gpMinterms);
                gp.SortGroupList();
                if (gp.GroupOfMinterms.Length == 0)
                {
                    break;
                }
                List<Implicant> implicantsInWhileList = new List<Implicant>();
                for (int i = 0; i < gp.GroupOfMinterms.Length; i++)
                {
                    List<Minterm> implicantMinterms = new List<Minterm>();
                    for (int j = 0; j < gp.GroupOfMinterms[i].Count; j++)
                    {
                        implicantMinterms.Add(gp.GroupOfMinterms[i][j]);
                    }
                    Implicant implicant = new Implicant(implicantMinterms);
                    implicantsInWhileList.Add(implicant);
                }
                firstPrimeImplicants.Add(implicantsInWhileList);
                gpMinterms = gp;
            }
            #endregion

            for (int i=firstPrimeImplicants.Count-1;i>=0;i--)
            {
                for (int j = 0; j < firstPrimeImplicants[i].Count; j++)
                {
                    if (firstPrimeImplicants[i][j].Status)
                    {
                        for (int k = 0; k < i; k++)
                        {
                            for (int l = 0; l < firstPrimeImplicants[k].Count; l++)
                            {
                                //Create Function Create
                            } 
                        }
                    }
                }
            }

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

        public static bool CheckEqual(Implicant gic1, Implicant sic2) //Complete It
        {
            bool restult = false;
            for (int i = 0; i < gic1.Minterms.Count; i++)
            {
                for (int j = 0; j < sic2.Minterms.Count; j++)
                {
                    if (true)
                    {
                        
                    } 
                }
            }

            return restult;
        }
    }
}
