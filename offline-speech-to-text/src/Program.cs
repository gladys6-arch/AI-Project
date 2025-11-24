using System;
using System.IO;
using Vosk;
using NAudio.Wave;

namespace SpeechToTextDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting LIVE Microphone Speech-to-Text (Vosk + NAudio)");

            // Automatically detect the model folder
            string projectRoot = AppContext.BaseDirectory;

            // Small model folder (make sure you renamed it to 'model')
            string modelPath = Path.Combine(projectRoot, "..", "..", "..", "model");

            modelPath = Path.GetFullPath(modelPath); // absolute path

            Console.WriteLine($"Looking for model folder at: {modelPath}");

            if (!Directory.Exists(modelPath))
            {
                Console.WriteLine($"Model folder not found at '{modelPath}'. Please extract vosk-model-small-en-us-0.15 into this location and rename it 'model'.");
                return;
            }

            const int sampleRate = 16000; // recommended for most Vosk English models

            try
            {
                using var model = new Model(modelPath);
                using var recognizer = new VoskRecognizer(model, sampleRate);
                using var waveIn = new WaveInEvent();

                waveIn.WaveFormat = new WaveFormat(sampleRate, 16, 1); // 16-bit mono

                waveIn.DataAvailable += (sender, e) =>
                {
                    if (e.BytesRecorded == 0) return;

                    if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                        Console.WriteLine(recognizer.Result());
                    else
                        Console.WriteLine(recognizer.PartialResult());
                };

                waveIn.RecordingStopped += (s, e) =>
                {
                    if (e.Exception != null)
                        Console.WriteLine($"Recording stopped due to an error: {e.Exception.Message}");
                    else
                        Console.WriteLine("Recording stopped.");
                };

                waveIn.StartRecording();
                Console.WriteLine("Recording... speak into the microphone.");
                Console.WriteLine("Press ENTER to stop.");
                Console.ReadLine();

                waveIn.StopRecording();
                Console.WriteLine("Final result:");
                Console.WriteLine(recognizer.FinalResult());
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
