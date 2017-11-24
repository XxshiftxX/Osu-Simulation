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

        Dictionary<string, string> OsuMetaData = new Dictionary<string, string>();

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
            // 나중에 ReadOsuFile에서 파일 읽어오기
            PlayMusic(@"D:\TestOsu.osu", "Suzumu feat. Kagamine Len - Kakumeisei Ousama Densenbyou.mp3");
            gameStartedPoint = DateTime.Now;

            loopDelegate += () =>
            {
                HitObject checkHitObject = HitObjects.Peek();
                double nowTiming = (DateTime.Now - gameStartedPoint).TotalMilliseconds;
                
                if(checkHitObject.Time <= nowTiming)
                {
                    HitObjects.Pop();
                    GenerateNote(checkHitObject.Line);
                }
            };
        }

        private void GenerateNote(int line)
        {
            throw new NotImplementedException();
        }
    }
}
