using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oikake.Device;
using Oikake.Actor;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Oikake.Scene;
using Oikake.Def;
using Oikake.Util;

namespace Oikake.Actor
{
    class Enemy :Character

    {
        private AI ai;
        private Random rnd;
        private State state;//状態
        private Timer timer;//表示用切り替え時間
        private bool isDisplay;//表示中か
        private readonly int Impression = 10;//表示回数
        private int displayCount;//表示カウンタ
       

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="ai"></param>
        public Enemy(IGameMediator mediator, AI ai) : base("black", mediator)
        {
            this.ai = ai;
            state = State.Preparation;
        }
        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public override void Initialize()
        {
            var gameDevice = GameDevice.Instance();
            rnd = gameDevice.GetRandom();
            position = new Vector2(
                rnd.Next(Screen.Width - 64),
                rnd.Next(Screen.Height - 64));
            //初期状態は準備に
            state = State.Preparation;
            //点滅関連
            timer = new CountDownTimer(0.25f);
            isDisplay = true;
            displayCount = Impression;//点滅回数を設定
        }
        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public override void Update(GameTime gameTime)
        {
            //AIが決定した位置に
            position = ai.Think(this);

            switch (state)
            {
                case State.Preparation:
                    PreparationUpdate(gameTime);
                    break;
                case State.Alive:
                    AliveUpdate(gameTime);
                    break;
                case State.Dying:
                    DyingUpdate(gameTime);
                    break;
                case State.Dead:
                    DeadUpdate(gameTime);
                    break;
            }
        }
        public override void Draw(Renderer renderer)
        {
            switch (state)
            {
                case State.Preparation:
                    PreparationDraw(renderer);
                    break;
                case State.Alive:
                    AliveDraw(renderer);
                    break;
                case State.Dying:
                    DyingDraw(renderer);
                    break;
                case State.Dead:
                    DeadDraw(renderer);
                    break;
            }
        }
        public override void Hit(Character other)
        {
            //ガード節
            if (state != State.Alive)
            {
                return;
            }
            //状態変更
            state = State.Dying;

            //得点の追加
            int score = 0;
         

            isDeadFlag = true;
     
            //次のAIを決定
           
     
            //死亡処理
           // isDeadFlag= true;
           // mediator.AddActor(new BurstEffect(position, mediator));
        }
     
        public override void Shutdown()
        {
        }

        private void PreparationUpdate(GameTime gameTime)
        {
            timer.Update(gameTime);
            if(timer.IsTime())
            {
                isDisplay = !isDisplay;//フラグ反転
                displayCount -= 1;
                timer.Initialize();
            }
            if(displayCount==0)
            {
                state = State.Alive;//生存状態に
                timer.Initialize();
                displayCount = Impression;
                isDisplay = true;
            }
        }

        private void PreparationDraw(Renderer renderer)
        {
            if(isDisplay)
            {
                base.Draw(renderer);
            }
        }
        private void AliveUpdate(GameTime gameTime)
        {
            position = ai.Think(this);
        }

        private void AliveDraw(Renderer renderer)
        {
            base.Draw(renderer);
        }
        
        private void DyingUpdate(GameTime gameTime)
        {
            timer.Update(gameTime);
            if (timer.IsTime())
            {
                displayCount -= 1;
                timer.Initialize();
                isDisplay = !isDisplay;
            }
            if(displayCount==0)
            {
                state = State.Dead;
            }

        }

        private void DyingDraw(Renderer renderer)
        {
            if(isDisplay)
            {
                renderer.DrawTexture(name, position, Color.Red);
            }
            else
            {
                base.Draw(renderer);
            }
        }
        private void DeadUpdate(GameTime gameTime)
        {
            isDeadFlag = true;
            mediator.AddActor(new BurstEffect(position, mediator));
        }
        private void DeadDraw(Renderer renderer)
        {

        }

       
    }
}
