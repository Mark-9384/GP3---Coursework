using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GP3_Coursework
{
    public class Timer
    {
        // counter for reload timer 
         public int counter = 1;
        // counts every second for reload timer
         public float countDuration = 1f; //every  2s.
        // current time for reload timer
         public float currentTime = 0f;
        // reload time
         public int time = 0;
    }




}
