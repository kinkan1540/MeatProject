using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Oikake.Device;
using Oikake.Objects;
namespace Oikake.Actor
{
    class CharacterManager
    {
        private List<Character> players;
        private List<Character> enemys;

        private List<Character> addNewCharacters;
        public CharacterManager()
        {
        }
        public void Initialize()
        {
            if (players != null)
            {
                players.Clear();
            }
            else
            {
                players = new List<Character>();
            }
            if (enemys != null)
            {
                enemys.Clear();
            }
            else
            {
                enemys = new List<Character>();
            }
            if (addNewCharacters != null)
            {
                addNewCharacters.Clear();
            }
            else
            {
                addNewCharacters = new List<Character>();
            }
        }
        public void Add(Character character)
        {
            if (character == null)
            {
                return;
            }
            else
            {
                addNewCharacters.Add(character);
            }
        }
        public void HitToCharacters()
        {
            foreach (var Player in players)
            {
                foreach (var enemy in enemys)
                {
                    if (Player.IsDead() || enemy.IsDead())
                    {
                        continue;
                    }
                    if (Player.IsCollision(enemy))
                    {
                        Player.Hit(enemy);
                        enemy.Hit(Player);
                    }
                }
            }
        }
        private void RemoveDeadCharacter()
        {
            players.RemoveAll(p => p.IsDead());
            enemys.RemoveAll(e => e.IsDead());
        }
        public void Update(GameTime gameTime)
        {
            foreach (var p in players)
            {
                p.Update(gameTime);
            }
            foreach (var e in enemys)
            {
                e.Update(gameTime);
            }

            HitToCharacters();

            //追加候補者をリストに追加
            foreach (var newChara in addNewCharacters)
            {
                //キャラクタがプレイヤーだったらプレイやリストに登録
                if (newChara is Player)
                {
                    newChara.Initialize();//初期化
                    players.Add(newChara);//登録
                }
                //プレイヤーの弾だったらプレイヤーリストに追加
                else　if(newChara is PlayerBullet)
                {
                    newChara.Initialize();
                    players.Add(newChara);
                }
                //動く床だったらプレイヤーに追加
                else if(newChara is MoveBlock)
                {
                    newChara.Initialize();
                    players.Add(newChara);
                }
                //それ以外は敵リストに追加
                else
                {
                    newChara.Initialize();//初期化
                    enemys.Add(newChara);//登録
                }
            }
            //追加処理後、追加リストはクリア
            addNewCharacters.Clear();

            //死亡フラグが立ってたら消去
            RemoveDeadCharacter();

        }
        public void Draw(Renderer renderer)
        {
            foreach (var e in enemys)
            {
                e.Draw(renderer);
            }
            foreach (var p in players)
            {
                p.Draw(renderer);
            }
         
        }

        public bool IsPlayerDead()
        {
            foreach (var p in players)
            {
                if (p is Player)
                {
                    if (p.IsDead())
                    {
                        return true;
                    }
                    return false;
                }
            }

            return true;
        }

        public bool IsBlock(Vector2 position)
        {
            Point point = new Point((int)position.X, (int)position.Y);
            foreach (var block in players)
            {
                if (block is MoveBlock)
                {
                    Vector2 pos = block.GetPosition();
                    Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, 64, 32 );

                    if (rect.Contains(point))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public MoveBlock IsMoveBlock(Vector2 position)
        {
            Point point = new Point((int)position.X, (int)position.Y);
            foreach (var block in players)
            {
                if (block is MoveBlock)
                {
                    Vector2 pos = block.GetPosition();
                    Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, 64, 32);

                    if (rect.Contains(point))
                    {
                        return (MoveBlock)block;
                    }
                }
            }
            return null;
        }
        
    }
}
