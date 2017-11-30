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
        Queue<HitObject>[] CreatedHitObjects = new Queue<HitObject>[4]
        {
            new Queue<HitObject>(), new Queue<HitObject>(), new Queue<HitObject>(), new Queue<HitObject>()
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
            PlayMusic(@"F:\294951 LEDMaster - Chrono Diver -PENDULUMs-", @"F:\294951 LEDMaster - Chrono Diver -PENDULUMs-\Chrono Diver -PENDULUMs-.mp3");
            gameStartedPoint = DateTime.Now;

            loopDelegate += () =>
            {
                double nowTiming = (DateTime.Now - gameStartedPoint).TotalMilliseconds;
                if (HitObjects.Count > 0)
                {
                    HitObject checkHitObject = HitObjects.Peek();

                    if (checkHitObject.StartTime <= nowTiming + sync)
                    {
                        DisplayingHitObjects.Add(checkHitObject);
                        CreatedHitObjects[HitObjects.Pop().Line].Enqueue(checkHitObject);
                        loopDelegate();
                    }
                }

                for (int i = 0; i < CreatedHitObjects.Length; i++)
                {
                    if (CreatedHitObjects[i].Count == 0)
                        continue;

                    HitObject temp = CreatedHitObjects[i].Peek();
                    if (temp.StartTime + 100 < nowTiming)
                    {
                        Combo = 0;
                        Health -= 50;
                        judgeMessage = "MISS!";
                        DisplayingHitObjects.Remove(temp);
                        CreatedHitObjects[i].Dequeue();
                    }
                }

                /*
                if (DisplayingHitObjects.Count > 0 && DisplayingHitObjects[0].Time <= nowTiming)
                {
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
            double nowTiming = (DateTime.Now - gameStartedPoint).TotalMilliseconds;

            if (CreatedHitObjects[input].Count <= 0)
            {
                return;
            }
            intervel = (int)((inputTime - gameStartedPoint).TotalMilliseconds) - CreatedHitObjects[input].Peek().StartTime;
            if (intervel < -100)
                return;

            DisplayingHitObjects.Remove(CreatedHitObjects[input].Dequeue());

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
