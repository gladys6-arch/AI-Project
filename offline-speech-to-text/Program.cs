using System;
using System.IO;
using Vosk;

class.Program
{
  static void Main(string[] args)
  {
    // path for Vosk Model
    string modelPath = "models/vosk-model-small-en-us-0.15";
    //pat for wav file
    string WAVPath = "audio/test.wav";

    // intialize Vosk model

        Vosk.Vosk.SetLogLevel(0); 
        Model model = new Model(modelPath);
        VoskRecognizer recognizer = new VoskRecognizer(model, 16000.0f);



        


  }
}
