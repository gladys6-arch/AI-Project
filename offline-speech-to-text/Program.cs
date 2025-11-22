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
            Console.WriteLine("Starting Vosk Speech-to-Text...");

            // Paths
            string modelPath = "model";      // Your Vosk model folder
            string audioFile = "test.wav";   // Your audio file

            // Check if model and audio file exist
            if (!Directory.Exists(modelPath))
            {
                Console.WriteLine($"Model folder '{modelPath}' not found. Please download a Vosk model and place it here.");
                return;
            }

            if (!File.Exists(audioFile))
            {
                Console.WriteLine($"Audio file '{audioFile}' not found.");
                return;
            }

            // Optional GPU initialization (comment out if no GPU support)
            // Vosk.Vosk.GpuInit();

            try
            {
                // Initialize Vosk model
                using var model = new Model(modelPath);

                // Open WAV file
                using var waveReader = new WaveFileReader(audioFile);

                // Initialize recognizer with sample rate
                using var recognizer = new VoskRecognizer(model, waveReader.WaveFormat.SampleRate);

                byte[] buffer = new byte[4096];
                int bytesRead;

                // Process audio
                while ((bytesRead = waveReader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (recognizer.AcceptWaveform(buffer, bytesRead))
                    {
                        Console.WriteLine(recognizer.Result());
                    }
                    else
                    {
                        Console.WriteLine(recognizer.PartialResult());
                    }
                }

                // Print final result
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
