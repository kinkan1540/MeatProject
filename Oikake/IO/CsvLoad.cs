using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;

namespace Oikake.IO
{
    static class CsvLoad
    {

        /// <summary>
        /// 全ての行のロード、縦横のサイズをカウント
        /// </summary>
        /// <param name="streamReader">ストリームリーダー</param>
        /// <param name="lines">行のデーター登録用リスト</param>
        /// <param name="splitLine">1行分の分割データ保存用</param>
        /// <returns></returns>
        private static Vector2 CsvLoadAndCountent(StreamReader streamReader,List<string>lines,string[]splitLine)
        {
            //一行ずつ最後まで読みこむ
            while (streamReader.Peek()>=0)
            {
                //ファイルから1行読みこんでlinesにAdd
                lines.Add(streamReader.ReadLine());
            }
            //何行あるか数える(縦の個数)
            int lineCount = lines.Count;

            //1行目を「,」区切りで分解する
            splitLine = lines[0].Split(',');
            //「,」区切りで何個あったか数える(横の個数)
            int colCount = splitLine.Length;

            //数えた大きさを返す
            return new Vector2(colCount, lineCount);
        }

        /// <summary>
        /// マップデータロード
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>データの入った2重配列</returns>
        public static int[,]Load(string filePath)
        {
            //戻り値
            int[,] mapDate;　
            //Readerの生成
            StreamReader streamReader = new StreamReader(filePath);
            //読みこんだデータを1行ずつ登録するリスト
            List<string> lines = new List<string>();
            //1行を分解して保存するデータ
            string[] spitLine = null;
            //データの個数を数える
            Vector2 size = CsvLoadAndCountent(streamReader, lines, spitLine);
            //データの個数で配列を生成
            mapDate = new int[(int)size.Y, (int)size.X];
            //1行ずつ取り出す
            for(int i=0;i<size.Y;i++)
            {
                //「,」で分ける
                spitLine = lines[i].Split(',');
                //分けた分を1行ずつ配列に入れていく
                for(int j=0;j<size.X;j++)
                {
                    mapDate[i, j] = int.Parse(spitLine[j]);
                }
            }
            //ストリームを閉じる
            streamReader.Close();

            //戻り値を返す
            return mapDate;
        }

    }
}
