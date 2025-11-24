using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using Vosk;

class Program
{
    static void Main()
    {
        Console.WriteLine("Press ENTER to start recording...");
        Console.ReadLine();

        // Load the Vosk model
        var model = new Model("model");
        var recognizer = new VoskRecognizer(model, 16000); // 16kHz mono

        Console.WriteLine("Recording... Press ENTER again to stop.");

        // Start recording from default ALSA device
        var record = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "arecord",
                Arguments = "-f S16_LE -r 16000 -c 1 -D default",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                RedirectStandardError = false
            }
        };

        try
        {
            record.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error starting recording: " + e.Message);
            return;
        }

        var stream = record.StandardOutput.BaseStream;

        // Thread to stop recording on ENTER
        bool stop = false;
        new Thread(() =>
        {
            Console.ReadLine();
            stop = true;
        }).Start();

        byte[] buffer = new byte[4096];

        while (!stop)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                // AcceptWaveform returns true only for finalized segments
                if (recognizer.AcceptWaveform(buffer, bytesRead))
                {
                    string finalText = ExtractText(recognizer.Result());
                    if (!string.IsNullOrWhiteSpace(finalText))
                    {
                        Console.Write(finalText + " "); // append live to current line
                    }
                }
                else
                {
                    // Skip partial results — no overwriting
                    continue;
                }
            }
        }

        // Stop recording
        try
        {
            record.Kill();
            record.WaitForExit();
        }
        catch { }

        Console.WriteLine("\nRecording stopped. Press ENTER to exit.");
        Console.ReadLine();
    }

    static string ExtractText(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("text", out var textElement))
            {
                return textElement.GetString() ?? "";
            }
        }
        catch { }
        return "";
    }
}

