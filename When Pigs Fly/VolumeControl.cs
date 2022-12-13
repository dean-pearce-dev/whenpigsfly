using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace When_Pigs_Fly
{
    class VolumeControl
    {
        private Game1 m_main;
        private float m_musicVolume = 0.05f;
        private float m_volumeIncrement = 0.05f;

        public VolumeControl()
        {

        }

        public VolumeControl(Game1 main)
        {
            SetMainRef(main);
        }

        private void SetMainRef(Game1 main)
        {
            m_main = main;
        }

        public void VolumeMenu()
        {
            if (m_musicVolume > 0.0f && m_main.GetPrevKeyboardState().IsKeyUp(Keys.Down) && m_main.GetCurrentKeyboardState().IsKeyDown(Keys.Down))
            {
                m_musicVolume -= m_volumeIncrement;

                // Clamping the volume
                if (m_musicVolume >= 1.0f)
                {
                    m_musicVolume = 1.0f;
                }

                // Rounding volume due to float inaccuracies
                m_musicVolume = (float)Math.Round(m_musicVolume * 100f) / 100f;
            }

            if (m_musicVolume < 1.0f && m_main.GetPrevKeyboardState().IsKeyUp(Keys.Up) && m_main.GetCurrentKeyboardState().IsKeyDown(Keys.Up))
            {
                m_musicVolume += m_volumeIncrement;

                // Clamping the volume
                if (m_musicVolume <= 0.0f)
                {
                    m_musicVolume = 0.0f;
                }

                // Rounding volume due to float inaccuracies
                m_musicVolume = (float)Math.Round(m_musicVolume * 100f) / 100f;
            }

            MediaPlayer.Volume = m_musicVolume;
        }

        public void VolumeDisplay()
        {

        }

        public float GetMusicVolume()
        {
            return m_musicVolume;
        }

        public void SetMusicVolume(float volume)
        {
            m_musicVolume = volume;
        }
    }
}
