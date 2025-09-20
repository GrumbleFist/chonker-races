# 🎮 Unity Setup and Game Launch Guide

This guide will walk you through getting Unity working and running your Chonker Races game.

## ✅ **What You Already Have:**
- ✅ Unity Hub installed
- ✅ Unity Editor 6000.2.5f1 installed
- ✅ All game scripts and files ready

## 🚀 **Step-by-Step Setup:**

### **Step 1: Open Unity Hub**
1. **Press Windows key** and type "Unity Hub"
2. **Click** on Unity Hub to open it
3. **Sign in** with your Unity account (or create one if needed)

### **Step 2: Create New Project**
1. **Click** "New Project" in Unity Hub
2. **Select** "3D (Built-in Render Pipeline)" template
3. **Name** your project: "Chonker Races"
4. **Choose** a location (default is fine)
5. **Click** "Create Project"

### **Step 3: Import Your Game Files**
1. **Wait** for Unity to create the project
2. **Copy** all files from your current folder to the new Unity project
3. **Or** replace the Assets folder in Unity with the one we created

### **Step 4: Install Required Packages**
1. **In Unity**, go to **Window → Package Manager**
2. **Click** the "+" button
3. **Select** "Add package from git URL"
4. **Install these packages one by one:**

   **Input System:**
   ```
   com.unity.inputsystem
   ```

   **TextMeshPro:**
   ```
   com.unity.textmeshpro
   ```

   **Mirror Networking:**
   ```
   https://github.com/vis2k/Mirror.git?path=/Assets/Mirror
   ```

### **Step 5: Quick Setup Your Game**
1. **In Unity**, go to **Chonker Races → Quick Setup**
2. **Click** "Create Basic Racing Scene"
3. **Wait** for the scene to be created

### **Step 6: Play Your Game!**
1. **Click** the Play button (▶️) in Unity
2. **Use WASD** or **Arrow Keys** to drive
3. **Press Space** for handbrake
4. **Press Shift** for boost

## 🎯 **Controls:**
- **W/↑** - Accelerate
- **S/↓** - Brake/Reverse
- **A/←** - Turn Left
- **D/→** - Turn Right
- **Space** - Handbrake
- **Shift** - Boost
- **R** - Restart
- **Escape** - Quit

## 🔧 **Troubleshooting:**

### **If Unity Won't Open:**
- Make sure Unity Hub is running
- Try running Unity Hub as administrator
- Check that you have enough disk space

### **If Packages Won't Install:**
- Make sure you have internet connection
- Try restarting Unity
- Check the Console for error messages

### **If Game Won't Run:**
- Make sure all packages are installed
- Check that the car has the CarController script
- Verify that the scene has a RaceManager

## 🎮 **Building Your Game:**

### **For Windows:**
1. **Go to** Chonker Races → Quick Build
2. **Click** "Build for Windows"
3. **Your game** will be built in the Builds folder
4. **Double-click** ChonkerRaces.exe to play!

### **For Android:**
1. **Go to** Chonker Races → Quick Build
2. **Click** "Build for Android"
3. **Install** the .apk file on your device

## 🆘 **Need Help?**

### **Check Package Status:**
- Go to **Chonker Races → Check Packages**
- Make sure all packages show ✅

### **Auto Package Installer:**
- Go to **Chonker Races → Auto Package Installer**
- Click "Install All Required Packages"

### **Console Errors:**
- Go to **Window → General → Console**
- Look for red error messages
- Fix any issues before playing

## 🎉 **You're Ready to Race!**

Once everything is set up, you can:
- 🏎️ **Drive** around the track
- 🚀 **Collect** power-ups
- 💥 **Take damage** and repair
- 👥 **Play** multiplayer (when set up)
- 📱 **Build** for mobile devices

**Happy Racing!** 🏁✨
