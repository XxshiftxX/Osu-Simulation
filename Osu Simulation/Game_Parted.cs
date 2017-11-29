using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace Osu_Simulation
{
    partial class Game1
    {
        DateTime gameStartedPoint;
        Stack<HitObject> HitObjects = new Stack<HitObject>();
        List<HitObject> DisplayingHitObjects = new List<HitObject>();
        HitObjectHeap[] CreatedHitObjects = new HitObjectHeap[4] {
            new HitObjectHeap(), new HitObjectHeap(), new HitObjectHeap(), new HitObjectHeap()
        };

        int sync = 500;

        Dictionary<string, string> OsuMetaData = new Dictionary<string, string>();

        int Combo;
        int GamePoint;
        float Health = 300;
        string judgeMessage = "";

        DateTime drawLoopPoint = new DateTime(0);

        private void ReadOsuFile(string filePath)
        {
            string allString = File.ReadAllText(filePath);
            Regex regex = new Regex(@"(\d+),(\d+),(\d+),(\d+),(\d+),(\d+):(\d+):(\d+):(\d+):");
            MatchCollection matches = regex.Matches(allString);
            regex = new Regex(@"AudioFilename: (\w+)");
            HitObjects.Clear();
            
            for(int i = matches.Count - 1; i >= 0; i--)
            {
                int line = 0, time;
                switch(int.Parse(matches[i].Groups[1].Value))
                {
                    case 64:
                        line = 0;
                        break;
                    case 192:
                        line = 1;
                        break;
                    case 320:
                        line = 2;
                        break;
                    case 448:
                        line = 3;
                        break;
                }
                time = int.Parse(matches[i].Groups[3].Value);

                HitObjects.Push(new HitObject(line, time));
            }
        }

        private void PlayMusic(string filePath, string fileName)
        {
            MMDevice device;
            ISoundOut soundOut;
            IWaveSource soundSource;

            string tempPath = Path.Combine(Path.GetDirectoryName(filePath), fileName);
            soundSource = CodecFactory.Instance.GetCodec(tempPath);

            device = new MMDeviceEnumerator().EnumAudioEndpoints(DataFlow.Render, DeviceState.Active)[0];

            soundOut = new WasapiOut() { Device = device, Latency = 100 };
            soundOut.Initialize(soundSource);

            soundOut.Play();
        }

        private void PlayGame()
        {
            drawLoopPoint = DateTime.Now;

            // 나중에 ReadOsuFile에서 파일 읽어오기
            PlayMusic(@"C:\Users\dsm2017\AppData\Local\osu!\Songs\654486 kamome sano Electric Orchestra - FIN4LE -Shushisen no Kanata e-", @"C:\Users\dsm2017\AppData\Local\osu!\Songs\654486 kamome sano Electric Orchestra - FIN4LE -Shushisen no Kanata e-\audio.mp3");
            gameStartedPoint = DateTime.Now;

            loopDelegate += () =>
            {
                double nowTiming = (DateTime.Now - gameStartedPoint).TotalMilliseconds;
                if (HitObjects.Count > 0)
                {
                    HitObject checkHitObject = HitObjects.Peek();

                    if (checkHitObject.Time <= nowTiming + sync)
                    {
                        DisplayingHitObjects.Add(checkHitObject);
                        CreatedHitObjects[HitObjects.Pop().Line].Add(checkHitObject);
                        loopDelegate();
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (CreatedHitObjects[i].Count == 0)
                        continue;

                    HitObject temp = CreatedHitObjects[i].RemoveOne();
                    if (temp.Time + 200 < nowTiming)
                    {
                        Combo = 0;
                        Health -= 50;
                        judgeMessage = "MISS!";
                    }
                    else
                    {
                        CreatedHitObjects[i].Add(temp);
                    }
                }

                /*
                if (DisplayingHitObjects.Count > 0 && DisplayingHitObjects[0].Time <= nowTiming)
                {
                    System.Diagnostics.Debug.WriteLine(test++);
                    DisplayingHitObjects.Remove(DisplayingHitObjects[0]);
                }
                */
                
            };
        }

        private bool DrawTiming()
        {
            if(drawLoopPoint < DateTime.Now)
            {
                drawLoopPoint.Add(new TimeSpan(10));
                return true;
            }
            return false;
        }

        private void KeyInput(int input, DateTime inputTime)
        {
            int intervel;
            HitObject min = null;
            HitObject max = null;
            HitObject nearest;
            double nowTiming = (DateTime.Now - gameStartedPoint).TotalMilliseconds;

            for (int i = 0; i < CreatedHitObjects[input].Count; i++)
            {
                HitObject temp = CreatedHitObjects[input].RemoveOne();
                DisplayingHitObjects.Remove(temp);

                if (temp.Time > nowTiming)
                {
                    max = temp;
                    break;
                }
                else
                {
                    min = temp;
                }
            }

            if (min == null)
                return;
            if (max == null || max > min)
            {
                nearest = min;
            }
            else
            {
                nearest = max;
            }

            intervel = (int)((inputTime - gameStartedPoint).TotalMilliseconds) - nearest.Time;

            if (intervel < 0) intervel = -intervel;
            if (intervel < 200)
            {
                if (intervel < 30)
                {
                    Combo++;
                    GamePoint += 300;
                    judgeMessage = "300";
                    Health += 60;
                }
                else if (intervel < 60)
                {
                    Combo++;
                    GamePoint += 100;
                    judgeMessage = "100";
                    Health += 40;
                }
                else if (intervel < 90)
                {
                    Combo++;
                    GamePoint += 50;
                    judgeMessage = "50";
                    Health += 20;
                }
                else
                {
                    Combo = 0;
                    judgeMessage = "Bad";
                }
            }
            return;
        }
    }
}
