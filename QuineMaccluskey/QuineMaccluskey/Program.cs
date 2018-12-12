using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
            List<Minterm> minterms = new List<Minterm>(); //Dar marhale Akhar be in esm Zakhire Shode
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
            bool x = true;
            while (true)
            {
                Minterms gp;
                if (x)
                {
                    gp = GroupMintermCreate(gpMinterms,x);
                    x = false;
                }
                else
                {
                     gp = GroupMintermCreate(gpMinterms, x);
                }
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

            #region PrimeImplicantCreate

            for (int i=firstPrimeImplicants.Count-1;i>=0;i--) //For Create Prime Implicant
            {
                for (int j = 0; j < firstPrimeImplicants[i].Count; j++)
                {
                    if (firstPrimeImplicants[i][j].Status)
                    {
                        for (int k = 0; k < i; k++)
                        {
                            for (int l = 0; l < firstPrimeImplicants[k].Count; l++)
                            {
                                if (CheckEqual(firstPrimeImplicants[i][j],firstPrimeImplicants[k][l]))
                                {
                                    firstPrimeImplicants[k][l].Status = false;
                                }
                            } 
                        }
                    }
                }
            }

            List<Implicant> primeImplicants = new List<Implicant>();
            for (int i = 0; i < firstPrimeImplicants.Count; i++)
            {
                for (int j = 0; j < firstPrimeImplicants[i].Count; j++)
                {
                    if (firstPrimeImplicants[i][j].Status )
                    {
                        primeImplicants.Add(firstPrimeImplicants[i][j]);
                    }
                }
            }

            #endregion

            #region FindeFinallImplicants

            List<Implicant> finalImplicants = new List<Implicant>();
            bool[] mintermsUse = new bool[minterms.Count];
            for (int i = 0; i < mintermsUse.Length; i++)
            {
                mintermsUse[i] = false;
            }

            bool[,] implicantsTable = new  bool[primeImplicants.Count,minterms.Count];
            for (int i = 0; i < minterms.Count; i++)
            {
                for (int j = 0; j < primeImplicants.Count; j++)
                {
                    if (primeImplicants[j].Minterms.Contains(minterms[i]))
                    {
                        implicantsTable[j, i] = true;
                    }
                    else
                    {
                        implicantsTable[j, i] = false;
                    }
                }
            }

            while (!CheckMinterUse(mintermsUse))
            {
                #region FindeEssential

                List<int> essentialList = new List<int>();
                for (int i = 0; i < implicantsTable.GetLength(1); i++)
                {
                    if (CheckEssentialImplicants(implicantsTable,i))
                    {
                        essentialList.Add(SendIndexOfEssential(implicantsTable,i));
                    }
                }

                for (int i = 0; i < essentialList.Count; i++)
                {
                    for (int j = 0; j < minterms.Count; j++)
                    {
                        if (primeImplicants[essentialList[i]].Minterms.Contains(minterms[j]))
                        {
                            mintermsUse[j] = true;
                        }
                    }
                    finalImplicants.Add(primeImplicants[essentialList[i]]);
                }

                if (essentialList.Count>0)
                {
                    implicantsTable = UpdateImplicantsTable(implicantsTable, essentialList);
                    continue;
                }

                #endregion

                #region ColumnLaw

                for (int i = 0; i < implicantsTable.GetLength(1); i++)
                {
                    for (int j = 0; j < implicantsTable.GetLength(1); j++)
                    {
                        if (NumberOfDifferences(implicantsTable,i,j,false) == 1)
                        {
                            if (NumberOfTrue(implicantsTable,i,false) > NumberOfTrue(implicantsTable, j, false))
                            {
                                implicantsTable = UpdateImplicantsTable(implicantsTable, i, false);
                            }
                            else
                            {
                                implicantsTable = UpdateImplicantsTable(implicantsTable, j, false);
                            }
                        }
                    }
                }

                #endregion

                #region RowLaw

                for (int i = 0; i < implicantsTable.GetLength(0); i++)
                {
                    for (int j = 0; j < implicantsTable.GetLength(0); j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        if (NumberOfDifferences(implicantsTable,i,j,true) ==1)
                        {
                            if (NumberOfTrue(implicantsTable,i,true) > NumberOfTrue(implicantsTable, j, true))
                            {
                                implicantsTable = UpdateImplicantsTable(implicantsTable, j, true);
                            }
                            else
                            {
                                implicantsTable = UpdateImplicantsTable(implicantsTable, i, true);
                            }
                        }
                        else if (NumberOfDifferences(implicantsTable,i,j,true) == 0)
                        {
                            implicantsTable = UpdateImplicantsTable(implicantsTable, j, true);
                        }
                        
                    }
                }

                #endregion

            }

            List<Implicant> finalImplicantSorted = new List<Implicant>();
            for (int i = 0; i < finalImplicants.Count; i++)
            {
                if (!finalImplicantSorted.Contains(finalImplicants[i]))
                {
                    finalImplicantSorted.Add(finalImplicants[i]);
                }
            }

            #endregion

            string result = "";
            for (int i = 0; i < finalImplicantSorted.Count; i++)
            {
                result += finalImplicantSorted[i].ToString() ;
                if (i % 10 == 0 && i!=0)
                {
                    result += "\n";
                }
            }
            Console.Clear();
            Console.WriteLine("<<<<<<<<<Result Of Simplify>>>>>>>> \n\n");
            Console.WriteLine(result.Remove(result.Length-1));

            Console.ReadLine();
        }

        public static Minterms GroupMintermCreate(Minterms groupMinterms , bool x) //Moshkel
        {
            List<List<Minterm>> mintermList1 = new List<List<Minterm>>();
            if (x == false)
            {
                for (int i = 0; i < groupMinterms.GroupOfMinterms.Length; i++)
                {
                    for (int j = 0; j < groupMinterms.GroupOfMinterms.Length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        if( CheckDifferent(groupMinterms.GroupOfMinterms[i][0].BinaryCode,groupMinterms.GroupOfMinterms[j][0].BinaryCode))
                        {
                            mintermList1.Add(NewGroupMake(groupMinterms, i, j));
                        }
                    }
                }
            }

            else 
            {
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

        public static List<Minterm> NewGroupMake(Minterms groupMinterms, int i, int j)
        {
            string result = GroupMake(groupMinterms.GroupOfMinterms[i][0].BinaryCode,
                groupMinterms.GroupOfMinterms[j][0].BinaryCode);
            List<Minterm> MintermsList = new List<Minterm>();
            for (int k = 0; k < groupMinterms.GroupOfMinterms[i].Count; k++)
            {
                Minterm minterm = new Minterm(groupMinterms.GroupOfMinterms[i][k].Number);
                minterm.BinaryCode = result;
                MintermsList.Add(minterm);
            }
            for (int k = 0; k < groupMinterms.GroupOfMinterms[j].Count; k++)
            {
                Minterm minterm = new Minterm(groupMinterms.GroupOfMinterms[j][k].Number);
                minterm.BinaryCode = result;
                MintermsList.Add(minterm);
            }

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

        public static bool CheckEqual(Implicant gic1, Implicant sic2) 
        {
            bool restult = true;
            for (int i = 0; i < sic2.Minterms.Count; i++)
            {
                if (! gic1.Minterms.Contains(sic2.Minterms[i]))
                {
                    restult = false;
                }
            }
            return restult;
        }

        public static bool CheckEssentialImplicants(bool[,] table, int indexJ)
        {
            int sum = 0;
            for (int i = 0; i < table.GetLength(0); i++)
            {
                if (table[i,indexJ])
                {
                    sum++;
                }
            }

            if (sum == 1)
            {
                return true;
            }

            return false;
        }

        public static int SendIndexOfEssential(bool[,] table, int indexJ)
        {
            int result = -1;
            for (int i = 0; i < table.GetLength(0); i++)
            {
                if (table[i,indexJ])
                {
                    result = i;
                }
            }

            return result;
        }

        public static bool CheckMinterUse(bool[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool[,] UpdateImplicantsTable(bool[,] firstTable, List<int> rowIndexes)
        {
            List<int> columnIndexes = new List<int>();
            for (int i = 0; i < rowIndexes.Count; i++)
            {
                for (int j = 0; j < firstTable.GetLength(1); j++)
                {
                    if (firstTable[rowIndexes[i],j])
                    {
                        columnIndexes.Add(j);
                        firstTable[rowIndexes[i], j] = false;
                    }
                }
            }

            for (int i = 0; i < columnIndexes.Count; i++)
            {
                for (int j = 0; j < firstTable.GetLength(0); j++)
                {
                    if (firstTable[j,columnIndexes[i]])
                    {
                        firstTable[j, columnIndexes[i]] = false;
                    }
                }
            }

            return firstTable;
        }

        public static bool[,] UpdateImplicantsTable(bool[,] firstTable, int index, bool row)
        {
            if (row)
            {
                for (int i = 0; i < firstTable.GetLength(1); i++)
                {
                    firstTable[index, i] = false;
                }

                return firstTable;
            }
            else
            {
                for (int i = 0; i < firstTable.GetLength(0); i++)
                {
                    firstTable[i, index] = false;
                }

                return firstTable;
            }
        }

        public static int NumberOfDifferences(bool[,] table, int index1, int index2 ,bool row)
        {
            int sum = 0;
            if (row)
            {
                for (int i = 0; i < table.GetLength(1); i++)
                {
                    if (table[index1,i] != table[index2,i])
                    {
                        sum++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    if (table[i,index1] != table[i,index2])
                    {
                        sum++;
                    }
                }
            }

            return sum;
        }

        public static int NumberOfTrue(bool[,] table, int index, bool row)
        {
            int sum = 0;
            if (row)
            {
                for (int i = 0; i < table.GetLength(1); i++)
                {
                    if (table[index,i])
                    {
                        sum++;
                    }
                } 
            }
            else
            {
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    if (table[i,index])
                    {
                        sum++;
                    }
                }
            }

            return sum;
        }
    }
}
