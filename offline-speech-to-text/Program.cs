using System;
using System.IO;
using Vosk;

class.Program
{
  static void Main(string[] args)
  {
    // path for Vosk Model
    string modelPath = "models/vosk-model-small-en-us-0.15";
    //path for wav file
    string WAVPath = "audio/test.wav";

    // intialize Vosk model

        Vosk.Vosk.SetLogLevel(0); 
        Model model = new Model(modelPath);
        VoskRecognizer recognizer = new VoskRecognizer(model, 16000.0f);



     4. Read the WAV file and feed it to the recognizer
      
        // We read the file in small chunks of 4096 bytes
        using (FileStream fs = new FileStream(wavPath, FileMode.Open))
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                // Send the audio chunk to the recognizer
                recognizer.AcceptWaveform(buffer, bytesRead);
            }
        }  


         // 5. Get the final recognized text
        
        string result = recognizer.FinalResult();

        // -------------------------------
        // 6. Display the result
        
        Console.WriteLine("Recognized text:");
        Console.WriteLine(result); 


  }
}
