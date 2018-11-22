using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Oikake.Device
{
    class Sound
    {
        #region
        private ContentManager contentManager;
        private Dictionary<string, Song> bgms;
        private Dictionary<string, SoundEffect> soundEffect;
        private Dictionary<string, SoundEffectInstance> seInstances;
        private Dictionary<string, SoundEffectInstance> sePlayDict;
        private string currentBGM;

        public Sound(ContentManager content)
        {
            contentManager = content;
            MediaPlayer.IsRepeating = true;
            bgms = new Dictionary<string, Song>();
            soundEffect = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();
            sePlayDict = new Dictionary<string, SoundEffectInstance>();
            currentBGM = null;
        }
        public void Unload()
        {
            bgms.Clear();
            soundEffect.Clear();
            seInstances.Clear();
            sePlayDict.Clear();
        }
        #endregion フィールドとコンストラクタ
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名(" + name + ")がありません" +
                "アセット名の確認、Dictionaryに登録しているか確認してください";

        }
        #region BGM(MP3:MediaPlayer)

        public void LoadBGM(string name,string filepach="./")
        {
            if(bgms.ContainsKey(name))
            {
                return;
            }
            bgms.Add(name, contentManager.Load<Song>(filepach + name));
        }

        public bool IsStoppedBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }
        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }
        public bool IsPausedBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }
        public void StopBGM()
        {
            MediaPlayer.Stop();
            currentBGM = null;
        }
        public void PlayBGM(string name)
        {
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));
            if(currentBGM==name)
            {
                return;
            }
            if(IsPlayingBGM())
            {
                StopBGM();
            }
            MediaPlayer.Volume = 0.5f;
            currentBGM = name;
            MediaPlayer.Play(bgms[currentBGM]);
        }
        public void PauseBGM()
        {
            if(IsPlayingBGM())
            {
                MediaPlayer.Pause();
            }

        }
        public void ResumeBGM()
        {
            if(IsPausedBGM())
            {
                MediaPlayer.Resume();
            }
        }
        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }
        #endregion BGM

        #region 関連

        public void LoadSE(string name,string filePach="./")
        {
            if(soundEffect.ContainsKey(name))
            {
                return;
            }
            soundEffect.Add(name, contentManager.Load<SoundEffect>(filePach + name));
        }
        public void PlaySE(string name)
        {
            Debug.Assert(soundEffect.ContainsKey(name), ErrorMessage(name));
            soundEffect[name].Play();
        }
        #endregion
        #region
        public void CreateSEInstance(string name)
        {
            if (seInstances.ContainsKey(name))
            {
                return;
            }
            Debug.Assert(soundEffect.ContainsKey(name), "先に" + name + "の読み込み処理を行ってください");
            seInstances.Add(name, soundEffect[name]. CreateInstance());

        }
        public void PlaySEInstance(string name,int no, bool loopFlag=false)
        {
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));
            if(sePlayDict.ContainsKey(name+no))
            {
                return;
            }
            var date = seInstances[name];
            date.IsLooped = loopFlag;
            date.Play();
            sePlayDict.Add(name + no, date);
        }

        public void SttopedSE(string name,int no)
        {
            if(sePlayDict.ContainsKey(name+no)==false)
            {
                return;
            }
            if(sePlayDict[name+no].State==SoundState.Playing)
            {
                sePlayDict[name + no].Stop();
            }
        }
        public void SttopedSE()
        {
            foreach(var se in sePlayDict)
            {
                if(se.Value.State==SoundState.Playing)
                {
                    se.Value.Stop();
                }
            }
        }
        public void RemoveSE(string name,int no)
        {
            if(sePlayDict.ContainsKey(name+no)==false)
            {
                return;
            }
            sePlayDict.Remove(name + no);
        }
        public void RemoveSE()
        {
            sePlayDict.Clear();
        }
        public void PauSeSE(string name,int no)
        {
            if(sePlayDict.ContainsKey(name+no)==false)
            {
                return;
            }
            if(sePlayDict[name+no].State==SoundState.Playing)
            {
                sePlayDict[name + no].Pause();
            }

        }
        public void PauseSE()
        {
            foreach(var se in sePlayDict)
            {
                if(se.Value.State==SoundState.Playing)
                {
                    se.Value.Pause();
                }
            }
        }
        public void ResumeSE(string name,int no)
        {
            if(sePlayDict.ContainsKey(name+no)==false)
            {
                return;
            }
            if(sePlayDict[name+no].State==SoundState.Paused)
            {
                sePlayDict[name + no].Resume();
            }
        }
        public bool IsPlayingSEInstance(string name,int no)
        {
            return sePlayDict[name + no].State == SoundState.Playing;
        }
        public bool isStoppedSEInstance(string name,int no)
        {
            return sePlayDict[name + no].State == SoundState.Stopped;
        }
        public bool IsPauseSEInstance(string name,int no)
        {
            return sePlayDict[name + no].State == SoundState.Paused;
        }
        #endregion
    }
}
