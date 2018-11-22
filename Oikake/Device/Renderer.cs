using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;//Assert用

namespace Oikake.Device
{
    class Renderer
    {
        private ContentManager contentManager; //コンテンツ管理者
        private GraphicsDevice graphicsDevice; //グラフィック機器
        private SpriteBatch spriteBatch; //スプライト一括描画用オブジェクト
        private Def.Camera camera;

        //複数画像管理用変数の宣言と生成
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">Game1クラスのコンテンツ管理者</param>
        /// <param name="graphics">Game1クラスのグラフィック機器</param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);
            camera = new Def.Camera(graphics.Viewport);
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="assetName">アセット名（ファイルの名前）</param>
        /// <param name="filepath">画像へのファイルパス</param>
        public void LoadContent(string assetName, string filepath = "./")
        {
            //すでにキー（assetName：アセット名）が登録されているとき
            if (textures.ContainsKey(assetName))
            {
#if DEBUG //DEBUGモードの時のみ下記エラー分をコンソールへ表示
                Console.WriteLine(assetName + "はすでに読み込まれています。\n プログラムを確認してください。");
#endif

                //それ以上読み込まないのでここで終了
                return;
            }
            //画像の読み込みとDictionaryへアセット名と画像を登録
            textures.Add(assetName, contentManager.Load<Texture2D>(filepath + assetName));

        }

        public void LoadContent(string assetName, Texture2D texture)
        {
            //すでにキー(assetName:アセット名)が登録されているとき
            if (textures.ContainsKey(assetName))
            {
#if DEBUG//DEBUGモードの時のみ下記のエラー文をコンソールへ表示
                Console.WriteLine(assetName + "はすでに読みこまれています。＼n" + "プログラムを確認してください");
#endif
                //それ以上は読みこまないので終了
                return;
            }
            textures.Add(assetName, texture);

        }

        /// <summary>
        /// アセット名
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">切り出し範囲</param>
        /// <param name="rotate">回転角度</param>
        /// <param name="rotatePosition">回転軸位置</param>
        /// <param name="scal">拡大縮小</param>
        /// <param name="effects">表示反転位置</param>
        /// <param name="depth">スプライト深度</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string assetName, Vector2 position, Rectangle? rect, float rotate, Vector2 rotatePosition, Vector2 scal, SpriteEffects effects = SpriteEffects.None, float depth = 0.0f, float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[assetName],//テクスチャ
                position,//位置
                rect,//切り取り範囲
                Color.White * alpha,//透明値
                rotate,//回転角度
                rotatePosition,//回転軸
                scal,//縮小拡大
                effects,//表示反転効果
                depth//スプライト深度
                );
        }

        /// <summary>
        /// アンロード
        /// </summary>
        public void Unload()
        {
            textures.Clear();//Dictionaryの情報をクリア
        }

        /// <summary>
        /// 描画開始
        /// </summary>
        public void Begin()
        {
            spriteBatch.Begin();
        }

        public void Begin(Vector2 center)
        {
            camera.Update(center);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null, null, null, null,
                camera.Transform);
        }

        /// <summary>
        /// 描画終了
        /// </summary>
        public void End()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// 画像の描画（画像サイズはそのまま）
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="alpha">透明値（1.0f：不透明 0.0f：透明）</param>
        public void DrawTexture( string assetName, Vector2 position, float alpha = 1.0f)
        {
            //デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません");

            spriteBatch.Draw(textures[assetName], position, Color.White * alpha);
        }

        /// <summary>
        /// 画像の描画（画像を指定範囲内だけ描画）
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">指定範囲</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture( string assetName, Vector2 position, Rectangle rect, float alpha = 1.0f)
        {
            //デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません");

            spriteBatch.Draw(
                textures[assetName], //テクスチャ
                position,            //位置
                rect,                //指定範囲（矩形で指定：左上の座標、幅、高さ）
                Color.White * alpha);//透明値
        }
        public void DrawNumber(
            string assetName,
            Vector2 postion,
            int number,
            float alpha=1.0f)
        {
            Debug.Assert(textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか" +
                "画像の読み込み自体出来ていません");
            if(number<0)
            {
                number = 0;
            }

            int width = 32;
            foreach(var n in number.ToString())
            {
                spriteBatch.Draw(
                    textures[assetName],
                    postion,
                    new Rectangle((n - '0') * width, 0, width, 64),
                    Color.White);

                postion.X += width;
            }
            
        }
     public void DrawNumber(
         string assetName,
         Vector2 postion,
         float number,
         float alpha=1.0f)
        {
            if(number<0.0f)
            { number = 0.0f; }
            int width = 32;
            foreach (var n in number.ToString("00.00"))
            {
                if(n=='.')
                {
                    spriteBatch.Draw(
                        textures[assetName],
                        postion,
                        new Rectangle(10 * width, 0, width, 64),
                        Color.White * alpha);
                }
                else
                {
                    spriteBatch.Draw(
                        textures[assetName],
                        postion,
                        new Rectangle((n-'0') * width, 0, width, 64),
                        Color.White * alpha);

                }
                postion.X += width;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
      

        public void DrawTexture(string asseName,Vector2 position,Color color,float alpha=1.0f)
        {
            //デバックモード時のみ、画像描画前のアセット名をチェック
            Debug.Assert(
                textures.ContainsKey(asseName),
                "描画時にアセット名の指定を間違えたか、" + "画像の読み込み自体が出来ていません");

            spriteBatch.Draw(textures[asseName], position, color * alpha);
        }
    }
}
