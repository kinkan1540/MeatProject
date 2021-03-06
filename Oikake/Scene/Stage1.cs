﻿using System;
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
    class Stage1 : IScene, IGameMediator
    {
        private CharacterManager characterManager;
        private List<Bullet> bullets;
        private bool IsEndFlag;
        private Sound sound;
        private Map1 map1;
        private Scene nextScene;
        private Player player;
        private Vector2 CenterCamera;
        private MoveBlock moveBlock;
        private Text text;
        private Robot robot;
        private Dictionary<int, string> stageNum_to_CSV = new Dictionary<int, string>()
        {
            {0, "Content/csv/Bolck.csv"},
            {1,"Content/csv/Bolck2.csv" }
        };

        public Stage1()
        {
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
            IsEndFlag = false;

            //マップ生成
            map1 = new Map1();
        }

        public void Draw(Renderer renderer)
        {
            int b = 150;
            int a = 0;
            renderer.Begin(CenterCamera);
            renderer.DrawTexture("mario", Vector2.Zero);
            map1.Draw(renderer);
            characterManager.Draw(renderer);

            renderer.End();

            renderer.Begin();
          
            for (int i = 1; i <= player.Hp; i++)
            {
                renderer.DrawTexture("hart", new Vector2(a, 0));

                a = a + b;
            }
            a = 0;
            if (robot.Isk)
            {
                for (int i = 1; i <= robot.Hp; i++)
                {
                    renderer.DrawTexture("hart", new Vector2(a, 100));
                    a = a + b;
                }
            }
            renderer.End();
        }
        public void Initialize()
        {
            moveBlock = new MoveBlock(this, new Vector2(320, 640), 2.5f);
            text = new Text("Hey", new Vector2(320, 640));
            Device.Camera.Initializa(Vector2.Zero);
            IsEndFlag = false;

            robot = new Robot(this, map1, 5);
            characterManager = new CharacterManager();
            characterManager.Initialize();

            characterManager.Add(robot);
            characterManager.Add(moveBlock);
            player = new Player(this, map1);
            characterManager.Add(player);

            bullets = new List<Bullet>();

            map1.Initialize();
        }

        public bool IsEnd()
        {
            return IsEndFlag;
        }

        public Scene Next()
        {
            if (player.IsTrap() == true)
            {
                sound.StopBGM();
                IsEndFlag = true;
                nextScene = Scene.Ending;
            }

            if (player.IsGoal() == true)
            {
                IsEndFlag = true;
                nextScene = Scene.GoodEnding;
            }

            if (characterManager.IsPlayerDead())
            {
                sound.StopBGM();
                IsEndFlag = true;
                nextScene = Scene.Ending;
            }


            return nextScene;
        }

        public void Shutdown()
        {

        }

        public void Update(GameTime gameTime)
        {
            CameraDraw();
            sound.PlayBGM("StageBGM");
            characterManager.Update(gameTime);
            Next();


            //マップ更新
            map1.Update();
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

            if (robot.Isk == false)
                CenterCamera = new Vector2(MathHelper.Clamp(player.GetPosition().X, minX, maxX), MathHelper.Clamp(centerY, minY, maxY));
            if (robot.Isk == true)
            {
                CenterCamera = new Vector2(MathHelper.Clamp(robot.GetPosition().X, minX, maxX), MathHelper.Clamp(centerY, minY, maxY));
            }
        }

        public bool IsBlock(Vector2 position)
        {
            if (map1.IsBloock(position) || characterManager.IsBlock(position))
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
            if (map1.IsBloock(position))
            {
                return true;
            }
            return false;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public Robot GetRobot()
        {
            return robot;
        }
    }
}
