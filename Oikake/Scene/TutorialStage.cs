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
    class TutorialStage:IScene,IGameMediator
    {
        private CharacterManager characterManager;
        private List<Bullet> bullets;
        private Score score;
        private Timer timer;
        private TimerUI timerUI;
        private bool IsEndFlag;
        private Sound sound;
        private TutorialMap tutorialMap;
        private Scene nextScene;
        private Player player;
        private Vector2 cameraCenter;
        private MoveBlock moveBlock;

        public TutorialStage(IScene scene)
        {
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
            IsEndFlag = false;
            //マップ生成
            tutorialMap = new TutorialMap();
        }
        public void Draw(Renderer renderer)
        {
            renderer.Begin(cameraCenter);
            renderer.DrawTexture("mario", Vector2.Zero);
            renderer.DrawTexture("Description2", new Vector2(270, 640));
            renderer.DrawTexture("Description1", new Vector2(100, 640));
            tutorialMap.Draw(renderer);
            characterManager.Draw(renderer);
            renderer.End();

            renderer.Begin();
            score.Draw(renderer);
          
            timerUI.Draw(renderer);
            renderer.End();
        }
        public void Initialize()
        {
            moveBlock = new MoveBlock(this, new Vector2(320, 640), 2);
            Device.Camera.Initializa(Vector2.Zero);
            IsEndFlag = false;
            player = new Player(this,tutorialMap);
            characterManager = new CharacterManager();
            characterManager.Initialize();
            characterManager.Add(new Player(this, tutorialMap));
            characterManager.Add(moveBlock);
            characterManager.Add(player);
            bullets = new List<Bullet>();
            score = new Score();
            timer = new CountDownTimer(30);
            timerUI = new TimerUI(timer);
            tutorialMap.Initialize();
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
                return nextScene = Scene.TutorialStage;
            }

            if (player.IsGoal() == true)
            {
                IsEndFlag = true;
                sound.StopBGM();
                nextScene = Scene.Title;
            }

            if (characterManager.IsPlayerDead())
            {
                IsEndFlag = true;
                nextScene = Scene.TutorialStage;
            }

            if (timer.IsTime() == true)
            {
                IsEndFlag = true;
                nextScene = Scene.TutorialStage;
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
            score.Update(gameTime);
            
            timer.Update(gameTime);
            if (timer.IsTime() == true)
            {
                score.Shutdown();
                IsEndFlag = true;
            }
            //マップ更新
            tutorialMap.Update();
        }

        public void AddActor(Character character)
        {
            characterManager.Add(character);
        }

        public void AddScore()
        {
            score.Add();
        }

        public void AddScore(int num)
        {
            score.Add(num);
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
            if (tutorialMap.IsBloock(position) || characterManager.IsBlock(position))
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
            if(tutorialMap.IsBloock(position))
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
