using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace HanLib
{
    public class EnergyComponent : MonoBehaviour, IData
    {
        public int maxEnergy;
        public int currEnergy;

        public int Adj(int amount)
        {
            if(amount > 0)
            {
                int addAmount = Mathf.Min(maxEnergy - currEnergy, amount);
                currEnergy += addAmount;
                return addAmount;
            }
            int subAmount = Mathf.Min(currEnergy, -amount);
            currEnergy -= subAmount;
            return subAmount;
        }

        public void Load(BinaryReader reader)
        {
            maxEnergy = reader.ReadInt32();
            currEnergy = reader.ReadInt32();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(maxEnergy);
            writer.Write(currEnergy);
        }

        public void AfterLoad(DataStorage model)
        {

        }
    }
}