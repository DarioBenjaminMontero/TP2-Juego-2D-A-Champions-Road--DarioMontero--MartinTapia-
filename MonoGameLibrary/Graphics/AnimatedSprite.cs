using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public class AnimatedSprite : Sprite 
{
    private int _currentFrame;
    private TimeSpan _elapsed;
    private Animation _animation;
    private bool _isFinished;

    public bool IsLooping { get; set; } = true;

    public Animation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            Reset();
        }
    }

    public AnimatedSprite() { }

    public AnimatedSprite(Animation animation, bool isLooping = true)
    {
        IsLooping = isLooping;
        Animation = animation;
    }

    public void Reset()
    {
        _currentFrame = 0;
        _elapsed = TimeSpan.Zero;
        _isFinished = false;

        if (_animation != null && _animation.Frames.Count > 0)
        {
            Region = _animation.Frames[0];
        }
    } 

    public bool IsFinished => !IsLooping && _isFinished;

    public void Update(GameTime gameTime)
    {
        if (_animation == null || (_isFinished && !IsLooping)) return;

        _elapsed += gameTime.ElapsedGameTime;

        while (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;

            if (IsLooping)
            {
                _currentFrame = (_currentFrame + 1) % _animation.Frames.Count;
                Region = _animation.Frames[_currentFrame];
            }
            else
            {
                if (_currentFrame < _animation.Frames.Count - 1)
                {
                    _currentFrame++;
                    Region = _animation.Frames[_currentFrame];
                }
                else
                {
                    // Si ya estábamos en el último frame y pasó su delay, marcamos como terminado
                    _isFinished = true;
                    break;
                }
            }
        }
    }
}