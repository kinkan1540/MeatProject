using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oikake.Device;
using Oikake.Def;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Oikake.Scene;
using Oikake.Util;
using Oikake.Objects;


namespace Oikake.Actor
{

    class Player : Character
    {
        #region　変数
        Vector2 velocity = Vector2.Zero;
        private Sound sound;
        private Motion motion;
        private bool isJump;
        private float gravity = 0.5f;
        private Map1 map;
        private bool isMove;
        private int moveCount;
        private List<Vector2> movePos;
        private Timer timer;
        private int bulletNum;
       
        private int counter;
        private CountUpTimer countUp;
        //各ブロック調査用
        private List<Vector2> rightPos;
        private List<Vector2> leftPos;
        private List<Vector2> upPos;
        private List<Vector2> downPos;
        private int Damege;

        private bool isDamage;
        private int damageCnt;
        #endregion 

        /// <summary>
        /// 向き
        /// </summary>
        private enum Direction
        {
            RIHT, DOWN, UP, LEFT
        };
        private Direction direction;//現在の向き
        //向きと範囲を管理
        private Dictionary<Direction, Range> directionRange;




        /// <summary>
        /// モーションの変更
        /// </summary>
        /// <param name="direction">変更したい向き</param>
        private void ChangeMotion(Direction direction)
        {
            this.direction = direction;
            motion.Initialize(directionRange[direction], new CountDownTimer(0.2f));
        }

        private void UpdateMotion()
        {
            //キー入力の状態を取得
            //Vector2 velocity = Input.Velocity();
            //キー入力がなければ何もしない
            if (velocity.Length() <= 0.0f)
            {
                return;
            }
            //キーが入力あったとき
            //右向きに変更
            else if (velocity.X >= 0.0f && (direction != Direction.RIHT))
            {
                ChangeMotion(Direction.RIHT);

            }
            //左向きに変更
            else if ((Input.Velocity().X < 0.0f) && (direction != Direction.LEFT))
            {
                ChangeMotion(Direction.LEFT);

            }
        }

        public Player(IGameMediator mediator, Map1 map1)
            : base("Player", mediator)
        {
            Device.Camera.GetScreenPos(position);
            this.map = map1;
            position = Vector2.Zero;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
            //リストの生成
            rightPos = new List<Vector2>() { new Vector2(27.5f, 0), new Vector2(27, 30) };
            leftPos = new List<Vector2>() { new Vector2(-1f, 0), new Vector2(-1f, 30) };
            upPos = new List<Vector2>() { new Vector2(0, -1), new Vector2(23, -1) };
            downPos = new List<Vector2>() { new Vector2(7, 32), new Vector2(25, 32) };
        }

        /// <summary>
        /// CharacterクラスのDrawメソッドに代わって描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {
         
            if (isDamage)
            {
                if (counter % 10 == 0)
                {
                    renderer.DrawTexture(name, position, motion.DrawingRange());
                }
            }
            if (counter == 0)
            {
                renderer.DrawTexture(name, position, motion.DrawingRange());
            }
        }

        public override void Initialize()
        {
            counter = 0;
            countUp = new CountUpTimer();
            bulletNum = 5;
            Hp = 3;
            timer = new CountDownTimer(0.3f);
            IsGoal();
            IsTrap();
            direction = new Direction();
            isJump = false;
            isDeadFlag = false;
            velocity = Vector2.Zero;
            position = new Vector2(32, 736);
            motion = new Motion();
            MotionInit();

            damageCnt = 0;
        }

        public override void Update(GameTime gameTime)
        {
            Damege = 1;
            BulletUpdate();
            FallStart();
            JumpUpdate();
            MoveUpdate();
            IsGoal();
            UpdateMotion();
            motion.Update(gameTime);
            IsTrap();
            Ride();
            Invincibly();
            DamgeUpdate();

            if (Hp <= 0)
            {
                isDeadFlag = true;
            }

            if (position.Y >= Screen.Height)
            {
                isDeadFlag = true;
            }
            
        }
        public override void Hit(Character other)
        {
            if (other is MoveBlock)
            {
                if (IsUp(other))
                {
                    position += ((MoveBlock)other).Velocity();
                }
            }
        }

        public override void Shutdown()
        {

        }

        private void MoveUpdate()
        {
            //キー入力の移動量を取得
            velocity.X = Input.Velocity().X * 1.5f;
            //入力されていれば移動処理状態にする
            if (velocity.Length() > 0)
            {
                isMove = true;
            }
            if (isMove)
            {
                //移動
                Move();
                //カウンタ加算
                moveCount += 1;
                //8回移動したら
                if (moveCount >= 1)
                {
                    isMove = false;
                    moveCount = 0;
                }
            }
        }


        private void MotionInit()
        {
            //下向き
            for (int i = 0; i < 3; i++)
            {
                motion.Add(i, new Rectangle(32 * (i % 3), 32 * (i / 3), 32, 32));
            }
            //上向き
            for (int i = 9; i < 12; i++)
                motion.Add(i, new Rectangle(32 * (i % 3), 32 * (i / 3), 32, 32));

            //右向き
            for (int i = 6; i < 9; i++)
            {
                motion.Add(i, new Rectangle(32 * (i % 3), 32 * (i / 3), 32, 32));
            }
            //左向き
            for (int i = 3; i < 6; i++)
            {
                motion.Add(i, new Rectangle(32 * (i % 3), 32 * (i / 3), 32, 32));
            }
            direction = Direction.RIHT;
            directionRange = new Dictionary<Direction, Range>()
            {
                {Direction.RIHT,new Range(6,8) },
                {Direction.LEFT,new Range(3,5) }
            };

            motion.Initialize(directionRange[direction], new CountDownTimer(0.2f));
        }
        private void Move()
        {
            XMove();
            YMove();
        }

        private void XMove()
        {
            //方向によるチェック位置を指定
            //右移動の場合
            if (velocity.X > 0)
            {
                movePos = rightPos;
            }
            //左移動の場合
            if (velocity.X < 0)
            {
                movePos = leftPos;
            }

            //1マスずつ移動する
            for (int i = 0; i < Math.Abs(velocity.X); i++)
            {
                foreach (Vector2 pos in movePos)
                {
                    if (mediator.IsBlock(position + pos))
                    {

                        if (Input.GetKeyTrigger(Keys.Space) || Input.GetKeyTrigger(PlayerIndex.One, Buttons.B))
                        {
                            if (isJump == false)
                            {
                                isJump = true;
                            }
                        }
                        return;
                    }
                }

                position.X += velocity.X;
            }
        }
        private void YMove()
        {
            var robot = mediator.GetRobot();
            //下移動の場合
            if (velocity.Y > 0)
            {
                movePos = downPos;

            }
            //上移動の場合
            if (velocity.Y < 0)
            {
                movePos = upPos;
            }
            //1マスずつ移動する
            for (int i = 0; i < Math.Abs(velocity.Y); i++)
            {
                foreach (Vector2 pos in movePos)
                {
                    if (mediator.IsBlock(position + pos))
                    {
                        // 下向きでぶつかったとき
                        if (velocity.Y > 0)
                        {
                            isJump = false;
                            velocity.Y = 0;
                        }
                        // 上向きにぶつかったとき
                        else
                        {
                            isJump = true;
                            velocity.Y = 0;

                        }
                        return;
                    }
                    else
                    {
                        isJump = true;
                    }
                }
                position.Y += Math.Sign(velocity.Y);
            }
        }

        private void JumpUpdate()
        {
            if (Input.GetKeyTrigger(Keys.Space) || Input.GetKeyTrigger(PlayerIndex.One, Buttons.A))
            {
                if (isJump == false)
                {
                    isJump = true;
                    velocity.Y -= 10;
                }
            }
            if (isJump)
            {
                velocity.Y += gravity;
            }
        }
        public bool IsGoal()
        {
            if (map.IsGoal(position + new Vector2(16, 16)))
            {
                return true;
            }
            return false;
        }

        public bool IsTrap()
        {
            if (map.IsTrap(position + new Vector2(16, 16)))
            {
                return true;
            }
            return false;
        }

        private void FallStart()
        {
            if (!isJump)
            {
                // 下にブロックがあるかどうか
                foreach (Vector2 pos in downPos)
                {
                    if (mediator.IsBlock(position + pos))
                    {
                        return;
                    }
                }
                // 下にブロックがないので
                isJump = true;
            }
        }

        private bool IsUp(Character other)
        {
            Point p = GetRect().Center - other.GetRect().Center;
            Vector2 vec = new Vector2(p.X, p.Y);

            if (Math.Abs(vec.X) < Math.Abs(vec.Y))
            {
                if (vec.Y < 0)
                {
                    return true;
                }
            }

            return false;
        }


        private void Ride()
        {
            foreach (Vector2 pos in downPos)
            {
                MoveBlock mb = mediator.IsMoveBlock(position + pos);

                if (mb != null)
                {
                    position += mb.Velocity();
                    return;
                }
            }
        }

        /// <summary>
        /// プレイヤーの銃管理
        /// </summary>
        private void BulletUpdate()
        {
            if (Input.GetKeyTrigger(Keys.Z) || Input.IskeyPadDown(PlayerIndex.One, Buttons.RightShoulder))
            {
                if (bulletNum > 0)
                {
                    Vector2 velocity = new Vector2(1, 0);
                    if (direction == Direction.LEFT)
                    {
                        velocity = new Vector2(-1, 0);
                    }
                    if (bulletNum > 0)
                    {
                        mediator.AddActor(
                      new PlayerBullet(
                          position,
                          mediator,
                          velocity));
                        bulletNum--;
                    }
                }
            }
            if (Input.GetKeyTrigger(Keys.E) || Input.IskeyPadDown(PlayerIndex.One, Buttons.X))
            {
                bulletNum = 5;
            }
        }

        /// <summary>
        /// ダメージ管理
        /// </summary>
        private void Invincibly()
        {
            if (mediator.GetRobot().IsGetOn == false)
            {
                if (Math.Abs(mediator.GetRobot().GetPosition().X - position.X) < 32 && Math.Abs(GetPosition().Y - mediator.GetRobot().GetPosition().Y) < 32)
                {
                    if (isDamage == false)
                    {
                        isDamage = true;
                        Hp -= Damege;
                    }
                }
            }
        }

        /// <summary>
        /// ダメージを受けた際の無敵時間
        /// </summary>
        private void DamgeUpdate()
        {
            if (IsGetOn == false)
            {
                if (isDamage)
                {
                    damageCnt++;
                    counter++;
                    Damege = 0;
                    if (100 < damageCnt)
                    {
                        isDamage = false;
                        damageCnt = 0;
                        counter = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 落下した際の死亡flag
        /// </summary>
        private void FaleDead()
        {
            if (position.Y >= Screen.Height)
            {
                isDeadFlag = true;
            }
        }
    }
}
