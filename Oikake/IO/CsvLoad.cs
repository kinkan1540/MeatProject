using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;

namespace Oikake.IO
{
     class CsvLoad
    {
        private List<string[]> stringDate;
        public void Clear()
        {
            stringDate.Clear();
        }

        public CsvLoad()
        {
            stringDate = new List<string[]>();
        }

        public string[][]GetArrayDate()
        {
            return stringDate.ToArray();
        }

        public List<string[]>GetDate()
        {
            return stringDate;
        }

        public int[][] GetIntDate()
        {
            //ジャグ配列を習得し、空のint型の2次元配列を習得
            var date = GetArrayDate();
            int row = date.Count();
            
            int[][] intDate = new int[row][];
            for (int i = 0; i < row; i++)
            {
                int col = date[i].Count();
                intDate[i] = new int[col];
            }
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < intDate[y].Count(); x++)
                {
                    intDate[y][x] = int.Parse(date[y][x]);
                }
            }
            return intDate;
        }

        public string[,] GetStringMatrix()
        {
            var date = GetArrayDate();
            int row = date.Count();
            int col = date[0].Count();

            string[,] result = new string[row, col];
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                {
                    result[y, x] = date[y][x];
                }
            }
            return result;
        }

        public int[,] GetIntMatrix()
        {
            var date = GetIntDate();
            int row = date.Count();
            int col = date[0].Count();

            int[,] result = new int[row, col];
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                {
                    result[y, x] = date[y][x];
                }
            }
            return result;
        }

        public void Read(string filename,string path="./")
        {
            Clear();
            try
            {
                using (var sr = new System.IO.StreamReader(@"Content/" + path + filename))
                {
                    while(!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var values = line.Split(',');

                        stringDate.Add(values);

#if DEBUG
                        foreach(var v in values)
                        {
                            System.Console.Write("{0}", v);
                        }

                        System.Console.WriteLine();
#endif
                    }
                }
            }
            catch(System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
