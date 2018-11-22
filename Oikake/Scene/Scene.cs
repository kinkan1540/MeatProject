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
namespace Oikake.Scene
{
    enum Scene
    {        
        Title,
        TutorialStage,
        Stage1,
        Stage2,
        CheckScene,
        Ending,     
        GoodEnding
    }
    interface IGameMediator
    {
        void AddActor(Character character);
        bool IsBlock(Vector2 position);

        MoveBlock IsMoveBlock(Vector2 position);

         bool IsMapBlock(Vector2 position);
    }
}
