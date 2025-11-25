# Step-by-Step Guide: Offline Speech-to-Text Application

This guide provides detailed, step-by-step instructions for setting up, building, and running the Offline Speech-to-Text application built with C#, Vosk, and ALSA on Linux.

## Prerequisites

### Step 1: Install .NET SDK
The application requires .NET SDK 6.0 or higher. .NET 8 is recommended.

1. Open a terminal.
2. Check if .NET is already installed:
   ```
   dotnet --version
   ```
3. If not installed, install .NET 8:
   ```
   sudo apt update
   sudo apt install dotnet-sdk-8.0
   ```
4. Verify installation:
   ```
   dotnet --version
   ```

### Step 2: Install ALSA Utils
The application uses ALSA for audio recording.

1. Install ALSA utils:
   ```
   sudo apt install alsa-utils
   ```
2. Test microphone access:
   ```
   arecord -l
   ```
   This should list available audio devices. If no devices are listed, check your microphone setup.

### Step 3: Verify PipeWire (Optional but Recommended)
Modern Linux systems use PipeWire for audio management.

1. Check PipeWire status:
   ```
   systemctl --user status pipewire
   ```
2. If not running, start it:
   ```
   systemctl --user start pipewire
   ```

## Download and Setup Vosk Model

### Step 4: Download Vosk Speech Model
The application requires an offline Vosk speech recognition model.

1. Choose a model size:
   - Small model (40MB): https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip
   - Medium model (1GB): https://alphacephei.com/vosk/models/vosk-model-en-us-0.22.zip

2. Download the model using wget or your browser:
   ```
   wget https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip
   ```

3. Unzip the model into the project directory:
   ```
   unzip vosk-model-small-en-us-0.15.zip -d offline-speech-to-text/src/model
   ```
   Note: Remove the version number from the folder name if present, so the path becomes `offline-speech-to-text/src/model/`.

4. Verify the model structure:
   ```
   ls offline-speech-to-text/src/model/
   ```
   You should see directories: `am/`, `conf/`, `graph/`, `ivector/`.

## Build the Project

### Step 5: Add NuGet Packages
The project depends on the Vosk library.

1. Navigate to the source directory:
   ```
   cd offline-speech-to-text/src
   ```

2. Add the Vosk package:
   ```
   dotnet add package Vosk
   ```

### Step 6: Build the Application
1. From the `offline-speech-to-text/src` directory, build the project:
   ```
   dotnet build
   ```
2. Verify the build succeeded (no errors in output).

## Run the Application

### Step 7: Execute the Application
1. Run the application:
   ```
   dotnet run
   ```

2. The console will display:
   ```
   Press ENTER to start recording...
   ```

3. Press ENTER to begin recording.

4. Speak into your microphone. The application will transcribe speech in real-time, displaying text in light blue.

5. Press ENTER again to stop recording.

6. The application will display:
   ```
   Recording stopped. Press ENTER to exit.
   ```

7. Press ENTER to exit the application.

## Customization

### Step 8: Change Text Color (Optional)
The transcription text appears in light blue by default.

1. Open `Program.cs` in a text editor.

2. Locate the line:
   ```
   Console.ForegroundColor = ConsoleColor.Cyan;
   ```

3. Change `ConsoleColor.Cyan` to another color, e.g., `ConsoleColor.Green`.

4. Rebuild and run the application.

## Troubleshooting

### Common Issues

#### Issue: "Device or resource busy"
- **Cause**: PipeWire or ALSA is holding the microphone.
- **Solution**:
  1. Check what's using the audio device:
     ```
     fuser -v /dev/snd/*
     ```
  2. Restart PipeWire:
     ```
     systemctl --user restart pipewire
     ```
  3. Alternatively, specify a different ALSA device:
     ```
     arecord -D plughw:0,0 -f S16_LE -r 16000 -c 1 test.wav
     ```
     Update the code to use the specific device if needed.

#### Issue: No microphone detected
- **Cause**: Microphone not connected or permissions issue.
- **Solution**:
  1. Check device list:
     ```
     arecord -l
     ```
  2. Ensure microphone permissions are granted.
  3. Test recording manually:
     ```
     arecord -f S16_LE -r 16000 -c 1 test.wav
     ```
     Speak for a few seconds, then press Ctrl+C. Play back with `aplay test.wav`.

#### Issue: Build fails
- **Cause**: Missing dependencies or incorrect .NET version.
- **Solution**:
  1. Ensure .NET SDK is installed correctly.
  2. Check for missing packages:
     ```
     dotnet restore
     ```
  3. Verify the project file `offline-speech-to-text.csproj` is correct.

#### Issue: Runtime error starting recording
- **Cause**: ALSA not configured or device unavailable.
- **Solution**:
  1. Install additional ALSA packages if needed:
     ```
     sudo apt install alsa-base alsa-tools
     ```
  2. Check ALSA configuration:
     ```
     alsamixer
     ```
  3. Ensure microphone is not muted.

### Additional Tips
- The application only outputs finalized speech segments, not partial results, for clean transcription.
- For better performance, use the medium model, but it requires more disk space and RAM.
- The application runs entirely offline with no internet required after setup.

## Project Structure
```
offline-speech-to-text/
├── src/
│   ├── Program.cs          # Main application code
│   ├── offline-speech-to-text.csproj  # Project file
│   └── model/              # Vosk speech model (unzipped)
├── bin/                    # Build output (generated)
└── obj/                    # Build intermediates (generated)
```

This completes the setup and usage of the Offline Speech-to-Text application.