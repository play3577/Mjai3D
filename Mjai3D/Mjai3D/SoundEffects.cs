using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using System.IO;

namespace Mjai3D
{
    static class SoundEffects
    {
        static SoundEffect pon;
        static SoundEffect chi;
        static SoundEffect kan;
        static SoundEffect tsumo;
        static SoundEffect ron;
        static SoundEffect reach;

        static SoundEffect dahai;

        static SoundEffect FromFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return SoundEffect.FromStream(stream);
            }
        }

        public static void Initialize()
        {
            pon = FromFile("../pon.wav");
            chi = FromFile("../chi.wav");
            kan = FromFile("../kan.wav");
            tsumo = FromFile("../tsumo.wav");
            ron = FromFile("../ron.wav");
            reach = FromFile("../reach.wav");
            dahai = FromFile("../dahai.wav");
        }

        public static void Pon(float pan)
        {
            pon.Play(1f, 0f, pan);
        }

        public static void Chi(float pan)
        {
            chi.Play(1f, 0f, pan);
        }

        public static void Kan(float pan)
        {
            kan.Play(1f, 0f, pan);
        }

        public static void Ron(float pan)
        {
            ron.Play(1f, 0f, pan);
        }

        public static void Tsumo(float pan)
        {
            tsumo.Play(1f, 0f, pan);
        }

        public static void Reach(float pan)
        {
            reach.Play(1f, 0f, pan);
        }

        public static void Dahai(float pan)
        {
            dahai.Play(1f, 0f, pan);
        }


    }
}
