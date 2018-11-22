using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Device;
namespace Oikake.Actor
{
    class CharaManager
    {
        private List<Character> player;
        private List<Character> enemy;
        private List<Character> addNewCharacters;
        public CharaManager()
        {
            Initialize();
        }
        public void Add(Character character)
        {
            if (character == null)
            {
                return;
            }
            addNewCharacters.Add(character);
        }
        public void Initialize()
        {
            if (player != null)
            { player.Clear(); }
            else
            { player = new List<Character>(); }
            if (enemy != null)
            { enemy.Clear(); }
            else
            { enemy = new List<Character>(); }
            if (addNewCharacters != null)
            { addNewCharacters.Clear(); }
            else
            { addNewCharacters = new List<Character>(); }
        }
        private void HitToCharacter()
        {
            foreach (var players in player)
            {
                foreach (var enemys in enemy)
                {
                    if (players.IsDead() || enemys.IsDead())
                    { continue; }
                    if (players.IsCollision(enemys))
                    {
                        players.Hit(enemys);
                        enemys.Hit(players);
                    }
                }
            }
        }
        
        public void RemoveCharacter()
        {
            player.RemoveAll(p => p.IsDead());
            enemy.RemoveAll(e => e.IsDead());
        }
        
        
        public void Update(GameTime gameTime)
        {
            foreach (var p in player)
            { p.Update(gameTime); }
            foreach (var e in enemy)
            { e.Update(gameTime); }
            foreach (var newChara in addNewCharacters)
            {

                if (newChara is Player)
                {
                    newChara.Initialize();
                    player.Add(newChara);
                }
                else
                {
                    newChara.Initialize();
                    enemy.Add(newChara);
                }
            }
            addNewCharacters.Clear();
            HitToCharacter();
            RemoveCharacter();
        }
        public void Draw(Renderer renderer)
        {
            foreach(var e in enemy)
            { e.Draw(renderer); }
            foreach(var p in player)
            { p.Draw(renderer); }
        }
    }
}
