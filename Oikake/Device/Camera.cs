using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Oikake.Device
{
   static class Camera
    {
        //カメラの位置(左上座標)
        private static Vector2 position =Vector2.Zero;
        //最小値
        private static Vector2 minpos;
        //最大値
        private static Vector2 maxPos;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="initPos"></param>
        public static void Initializa(Vector2 initPos)
        {
            position = initPos;
            //最小値から最大値にの間に調整
            position = Vector2.Clamp(position, minpos, maxPos);
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="velocity">移動量</param>
        public static void Move(Vector2 velocity)
        {
            position += velocity;
            //最小値から最大値の間に調整
            position = Vector2.Clamp(position, minpos, maxPos);
        }

        /// <summary>
        ///ゲームウィンド上の位置(表示位置)取得) 
        /// </summary>
        /// <param name="mapPos">マップ上での位置表示</param>
        /// <returns></returns>
        public static Vector2 GetScreenPos(Vector2 mapPos)
        {
            return mapPos - position;
        }
        /// <summary>
        /// 位置取得
        /// </summary>
        /// <returns>位置</returns>
        public static Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// 最小座標設定
        /// </summary>
        /// <param name="min">最小座標</param>
        public static void SetMin(Vector2 min)
        {
            minpos = min;
        }

        public static void SetMax(Vector2 max)
        {
            maxPos = max;
        }
   }
}
