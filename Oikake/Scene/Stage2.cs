using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Actor;
using Oikake.Device;
using Oikake.Util;
using Oikake.Objects;
using Oikake.Def;

namespace Oikake.Scene
{
    class Stage2 : IScene, IGameMediator
    {
        private CharacterManager characterManager;
        private List<Bullet> bullets;
        private Timer timer;
        private TimerUI timerUI;
        private bool IsEndFlag;
        private Sound sound;
        private Map2 map2;
        private Scene nextScene;
        private Player player;
        private Vector2 cameraCenter;
        private MoveBlock moveBlock;


        public Stage2(IScene scene)
        {
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
            IsEndFlag = false;
            //マップ生成
            map2 = new Map2();
        }
        public void Draw(Renderer renderer)
        {
            renderer.Begin(cameraCenter);
            renderer.DrawTexture("mario", Vector2.Zero);
            map2.Draw(renderer);
            characterManager.Draw(renderer);
            renderer.End();

            renderer.Begin();
            timerUI.Draw(renderer);
            renderer.End();
        }
        public void Initialize()
        {
            moveBlock = new MoveBlock(this,new Vector2(320, 320), 3);
            Device.Camera.Initializa(Vector2.Zero);
            IsEndFlag = false;
            player = new Player(this,map2);
            characterManager = new CharacterManager();
            characterManager.Initialize();
            characterManager.Add(new Player(this,map2));
            characterManager.Add(player);
            characterManager.Add(moveBlock);
            bullets = new List<Bullet>();
            timer = new CountDownTimer(30.0f);
            timerUI = new TimerUI(timer);
            map2.Initialize();
        }

        public bool IsEnd()
        {
            return IsEndFlag;
        }

        public Scene Next()
        {
            if (player.IsTrap() == true)
            {
                IsEndFlag = true;
                nextScene = Scene.CheckScene;
            }

            if (player.IsGoal() == true)
            {
                IsEndFlag = true;
                sound.StopBGM();
                nextScene = Scene.GoodEnding;
            }

            if (characterManager.IsPlayerDead())
            {
                IsEndFlag = true;
                nextScene = Scene.CheckScene;
            }

            if (timer.IsTime() == true)
            {
                IsEndFlag = true;
                nextScene = Scene.CheckScene;
            }
            return nextScene;
        }

        public void Shutdown()
        {

        }

        public void Update(GameTime gameTime)
        {
            CameraDraw();
            sound.PlayBGM("TitleSound");
            characterManager.Update(gameTime);
            Next();
            timer.Update(gameTime);
            if (timer.IsTime() == true)
            {
                IsEndFlag = true;
            }
            //マップ更新
            map2.Update();
        }

        public void AddActor(Character character)
        {
            characterManager.Add(character);
        }

      
        public void CameraDraw()
        {
            float mapHeightSize = 32.0f * 25.0f;

            //カメラのズーム率を考慮したスクリーンの縦方向のサイズ
            float currentScreenHeight = Screen.Height / Def.Camera.Zoom;

            float centerY = player.GetPosition().Y;
            float minY = currentScreenHeight / 2.0f;
            float maxY = mapHeightSize - currentScreenHeight / 2.0f;

            float mapWidth = 40.0f * 32.0f;

            float currentScreenWidth = Screen.Width / Def.Camera.Zoom;

            float minX = currentScreenWidth / 2.0f;
            float maxX = mapWidth - currentScreenWidth / 2.0f;

            cameraCenter = new Vector2(MathHelper.Clamp(player.GetPosition().X, minX, maxX), MathHelper.Clamp(centerY, minY, maxY));
        }

        public bool IsBlock(Vector2 position)
        {
            if (map2.IsBloock(position) || characterManager.IsBlock(position))
            {
                return true;
            }

            return false;
        }

        public MoveBlock IsMoveBlock(Vector2 position)
        {
            return characterManager.IsMoveBlock(position);
        }

        public bool IsMapBlock(Vector2 position)
        {
            if(map2.IsBloock(position))
            {
                return true;
            }
            return false;
        }

        public Player GetPlayer()
        {
            return player;
        }
    }
}
