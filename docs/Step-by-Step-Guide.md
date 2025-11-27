# Getting Started with Offline Speech-to-Text using Vosk in C# – A Beginner’s Guide

## Title & Objective

**What technology did you choose?**  
Offline Speech-to-Text using the Vosk library in C#.

**Why did you choose it?**  
Vosk was chosen for its offline speech recognition capabilities, allowing the application to work without an internet connection, ensuring privacy and reliability in environments with limited connectivity.

**What’s the end goal?**  
To build and run a console application that records audio from a microphone and transcribes it to text in real-time using offline speech recognition.

## Quick Summary of the Technology

**What is it?**  
Vosk is an open-source toolkit for offline speech recognition that uses deep learning models to convert spoken language into text without requiring an internet connection.

**Where is it used?**  
It is used in applications where privacy is critical, such as secure voice assistants, transcription tools for sensitive data, and embedded systems in IoT devices.

**One real-world example.**  
A voice-controlled home automation system that responds to user commands without sending audio data to external servers.

## System Requirements

- **OS:** Linux (Ubuntu or similar distribution)
- **Tools/Editors required:** VS Code, .NET SDK 8.0 or higher
- **Any packages:** dotnet-sdk-8.0, alsa-utils, wget, unzip

## Installation & Setup Instructions

1. **Install .NET SDK**  
   Open a terminal and run:  
   ```
   sudo apt update
   sudo apt install dotnet-sdk-8.0
   ```  
   Verify with:  
   ```
   dotnet --version
   ```

2. **Install ALSA Utils**  
   Run:  
   ```
   sudo apt install alsa-utils
   ```  
   Test microphone:  
   ```
   arecord -l
   ```

3. **Download Vosk Model**  
   Choose a model (e.g., small: https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip) and download:  
   ```
   wget https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip
   unzip vosk-model-small-en-us-0.15.zip -d offline-speech-to-text/src/model
   ```

4. **Add NuGet Package**  
   Navigate to the project directory:  
   ```
   cd offline-speech-to-text/src
   dotnet add package Vosk
   ```

5. **Build the Project**  
   ```
   dotnet build
   ```

## Minimal Working Example

This example demonstrates a console application that records audio and transcribes it to text using Vosk.

**what the example does.**  
The application waits for user input to start recording, captures audio from the microphone, processes it with Vosk for speech recognition, and outputs the transcribed text in real-time. It stops recording on another key press.

**Code with inline comments.**  
```csharp
using Vosk; // Import Vosk library
using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Initialize Vosk model
        Model model = new Model("model");

        // Create recognizer
        VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);

        // Open audio input
        PortAudioSharp.Stream input = new PortAudioSharp.Stream(1, 0, false, 16000, 1024);

        Console.WriteLine("Press ENTER to start recording...");
        Console.ReadLine();

        input.Start();

        Console.ForegroundColor = ConsoleColor.Cyan; // Set text color

        while (true)
        {
            short[] buffer = new short[1024];
            input.Read(buffer, 0, buffer.Length);

            if (rec.AcceptWaveform(buffer, buffer.Length))
            {
                string result = rec.Result();
                Console.WriteLine(result); // Output transcribed text
            }
        }

        input.Stop();
        Console.ResetColor();
        Console.WriteLine("Recording stopped. Press ENTER to exit.");
        Console.ReadLine();
    }
}
```

**Expected output.**  
After pressing ENTER to start, speak into the microphone. The console displays transcribed text in light blue. Press ENTER again to stop, then exit.

## AI Prompt Journal

what is C# as a language?
What do i need to install in my system?
What packages do i need to install in my project folder?
Apart from NAudio and PortAudio which other packages can be used on ubuntu?
Why am i not able to record using my mic?
Instructions to run the app?
How can i deploy this Project?



## Common Issues & Fixes

- **"Device or resource busy"**  
  Cause: Audio device in use.  
  Fix: Restart PipeWire with `systemctl --user restart pipewire` or specify device in code.

- **No microphone detected**  
  Cause: Hardware or permissions issue.  
  Fix: Check with `arecord -l`, ensure permissions, test with `arecord -f S16_LE -r 16000 -c 1 test.wav`.

- **Build fails**  
  Cause: Missing .NET or packages.  
  Fix: Run `dotnet restore`, verify SDK installation.

- **Runtime error**  
  Cause: ALSA misconfiguration.  
  Fix: Install `alsa-base alsa-tools`, check with `alsamixer`.

Links: Vosk GitHub (https://github.com/alphacep/vosk-api), .NET docs (https://docs.microsoft.com/en-us/dotnet/).

## References

- Official Vosk Documentation: https://alphacephei.com/vosk/
- .NET SDK Installation: https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu
- ALSA Project: https://www.alsa-project.org/
- Helpful Blog: https://medium.com/@alphacep/offline-speech-recognition-with-vosk-6c3a15e6eabf