﻿using System;
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
            List<Minterm>[] mintermGroups = new List<Minterm>[maxNumberOfOne +1];
            mintermGroups[0] = new List<Minterm>();
            for (int i = 0; i < minterms.Count; i++)
            {
                if (minterms[i].NumberofOnes == 0)
                {
                    mintermGroups[0].Add(minterms[i]);
                }
            }
            for (int i = 1; i <= maxNumberOfOne; i++)
            {
                mintermGroups[i] = new List<Minterm>();
                for (int j = 0; j < minterms.Count; j++)
                {
                    if (minterms[j].NumberofOnes == i )
                    {
                        mintermGroups[i].Add(minterms[j]);
                    }
                }
            }

            this.GroupOfMinterms = mintermGroups;
        }

        public void SortGroupList()
        {
            List<string> binaryCodesUnOrdinate = new List<string>();
            for (int i = 0; i < this.GroupOfMinterms.Length ; i++)
            {
               binaryCodesUnOrdinate.Add(this.GroupOfMinterms[i][0].BinaryCode);
            }

            List<string> binaryCodes = Minterms.SortList(binaryCodesUnOrdinate);
            List<Minterm>[] newMinterms = new List<Minterm>[binaryCodes.Count];
            for (int i = 0; i < binaryCodes.Count; i++)
            {
                newMinterms[i] = new List<Minterm>();
            }
            for (int i = 0; i < binaryCodes.Count; i++)
            {
                for (int j = 0; j < this.GroupOfMinterms.Length; j++)
                {
                    for (int k = 0; k < this.GroupOfMinterms[j].Count; k++)
                    {
                        if (binaryCodes[i] == this.GroupOfMinterms[j][k].BinaryCode )
                        {
                            if (!( newMinterms[i].Contains(this.GroupOfMinterms[j][k])))
                            {
                                newMinterms[i].Add(this.GroupOfMinterms[j][k]);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < newMinterms.Length; i++)
            {
                newMinterms[i] = Minterms.SortList(newMinterms[i]);
            }
            this.GroupOfMinterms = new List<Minterm>[newMinterms.Length];
            this.GroupOfMinterms = newMinterms;


        }

        public static List<string> SortList(List<string> list)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i+1; j < list.Count; j++)
                {
                    if (list[i] == list[j])
                    {
                        list[j] = " ";
                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != " ")
                {
                    result.Add(list[i]);
                }
            }

            return result;
        }

        public static List<Minterm> SortList(List<Minterm> list)
        {
            List<Minterm> result = new List<Minterm>();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i+1; j < list.Count; j++)
                {
                    if (list[i].Number == list[j].Number)
                    {
                        list[j].Number = " ";
                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Number != " ")
                {
                    result.Add(list[i]);
                }
            }

            return result;
        }
    }
}