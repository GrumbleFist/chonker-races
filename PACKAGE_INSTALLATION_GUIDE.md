# 📦 Unity Package Installation Guide

This guide will help you install all the required packages for Chonker Races.

## 🎯 **Required Packages**

1. **Input System** - For cross-platform controls
2. **TextMeshPro** - For better text rendering
3. **Mirror Networking** - For multiplayer functionality

## 📋 **Step-by-Step Installation**

### **Step 1: Open Package Manager**

1. **In Unity**, go to **Window → Package Manager**
2. **Wait** for the Package Manager window to open
3. **Make sure** "In Project" is selected in the dropdown (top left)

### **Step 2: Install Input System**

#### **Method A: From Git URL (Recommended)**
1. **Click** the **"+"** button in Package Manager (top left)
2. **Select** "Add package from git URL"
3. **Type**: `com.unity.inputsystem`
4. **Click** "Add"
5. **If prompted** about enabling the new Input System:
   - Click **"Yes"**
   - Unity will restart (this is normal)

#### **Method B: From Package Manager**
1. **In Package Manager**, look for **"Input System"** in the list
2. **Click** on it
3. **Click** "Install" button

### **Step 3: Install TextMeshPro**

#### **Method A: From Git URL (Recommended)**
1. **Click** the **"+"** button in Package Manager (top left)
2. **Select** "Add package from git URL"
3. **Type**: `com.unity.textmeshpro`
4. **Click** "Add"
5. **If prompted** to import TMP Essentials:
   - Click **"Import TMP Essentials"**
   - This adds default fonts and materials

#### **Method B: From Package Manager**
1. **In Package Manager**, look for **"TextMeshPro"** in the list
2. **Click** on it
3. **Click** "Install" button

### **Step 4: Install Mirror Networking**

#### **Method A: From Git URL (Recommended)**
1. **Click** the **"+"** button in Package Manager (top left)
2. **Select** "Add package from git URL"
3. **Type**: `https://github.com/vis2k/Mirror.git?path=/Assets/Mirror`
4. **Click** "Add"
5. **Wait** for the installation to complete

#### **Method B: Download and Import**
1. **Go to**: https://mirror-networking.com/
2. **Click** "Download Mirror"
3. **Download** the latest version (usually a .unitypackage file)
4. **In Unity**, go to **Assets → Import Package → Custom Package**
5. **Select** the downloaded file and click **"Import"**

## ✅ **Verify Installation**

### **Using the Package Checker**
1. **In Unity**, go to **Chonker Races → Check Packages**
2. **Look** for the Package Checker window
3. **Check** that all packages show ✅ (green checkmarks)

### **Manual Verification**
1. **Input System**: Look for "Input System" in Package Manager
2. **TextMeshPro**: Look for "TextMeshPro" in Package Manager
3. **Mirror**: Look for "Mirror" folder in your Project window

## 🐛 **Troubleshooting**

### **Input System Issues**
- **Problem**: "Input System not found"
- **Solution**: Make sure you enabled the new Input System when prompted
- **Fix**: Go to Edit → Project Settings → XR Plug-in Management → Input System Package

### **TextMeshPro Issues**
- **Problem**: "TextMeshPro not found"
- **Solution**: Make sure you imported TMP Essentials
- **Fix**: Go to Window → TextMeshPro → Import TMP Essentials

### **Mirror Issues**
- **Problem**: "Mirror not found"
- **Solution**: Make sure you imported the .unitypackage file
- **Fix**: Check that the Mirror folder exists in your Project window

## 📱 **Additional Packages (Optional)**

### **For Mobile Development**
- **Android Build Support**: For building Android games
- **iOS Build Support**: For building iOS games

### **For Better Graphics**
- **Post Processing**: For better visual effects
- **Cinemachine**: For advanced camera control

## 🎯 **Next Steps**

After installing all packages:

1. **Run** the Package Checker to verify installation
2. **Use** the Quick Setup tool to create your first scene
3. **Start** building your racing game!

## 🆘 **Need Help?**

If you're having trouble:
- **Check** the Unity Console for error messages
- **Restart** Unity if packages don't appear
- **Make sure** you have a stable internet connection
- **Check** that you have enough disk space

## 📚 **Useful Links**

- **Unity Package Manager**: https://docs.unity3d.com/Manual/PackageManager.html
- **Input System**: https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/
- **TextMeshPro**: https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest/
- **Mirror Networking**: https://mirror-networking.com/

---

**Happy Package Installing!** 📦✨
