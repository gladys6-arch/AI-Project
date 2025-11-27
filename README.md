# Offline Speech-to-Text (C# + Vosk + ALSA)

A fully offline, real-time speech-to-text application built using .NET, Vosk, and ALSA arecord on Linux.
The app captures live microphone audio and streams it to Vosk, outputting text continuously in a Google Docs–style paragraph, with optional colored transcription text.

Perfect for developers who want fast, private, offline speech recognition.

## Features

- Real-time microphone transcription

- 100% offline — no cloud APIs

- Powered by Vosk (Kaldi backend)

- Output displays as natural flowing paragraph

- Supports colored text output (light blue by default)

- Designed for Linux (Ubuntu + ALSA)

- Easy to fork and run

## Dependencies
System Requirements

Before running the project, ensure you have the following installed:

### 1. .NET SDK 6.0+
   - Check with: `dotnet --version`
   - Install (.NET 8 recommended): `sudo apt install dotnet-sdk-8.0`

### 2. ALSA (Audio Recording)
   The app uses arecord to capture microphone audio.
   - Install ALSA: `sudo apt install alsa-utils`
   - Test microphone: `arecord -l`

### 3. PipeWire (optional but recommended)
   Most modern Linux systems use PipeWire.
   - Verify: `systemctl --user status pipewire`

### 4. Vosk Model (Offline Speech Model)
   Download an English model:
   - Small model (40MB): `https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip`
   - Medium model (1GB): `https://alphacephei.com/vosk/models/vosk-model-en-us-0.22.zip`
   - Unzip into: `/offline-speech-to-text/model/`
   - You should end up with: `model/am/final.mdl`, `model/graph/...`, `model/ivector/...`

### 5. NuGet Packages
   - Inside the project folder run: `dotnet add package Vosk`

📁 Project Structure
offline-speech-to-text/
│
├── src/
│   ├── Program.cs
│   └── ...
│
├── model/
│   └── (unzipped Vosk model)
│
└── README.md

### Running the Project

Navigate to src/ then run:
```
dotnet run
```


You will see:

Press ENTER to start recording...


Hit ENTER, then speak.

To stop recording, press ENTER again.

### Text Color (Optional)

Speech output is printed in light blue using:

Console.ForegroundColor = ConsoleColor.Cyan;
Console.ResetColor();


You can change the color easily:

Console.ForegroundColor = ConsoleColor.Green;

### How It Works

- App loads the offline Vosk model

- Uses arecord to stream 16kHz mono audio

- Sends raw PCM bytes to VoskRecognizer

- When Vosk finalizes a phrase, it prints the text in a continuous flow

- No partial updates — only clean, stable transcription

### Troubleshooting
Device or resource busy

PipeWire/ALSA may be holding your mic.

Run:
```
fuser -v /dev/snd/*
```


If pipewire blocks PCM input:

```
systemctl --user restart pipewire
```


Or specify another ALSA device:

arecord -D plughw:0,0 ...

## Deployment cannot be done because

My project is a desktop/server-side C# console application that:

- accesses the microphone hardware

- runs real-time audio capture (arecord)

- loads a local Vosk model (hundreds of MBs)

- does offline speech recognition with access to /dev/snd/*

Vercel or Render cannot:

- access microphone hardware

- run background audio services

- run non-web console apps

- open /dev/snd audio devices

- handle long-running processes

- ship huge model files



## License

copyright (c) <2025> <copyright Gladys Achando>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.