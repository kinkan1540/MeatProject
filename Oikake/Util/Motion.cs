using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Oikake.Util
{
    class Motion
    { 
        private Range range;//範囲
        private Timer timer;//切り替え時間
        private int motionNumber;//現在のモーション番号

        //表示位置を番号で確認
        //Dictionaryを使えば登録順番を気にしなくてもいい
        private Dictionary<int, Rectangle> rectangles = new Dictionary<int, Rectangle>();

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public Motion()
        {
            //何もしない
           Initialize(new Range(0, 0), new CountDownTimer());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="range">範囲</param>
        ///<param name="timer">モーション切り替え</param>>
        public Motion(Range range,Timer timer)
        {
            Initialize(range, timer);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="range">範囲</param>
        /// <param name="timer">モーション切り替え時間</param>
        public void Initialize(Range range, Timer timer)
        {
            this.range = range;
            this.timer = timer;

            //モーション番号は、範囲の最初に設定
            motionNumber = range.First();
        }

        /// <summary>
        /// モーション短形情報の追加
        /// </summary>
        /// <param name="index">管理番号</param>
        /// <param name="rect">短形</param>
        public void Add(int index,Rectangle rect)
        {
            //すでに登録されていたらないもしない
           if(rectangles.ContainsKey(index))
            {
                return;
            }
            //登録
            rectangles.Add(index, rect);
        }

        /// <summary>
        /// モーションの更新
        /// </summary>
        private void MotionUpdate()
        {
            //モーション番号をインクリメント
            motionNumber += 1;

            //範囲外なら最初に戻す
            if (range.IsOutOFRange(motionNumber))
            {
                motionNumber = range.First();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        /// <param name="gameTime">ゲーム時間</param>>
        public void Update(GameTime gameTime)
        {
            //ガード説(範囲外なら何もしない)
            if (range.IsOutOFRange()) 
            {
                return;
            }
            //時間を更新
            timer.Update(gameTime);
            //指定時間になってたらモーションを更新
            if(timer.IsTime())
            {
                timer.Initialize();
                MotionUpdate();
            }
        }

        /// <summary>
        /// 描画範囲の取得
        /// </summary>
        /// <returns></returns>
        public Rectangle DrawingRange()
        {
            return rectangles[motionNumber];
        }
    }
}
