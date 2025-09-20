# Contributing to Chonker Races

Thank you for your interest in contributing to Chonker Races! This document provides guidelines and information for contributors.

## 🚀 Getting Started

### Prerequisites
- Unity 2022.3 LTS or later
- Git
- Basic knowledge of Unity and C#
- GitHub account

### Setting Up Development Environment

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/Chonker-Races.git
   cd Chonker-Races
   ```
3. **Open the project** in Unity 2022.3 LTS
4. **Install required packages** (see PROJECT_SETUP.md)
5. **Create a new branch** for your feature:
   ```bash
   git checkout -b feature/your-feature-name
   ```

## 🎯 How to Contribute

### Types of Contributions

We welcome various types of contributions:

- **🐛 Bug Fixes**: Fix existing issues
- **✨ New Features**: Add new functionality
- **📚 Documentation**: Improve documentation
- **🎨 Art Assets**: Create 3D models, textures, sounds
- **🎮 Game Design**: Design new tracks, power-ups, game modes
- **🔧 Performance**: Optimize code and assets
- **📱 Mobile**: Improve mobile experience
- **🌐 Networking**: Enhance multiplayer features

### Contribution Process

1. **Check existing issues** and pull requests
2. **Create an issue** if you're planning a large change
3. **Make your changes** following our coding standards
4. **Test your changes** thoroughly
5. **Submit a pull request** with a clear description

## 📝 Coding Standards

### C# Code Style
- Use PascalCase for public methods and properties
- Use camelCase for private fields and local variables
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Keep methods focused and under 50 lines when possible

### Unity Best Practices
- Use `[SerializeField]` for private fields that need to be exposed in inspector
- Use `[Header]` attributes to organize inspector fields
- Prefer composition over inheritance
- Use object pooling for frequently instantiated objects
- Optimize for mobile platforms

### Example Code Structure
```csharp
namespace ChonkerRaces
{
    /// <summary>
    /// Handles car physics and controls
    /// </summary>
    public class CarController : MonoBehaviour
    {
        [Header("Car Settings")]
        [SerializeField] private float motorForce = 1500f;
        [SerializeField] private float brakeForce = 3000f;
        
        private Rigidbody carRigidbody;
        
        /// <summary>
        /// Applies motor force to the car
        /// </summary>
        /// <param name="force">The force to apply</param>
        public void ApplyMotorForce(float force)
        {
            // Implementation
        }
    }
}
```

## 🎮 Game Design Guidelines

### Track Design
- Keep tracks challenging but fair
- Include multiple racing lines
- Add visual variety and landmarks
- Test with different car speeds
- Ensure mobile-friendly track width

### Power-ups
- Make effects clear and understandable
- Balance power-up frequency
- Provide visual and audio feedback
- Consider multiplayer balance

### UI/UX
- Keep interfaces simple and intuitive
- Ensure mobile responsiveness
- Use consistent color schemes
- Provide clear feedback for user actions

## 🧪 Testing

### Testing Checklist
- [ ] Test on PC (Windows/Mac/Linux)
- [ ] Test on Android device
- [ ] Test multiplayer functionality
- [ ] Test with different input methods
- [ ] Verify performance on lower-end devices
- [ ] Check for memory leaks
- [ ] Test edge cases and error conditions

### Performance Guidelines
- Maintain 60 FPS on target devices
- Keep memory usage under 1GB
- Optimize for mobile battery life
- Use LOD groups for distant objects
- Implement object pooling where appropriate

## 📋 Pull Request Guidelines

### Before Submitting
- [ ] Code follows our style guidelines
- [ ] All tests pass
- [ ] Documentation is updated
- [ ] No console errors or warnings
- [ ] Performance impact is considered

### Pull Request Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Tested on PC
- [ ] Tested on Android
- [ ] Tested multiplayer
- [ ] Performance tested

## Screenshots/Videos
(If applicable)

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] No breaking changes
```

## 🐛 Reporting Issues

### Bug Reports
When reporting bugs, please include:
- Unity version
- Platform (PC/Android)
- Steps to reproduce
- Expected vs actual behavior
- Screenshots/videos if applicable
- Console logs if available

### Feature Requests
For feature requests, please include:
- Clear description of the feature
- Use case and motivation
- Mockups or examples if applicable
- Consideration of implementation complexity

## 🏷️ Issue Labels

We use the following labels:
- `bug`: Something isn't working
- `enhancement`: New feature or request
- `documentation`: Improvements to documentation
- `good first issue`: Good for newcomers
- `help wanted`: Extra attention is needed
- `mobile`: Mobile-specific issues
- `multiplayer`: Networking-related
- `performance`: Performance improvements
- `ui/ux`: User interface improvements

## 🎨 Asset Guidelines

### 3D Models
- Use low-poly style for performance
- Optimize for mobile devices
- Include LOD versions
- Use standard Unity materials

### Textures
- Use power-of-2 dimensions
- Compress appropriately for platform
- Maintain consistent art style
- Use texture atlases when possible

### Audio
- Use compressed formats (OGG, MP3)
- Keep file sizes reasonable
- Provide multiple quality levels
- Include audio credits

## 📞 Getting Help

- **Discord**: Join our community server
- **Issues**: Use GitHub issues for bugs and features
- **Discussions**: Use GitHub discussions for questions
- **Wiki**: Check our wiki for detailed guides

## 🏆 Recognition

Contributors will be recognized in:
- README.md contributors section
- Game credits
- Release notes
- Community highlights

## 📄 License

By contributing to Chonker Races, you agree that your contributions will be licensed under the MIT License.

## 🙏 Thank You

Thank you for contributing to Chonker Races! Your contributions help make this project better for everyone.

Happy racing! 🏎️
