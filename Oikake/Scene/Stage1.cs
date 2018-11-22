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
    class Stage1 : IScene, IGameMediator
    {
        private CharacterManager characterManager;
        private List<Bullet> bullets;
        private Score score;
        private Timer timer;
        private TimerUI timerUI;
        private bool IsEndFlag;
        private Sound sound;
        private Map1 map1;
        private Scene nextScene;
        private Player player;
        private Vector2 CenterCamera;
        private MoveBlock moveBlock;
        private Text text;

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
            renderer.Begin(CenterCamera);
            renderer.DrawTexture("mario", Vector2.Zero);
            map1.Draw(renderer);
            characterManager.Draw(renderer);
            renderer.End();

            renderer.Begin();
            timerUI.Draw(renderer);
            renderer.End();
        }
        public void Initialize()
        {
            moveBlock = new MoveBlock(this, new Vector2(320,640),2.5f);
            text = new Text("Hey",new Vector2(320,640));
            Device. Camera.Initializa(Vector2.Zero);
            IsEndFlag = false;
            player = new Player(this, map1);
            characterManager = new CharacterManager();
            characterManager.Initialize();
            characterManager.Add(player);
            characterManager.Add(moveBlock); 

            bullets = new List<Bullet>();
            timer = new CountDownTimer(20);
            timerUI = new TimerUI(timer);
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
                nextScene = Scene.CheckScene;
            }

            if (characterManager.IsPlayerDead())
            {
                sound.StopBGM();
                IsEndFlag = true;
                nextScene = Scene.Ending;
            }

            if(timer.IsTime()==true)
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
            sound.PlayBGM("TitleSound");
            characterManager.Update(gameTime);
            Next();
       
            timer.Update(gameTime);
            if (timer.IsTime() == true)
            {
               
                IsEndFlag = true;
            }
            //マップ更新
            map1.Update();
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

           CenterCamera = new Vector2(MathHelper.Clamp(player.GetPosition().X, minX, maxX), MathHelper.Clamp(centerY, minY, maxY));
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
            if(map1.IsBloock(position))
            {
                return true;
            }
            return false;
        }
    }
}
