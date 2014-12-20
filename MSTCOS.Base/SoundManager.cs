using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using  Microsoft.Xna.Framework.Audio;

namespace MSTCOS.Base
{
    /// <summary>
    /// 声音管理器
    /// 范若余
    /// </summary>
    public class SoundManager
    {
        
        public static void Play3DSound(SoundEffect e, Vector2 position,Vector2 listener,float cameraScale)
        {
            SoundEffectInstance i = e.CreateInstance();
            
            Play3DSound(i, position,listener,cameraScale);

        }
        public static void Play3DSound(SoundEffect e, Vector2 position, Vector2 listener, float cameraScale,float volume)
        {
            SoundEffectInstance i = e.CreateInstance();
            i.Volume = volume;
            Play3DSound(i, position, listener, cameraScale);

        }
        public static  void Play3DSound(SoundEffectInstance e, Vector2 position, Vector2 listener,float cameraScale)
        {

            AudioListener al = new AudioListener();


            al.Forward = new Vector3(0,0,-1);
            al.Up = new Vector3 (0,1,0);
            al.Position = new Vector3 (listener.X,300/cameraScale,-listener.Y);
            AudioEmitter ae = new AudioEmitter();
            ae.Position = new Vector3 (position.X,0,-position.Y);
            //ae.Position = position;

            e.Apply3D(al, ae);
            //if (e.State == SoundState.Stopped)
            //{

            e.Play();
            //}
        }

    }

    
}
