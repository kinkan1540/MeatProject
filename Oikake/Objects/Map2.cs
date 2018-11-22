using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Def;
using Oikake.Device;
using Oikake.IO;


namespace Oikake.Objects
{
    class Map2:Map1
    {
        //「どこ」に「何番目のチップ」を置くか
        private int[,] mapDate;
        private float[] chipX;
        private float[] chipY;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Map2()
        {
       
            SetChipXY();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            mapDate = CsvLoad.Load("Content/csv/Bolck2.csv");
            Device.Camera.SetMin(Vector2.Zero);
            Device.Camera.SetMax(new Vector2(
                mapDate.GetLength(0) * Size.ChipX - Screen.Width,
                mapDate.GetLength(1) * Size.ChipY - Screen.Height));
        }
        public override void Update()
        {

        }
        public override void Draw(Renderer renderer)
        {
            //開始
            Vector2 start = Device.Camera.GetPosition();
            //配列番号に直す
            start = new Vector2(start.X / Size.ChipX, start.Y / Size.ChipY);
            //mapDateから順番に数字を取り出して、
            //対応する画像を表示する
            for (int j = (int)start.Y; j <= (int)start.Y + 25; j++)
            {
                if (j < 0 || mapDate.GetLength(0) <= j)
                    continue;
                for (int i = (int)start.X; i < start.X + 40; i++)
                {
                    if (i < 0 || mapDate.GetLength(1) <= 1)
                        continue;
                    //「(i,j)の位置」の「数値データ」
                    //(ここに上のデータの数字が入ってくる)
                    float chipNum = mapDate[j, i];

                    //表示位置
                    Vector2 position = new Vector2(
                        i * Size.ChipX,
                        j * Size.ChipY);

                    //表示ウィンド上の位置に調整
                    //(マップ上の位置→表示ウィンド上の位置)
                    position = Device.Camera.GetScreenPos(position);

                    //切り取り範囲
                    Rectangle rect =
                        new Rectangle((int)chipX[(int)chipNum], (int)chipY[(int)chipNum], (int)Size.ChipX, (int)Size.ChipY);

                    //「(i,j)」の位置に
                    //「chipNum」番にチップを描く
                    renderer.DrawTexture("background", position, rect);
                }
            }
        }

        public override bool IsBloock(Vector2 position)
        {
            int chipNum = ChipNum(position);

            //番号がブロックだったらtrueを返す
            if (chipNum == 0)
            {
                return true;
            }
            //そうでなければfalseを返す
            return false;
        }

        public override bool IsGoal(Vector2 position)
        {
            //その位置のマップチップ番号を見る
            int chipNum = ChipNum(position);


            //番号がブロックだったらtrueを返す
            if (chipNum == 2)
            {
                return true;
            }
            //そうでなければfalseを返す
            return false;
        }

        public override bool IsTrap(Vector2 position)
        {
            int chipNum = ChipNum(position);
            if (chipNum == 3)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///マップチップ切り取り位置計算
        /// </summary>
        private void SetChipXY()
        {
            //マップチップの個数分
            chipX = new float[64];
            chipY = new float[64];
            {
                //あらかじめ切り取り位置を計算しておく
                for (int i = 0; i < 32; i++)
                {
                    chipX[i] = (i % 8) * Size.ChipX;
                    chipY[i] = (i / 8) * Size.ChipY;
                }
            }
        }
        public override int ChipNum(Vector2 position)
        {
            //一応positionを画面内に調整しておく
            position = Vector2.Clamp(position, Vector2.Zero,
                new Vector2(Screen.Width, Screen.Height));

            //マップデータの位置番号に直す
            position.X = position.X / 32;
            position.Y = position.Y / 32;

            if ((int)position.Y >= mapDate.GetLength(0))
            {
                position.Y = mapDate.GetLength(0) - 1;
            }
            if ((int)position.X >= mapDate.GetLength(1))
            {
                position.X = mapDate.GetLength(1) - 1;
            }
            return mapDate[(int)position.Y, (int)position.X];
        }
    }
}
