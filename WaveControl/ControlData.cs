using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveControl
{
    public class ControlDataEntity
    {
        public int x;
        public int y;
        public int childHeart1;
        public int childHeart2;
        public int palaceCompression;
        public int fetalMovement;
        public string sampleTime;
        public bool isDrawVLine;
        public bool isDrawText;
        public static int[] tValue = { 210, 180, 150, 120, 90, 60 };
        public static int[] bValue = { 100, 80, 60, 40, 20, 0 };
        public static ControlDataEntity ConvertEntity(string[] data)
        {
            ControlDataEntity entity = new ControlDataEntity();
            entity.childHeart1 = Convert.ToInt32(data[0]);
            entity.childHeart2 = Convert.ToInt32(data[2]);

            entity.palaceCompression = Convert.ToInt32(data[3]);
            entity.fetalMovement = Convert.ToInt32(data[4]);

            entity.sampleTime = data[1];
            entity.isDrawText = false;
            entity.isDrawVLine = false;
            return entity;
        }
    }

    public class ControlDataCenter
    {
        public List<ControlDataEntity> datalist = new List<ControlDataEntity>();
        public void AddData(ControlDataEntity entity)
        {
            datalist.Add(entity);
        }
        public void DelData(int index)
        {
            datalist.RemoveAt(index);
        }
        public ControlDataEntity GetData(int index)
        {
            return datalist[index];
        }
        public List<ControlDataEntity> GetDataSequnce(int begin, int end)
        {
            List<ControlDataEntity> data = new List<ControlDataEntity>();
            for (int i = begin, j = 0; i < end; i++, j++)
            {
                data.Add(datalist[i]);
            }
            return data;
        }
        public int GetSize()
        {
            return datalist.Count;
        }
        public List<int> GetChildHeart1Data(int begin, int end)
        {
            List<int> d = new List<int>();
            List<ControlDataEntity> data = GetDataSequnce(begin, end);
            for (int i = 0; i < end - begin; i++)
            {
                d.Add(data[i].childHeart1);
            }
            return d;
        }
    }
}
