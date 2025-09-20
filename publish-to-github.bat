@echo off
echo 🚀 Publishing Chonker Races to GitHub...
echo.

REM Check if git is installed
git --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ Git is not installed or not in PATH
    echo Please install Git from https://git-scm.com/
    pause
    exit /b 1
)

REM Check if we're in a git repository
if not exist ".git" (
    echo 📁 Initializing Git repository...
    git init
    echo ✅ Git repository initialized
)

REM Add all files
echo 📝 Adding files to Git...
git add .

REM Check if there are changes to commit
git diff --cached --quiet
if %errorlevel% equ 0 (
    echo ℹ️  No changes to commit
    echo.
    echo 📋 Next steps:
    echo 1. Create a repository on GitHub
    echo 2. Add the remote: git remote add origin https://github.com/YOUR_USERNAME/Chonker-Races.git
    echo 3. Push to GitHub: git push -u origin main
    echo.
    echo 📖 See GITHUB_PUBLISHING_GUIDE.md for detailed instructions
    pause
    exit /b 0
)

REM Commit changes
echo 💾 Committing changes...
git commit -m "Initial commit: Chonker Races project setup

- Complete Unity racing game project
- Cross-platform support (PC/Android)
- Multiplayer networking with Mirror
- Car physics and controls system
- Power-up and damage systems
- Mobile touch controls
- UI system and race management
- GitHub Actions for automated builds
- Comprehensive documentation"

echo ✅ Changes committed successfully
echo.

REM Check if remote exists
git remote get-url origin >nul 2>&1
if %errorlevel% neq 0 (
    echo 📡 No remote repository configured
    echo.
    echo 📋 Next steps:
    echo 1. Create a repository on GitHub
    echo 2. Add the remote: git remote add origin https://github.com/YOUR_USERNAME/Chonker-Races.git
    echo 3. Push to GitHub: git push -u origin main
    echo.
    echo 📖 See GITHUB_PUBLISHING_GUIDE.md for detailed instructions
) else (
    echo 🚀 Pushing to GitHub...
    git push -u origin main
    if %errorlevel% equ 0 (
        echo ✅ Successfully pushed to GitHub!
        echo.
        echo 🎉 Your project is now live on GitHub!
        echo 📖 Check GITHUB_PUBLISHING_GUIDE.md for next steps
    ) else (
        echo ❌ Failed to push to GitHub
        echo Please check your remote URL and credentials
    )
)

echo.
echo 📚 Documentation:
echo - README.md: Project overview and setup
echo - PROJECT_SETUP.md: Unity setup instructions
echo - GITHUB_PUBLISHING_GUIDE.md: GitHub publishing guide
echo - CONTRIBUTING.md: Contribution guidelines
echo.
pause
