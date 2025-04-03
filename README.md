# YTDownloader
Download videos and playlists from YouTube

You have to download ffmpeg to your computer

# FFmpeg Installation Guide

FFmpeg is a powerful multimedia framework that allows you to decode, encode, transcode, mux, demux, stream, and play almost anything. This guide will help you install FFmpeg on Windows.

## Installation Steps

### 1. Download FFmpeg
- Go to the official FFmpeg website: [https://ffmpeg.org/download.html](https://ffmpeg.org/download.html)
- Under **Get packages & executable files**, click on **Windows**.
- Choose a build from one of the recommended providers (e.g., gyan.dev or BtbN).
- Download the latest **"full"** version.

### 2. Extract the Files
- Once the ZIP file is downloaded, extract it using **WinRAR** or **7-Zip**.
- Move the extracted folder to `C:\ffmpeg` for easier access.

### 3. Set Environment Variables
- Open **Start Menu**, search for **Environment Variables**, and open **Edit the system environment variables**.
- Click **Environment Variables**.
- In the **System variables** section, find and select **Path**, then click **Edit**.
- Click **New** and add the following path: `C:\ffmpeg\bin`
- Click **OK** to save the changes.

### 4. Verify the Installation
- Open **Command Prompt** (Win + R, type `cmd`, and press Enter).
- Type the following command:
````sh
ffmpeg -version