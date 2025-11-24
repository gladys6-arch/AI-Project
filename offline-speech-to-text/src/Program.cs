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
            Console.WriteLine("Make sure your Vosk model folder is in the project root as 'model/'");
            Console.WriteLine();

            string modelPath = "model";
            if (!Directory.Exists(modelPath))
            {
                Console.WriteLine($"Model folder '{modelPath}' not found. Please download and extract a Vosk model into the '{modelPath}' folder.");
                return;
            }

            // Vosk works best with 16kHz mono models; if your model uses a different sample rate,
            // pass that sample rate into VoskRecognizer below.
            const int sampleRate = 16000; // recommended for many Vosk English models

            // Initialize model
            using var model = new Model(modelPath);

            // Create recognizer with the sample rate
            using var recognizer = new VoskRecognizer(model, sampleRate);

            // Configure the microphone capture
            using var waveIn = new WaveInEvent();

            // Request 16kHz mono, 16-bit PCM — Vosk expects PCM16
            waveIn.WaveFormat = new WaveFormat(sampleRate, 16, 1);

            // Hook the data available event
            waveIn.DataAvailable += (sender, e) =>
            {
                try
                {
                    // e.Buffer is a byte[] containing PCM16 data, e.BytesRecorded is bytes count
                    if (e.BytesRecorded == 0) return;

                    // Pass the raw bytes to Vosk recognizer
                    // AcceptWaveform(byte[] data, int len) returns true when a final result is ready
                    if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                    {
                        // Final (segment) result
                        Console.WriteLine(recognizer.Result());
                    }
                    else
                    {
                        // Partial (intermediate) result
                        Console.WriteLine(recognizer.PartialResult());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"(capture error) {ex.Message}");
                }
            };

            waveIn.RecordingStopped += (s, e) =>
            {
                if (e.Exception != null)
                {
                    Console.WriteLine($"Recording stopped due to an error: {e.Exception.Message}");
                }
                else
                {
                    Console.WriteLine("Recording stopped.");
                }
            };

            // Start recording
            try
            {
                waveIn.StartRecording();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not start recording: {ex.Message}");
                return;
            }

            Console.WriteLine("Recording... speak into the microphone.");
            Console.WriteLine("Press ENTER to stop.");

            // Block until user presses Enter
            Console.ReadLine();

            // Stop and cleanup
            try
            {
                waveIn.StopRecording();
            }
            catch { /* ignore */ }

            // Print final result (if any buffered)
            Console.WriteLine("Final result:");
            Console.WriteLine(recognizer.FinalResult());

            Console.WriteLine("Done.");
        }
    }
}
